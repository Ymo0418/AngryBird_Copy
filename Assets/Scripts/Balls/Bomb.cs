using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Normal
{
    float range = 5f;

    //Skill used when touched other object
    protected override void Skill()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, range, 1 << 7);

        foreach(Collider2D block in colliders)
        {
            Vector2 forceDir = (block.transform.position - transform.position).normalized;
            block.GetComponent<Rigidbody2D>().AddForce(forceDir * 300, ForceMode2D.Impulse);
        }

        base.BallDone();
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if(!skillUsed)
        {
            skillUsed = true;
            Skill();
        }

        if (collision.transform.CompareTag("Boundary"))
        {
            base.BallDone();
        }

        firstTouched = true;
        particleSys.Pause();
    }
}
