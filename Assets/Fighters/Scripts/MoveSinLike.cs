using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSinLike : MonoBehaviour
{

    float sinOffset;
    public float amplitude = 1;
    public float frequency = 1;

    // Start is called before the first frame update
    void Start()
    {
        sinOffset = transform.position.x;
    }

    private void FixedUpdate()
    {
        Vector2 pos = transform.position;

        float sin = Mathf.Sin(pos.y * frequency) * amplitude;

        pos.x = sin + sinOffset;

        transform.position = pos;
    }
}
