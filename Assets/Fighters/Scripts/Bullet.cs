using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector] public Vector2 direction = new(0, 1);

    public float speedMultiplier = 2;

    Vector2 speed;

    public bool isEnemyBullet = false;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 3);
    }

    // Update is called once per frame
    void Update()
    {
        speed = direction * speedMultiplier;
    }

    void FixedUpdate()
    {
        Vector2 pos = transform.position;

        pos += speed * Time.fixedDeltaTime;

        transform.position = pos;
    }
}
