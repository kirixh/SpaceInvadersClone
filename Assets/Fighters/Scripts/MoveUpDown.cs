using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUpDown : MonoBehaviour
{

    public float moveSpeed = 1;
    public bool isEnemy = true;
    private void FixedUpdate()
    {
        Vector2 pos = transform.position;

        pos.y -= moveSpeed * Time.fixedDeltaTime;

        if (pos.y <= -1)
        {
            Destroy(gameObject);
            if (isEnemy)
                GameController.instance.DecreaseHealPoints();
        }

        transform.position = pos;
    }
}
