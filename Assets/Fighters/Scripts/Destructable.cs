using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour
{

    bool inCameraView = false;
    public static int scoreValue = 10;

    public GameObject explosion;

    // Update is called once per frame
    void Update()
    {
        if (!inCameraView && transform.position.y <= 10f)
        {
            inCameraView = true;
            Gun[] guns = transform.GetComponentsInChildren<Gun>();
            foreach (Gun gun in guns)
            {
                gun.isActive = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (inCameraView)
        {
            Bullet bullet = collision.GetComponent<Bullet>();
            if (bullet && !bullet.isEnemyBullet)
            {
                GameController.instance.AddScore(scoreValue);
                Instantiate(explosion, transform.position, Quaternion.identity);
                Destroy(gameObject);
                Destroy(bullet.gameObject);
                GameController.instance.spawnPowerUp(transform.position);
            }
        }
    }
}
