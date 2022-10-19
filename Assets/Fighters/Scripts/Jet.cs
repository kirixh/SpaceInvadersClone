using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Jet : MonoBehaviour
{

    public float moveSpeed = 5;
    Gun[] guns;

    bool moveUp;
    bool moveDown;
    bool moveLeft;
    bool moveRight;

    bool shoot;

    public float shootInterval = 0.5f;
    float shootTimer = 0f;

    public float healPointsMax = 5f;
    public float healPointsMin = 0f;
    float healPointsCur;
    public LinearIndicator healIndicator;

    // Start is called before the first frame update
    void Start()
    {
        guns = transform.GetComponentsInChildren<Gun>();
        foreach (Gun gun in guns)
        {
            gun.isActive = true;
        }

        healIndicator = GameObject.Find("LinearIndicator-Canvas").GetComponent<LinearIndicator>();
        healPointsCur = healPointsMax;
        healIndicator.SetupIndicator(healPointsMin, healPointsMax);
        healIndicator.SetValue(healPointsCur);
    }

    // Update is called once per frame
    void Update()
    {
        moveUp = Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W);
        moveDown = Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S);
        moveLeft = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);
        moveRight = Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);

        shoot = Input.GetKeyDown(KeyCode.Space);

        if (shoot && shootTimer >= shootInterval)
        {
            shootTimer = 0f;
            shoot = false;
            foreach (Gun gun in guns)
            {
                gun.Shoot();
            }
        }
        else
        {
            shootTimer += Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        Vector2 pos = transform.position;

        float moveAmount = moveSpeed * Time.fixedDeltaTime;
        Vector2 move = Vector2.zero;

        if (moveUp)
        {
            move.y += moveAmount;
        }

        if (moveDown)
        {
            move.y -= moveAmount;
        }

        if (moveLeft)
        {
            move.x -= moveAmount;
        }

        if (moveRight)
        {
            move.x += moveAmount;
        }

        float moveMagnitude = Mathf.Sqrt(move.x * move.x + move.y * move.y);
        if (moveMagnitude > moveAmount)
        {
            move *= moveAmount / moveMagnitude;
        }
        pos += move;

        if (pos.y > 8.6f)
            pos.y = 8.6f;
        if (pos.y < 1.2f)
            pos.y = 1.2f;
        if (pos.x > 16.5f)
            pos.x = 16.5f;
        if (pos.x < 1.5f)
            pos.x = 1.5f;

        transform.position = pos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Bullet bullet = collision.GetComponent<Bullet>();
        if (bullet && bullet.isEnemyBullet)
        {
            Destroy(bullet.gameObject);
            DecreaseHealPoints();
            GameController.instance.camShake();
        }

        Destructable destructable = collision.GetComponent<Destructable>();
        if (destructable)
        {
            Destroy(destructable.gameObject);
            GameController.instance.AddScore(Destructable.scoreValue);
            DecreaseHealPoints();
            GameController.instance.camShake();
            GameController.instance.spawnPowerUp(transform.position);
        }

        PowerUp powerup = collision.GetComponent<PowerUp>();
        if (powerup)
        {
            if (powerup.type == 1)
            {
                IncreaseHealPoints();
            }
            else if (powerup.type == 2)
            {
                GameController.instance.upgradeJet();
            }
            Destroy(powerup.gameObject);

        }
    }

    public void IncreaseHealPoints()
    {
        if (healPointsCur < healPointsMax)
        {
            ++healPointsCur;
            healIndicator.SetValue(healPointsCur);
        }
    }
    public void DecreaseHealPoints()
    {
        --healPointsCur;
        healIndicator.SetValue(healPointsCur);
        if (healPointsCur <= 0)
        {
            Destroy(gameObject);
            GameController.instance.RestartGame();
        }
    }

    public float getHealPoints()
    {
        return healPointsCur;
    }

    public void setHealPoints(float healPoints)
    {
        healPointsCur = healPoints;
    }
    public void DestroyJet()
    {
        Destroy(gameObject);
    }

    public Quaternion getQuaternionIdentity()
    {
        return Quaternion.identity;
    }
}
