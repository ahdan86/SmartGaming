using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class EnemyAgent : Agent
{
    public int score = 0;
    public float speed = 3f;
    public float rotationSpeed = 3f;

    public Transform attackPoint;
    public int minStepsBetweenShots = 50;
    public float damage = 15f;

    public GameObject bullet;

    private bool ShotAvailable = true;
    private int StepsUntilShotIsAvailable = 0;

    private Vector3 StartingPosition;
    private Rigidbody Rb;
    private EnvironmentParameters EnvironmentParameters;
    public float shootForce;

    public event Action OnEnvironmentReset;

    private void Shoot()
    {
        if (!ShotAvailable)
            return;

        var layerMask = 1 << LayerMask.NameToLayer("Enemy");
        var direction = transform.forward;

        GameObject currentBullet = Instantiate(bullet, attackPoint.position,
                                                Quaternion.Euler(0f, -90f, 0f));
        currentBullet.GetComponent<Rigidbody>().AddForce(direction.normalized * shootForce, ForceMode.Impulse);

        Debug.DrawRay(transform.position, direction, Color.blue, 1f);

        if (!Physics.Raycast(attackPoint.position, direction, out var hit, 200f, layerMask))
        {
            AddReward(-0.033f);
        }

        ShotAvailable = false;
        StepsUntilShotIsAvailable = minStepsBetweenShots;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(ShotAvailable);
    }

    private void FixedUpdate()
    {
        if (!ShotAvailable)
        {
            StepsUntilShotIsAvailable--;

            if (StepsUntilShotIsAvailable <= 0)
                ShotAvailable = true;
        }
        AddReward(-1f / MaxStep);
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        if (Mathf.RoundToInt(vectorAction[0]) >= 1)
        {
            Shoot();
        }

        Rb.velocity = new Vector3(vectorAction[1] * speed, 0f, vectorAction[2] * speed);
        transform.Rotate(Vector3.up, vectorAction[3] * rotationSpeed);
    }

    public override void Initialize()
    {
        StartingPosition = transform.position;
        Rb = GetComponent<Rigidbody>();

        //TODO: Delete
        Rb.freezeRotation = true;
        EnvironmentParameters = Academy.Instance.EnvironmentParameters;
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = Input.GetKey(KeyCode.P) ? 1f : 0f;
        actionsOut[2] = Input.GetAxis("Horizontal");
        //actionsOut[1] = -Input.GetAxis("Vertical");
        actionsOut[3] = Input.GetAxis("Vertical");
    }

    public override void OnEpisodeBegin()
    {
        OnEnvironmentReset?.Invoke();

        //Load Parameter from Curciulum
        minStepsBetweenShots = Mathf.FloorToInt(EnvironmentParameters.GetWithDefault("shootingFrequenzy", 30f));

        transform.position = StartingPosition;
        Rb.velocity = Vector3.zero;
        ShotAvailable = true;
    }

    public void RegisterKill()
    {
        score++;
        AddReward(1.0f / EnvironmentParameters.GetWithDefault("amountZombies", 4f));
    }
}
