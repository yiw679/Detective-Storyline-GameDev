using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bullet;
    public float range = 20f;
    public float fireTime = 10f;

    private float timer;
    private Transform target;

    private void Start()
    {
        timer = fireTime;
    }

    private void Update()
    {
        timer += Time.deltaTime;
    }

    private void Shoot(Transform target)
    {
        if (Vector3.Distance(target.position, transform.position) <= range)
        {
            if (timer > fireTime)
            {
                timer = 0f;
                GameObject bul = Instantiate(bullet, transform) as GameObject;
            }
        }
    }
}
