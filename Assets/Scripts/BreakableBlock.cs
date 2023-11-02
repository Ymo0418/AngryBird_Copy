using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBlock : MonoBehaviour
{
    [SerializeField]
    float fullhp = 10;
    [SerializeField]
    float curhp;

    Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        curhp = fullhp;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 velocity = collision.relativeVelocity;

        float dmg = velocity.magnitude;

        if (curhp <= dmg)
            Destroy(gameObject);

        curhp -= dmg;
        if (curhp < fullhp / 2)
            GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("CrackTexture");

    }
}
