using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    public Bullet bullet;
    Vector2 direction;

    public bool autoShoot = false;
    public float shootIntervalSeconds = 0.5f;
    public float shootDelaySeconds = 3f;
    float shootTimer = 0f;
    float delayTimer = 0f;
    [HideInInspector] public bool isActive = false;

    // Update is called once per frame
    void Update()
    {
        if (!isActive)
            return;

        direction = (transform.localRotation * Vector2.up).normalized;
        if (autoShoot)
        {
            if (delayTimer >= shootDelaySeconds)
            {
                if (shootTimer >= shootIntervalSeconds)
                {
                    shootTimer = 0f;
                    Shoot();
                }
                else
                {
                    shootTimer += Time.deltaTime;
                }
            }
            else
            {
                delayTimer += Time.deltaTime;
            }
        }
    }

    public void Shoot()
    {
        GameObject go = Instantiate(bullet.gameObject, transform.position, Quaternion.identity);
        Bullet goBullet = go.GetComponent<Bullet>();
        goBullet.direction = direction;
        goBullet.transform.rotation = transform.rotation;
    }
}
