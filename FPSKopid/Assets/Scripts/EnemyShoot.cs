using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class EnemyShoot : MonoBehaviour
{
    public GameObject bullet;
    public Transform attackPoint;
    public GameObject player;
    public float shootForce;

    private void Awake()
    {
        
    }

    private void Start()
    {
        InvokeRepeating("Shoot", 0, 1.0f);
    }

    private void Shoot()
    {
        GameObject currentBullet = Instantiate(bullet, attackPoint.position, 
                                                Quaternion.identity);
        Vector3 targetPoint;
        targetPoint = player.transform.position;
        Vector3 direction = targetPoint - attackPoint.position;
        currentBullet.GetComponent<Rigidbody>().AddForce(direction.normalized * shootForce, ForceMode.Impulse);
    }
}
