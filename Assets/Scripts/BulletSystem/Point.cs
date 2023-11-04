using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    // ATTRIBUTES

    // Bullet Prefab
    [SerializeField] private EnemyBullet Shot;
    [Range(0.1f,30.0f)] public float ShotSpeed;
    [Range(-10.0f,10.0f)] public float localOffset = 0;

    [Range(50f,0.1f)] public float shotRate = 5;
    [Range(0,100)] public int pauseRate;
    [Range(0.0f,20.0f)] public float pauseLength;

    private bool canShoot = true;
    public float nextPauseTime;

    // METHODS

    private void Start()
    {
        CalculateNextPauseTime();
    }

    private void Update() {
        if(canShoot){
            StartCoroutine(ShootCoroutine());
        }
    }

    private IEnumerator ShootCoroutine(){
        // INIT
        canShoot = false;
        // Shoot Ratio
        yield return new WaitForSeconds(1.0f / shotRate);

        // Shoot
        EnemyBullet e = Instantiate( Shot, transform.position + transform.up * localOffset, transform.rotation);
        e.base_speed = ShotSpeed;
        if (Time.time >= nextPauseTime)
        {
            yield return new WaitForSeconds(pauseLength);
            CalculateNextPauseTime();
        }
        // END
        canShoot = true;
    }

    private void CalculateNextPauseTime()
    {
        float pauseInterval = 1.0f / shotRate * pauseRate;
        nextPauseTime = Time.time + pauseInterval;
    }

}
