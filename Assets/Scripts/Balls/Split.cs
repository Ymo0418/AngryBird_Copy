using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Split : Normal
{
    [SerializeField]
    Sprite babySprite;

    [SerializeField]
    GameObject babyBall;
    
    Rigidbody2D[] babyRigids = new Rigidbody2D[2];

    //If baby balls exist check them either
    protected override void CheckBallDone()
    {
        if(skillUsed)
        {
            if(babyRigids[0].GetComponent<Normal>().ballDone &&
               babyRigids[1].GetComponent<Normal>().ballDone && 
               firstTouched && rigid.IsSleeping())
            {
                GetComponent<SpriteRenderer>().enabled = false;
                GetComponent<Collider2D>().enabled = false;
                rigid.isKinematic = true;

                ballDone = true;
            }
        }
        else
        {
            base.CheckBallDone();
        }
    }

    protected override void Skill()
    {
        babyRigids[0] = Instantiate(babyBall).GetComponent<Rigidbody2D>();
        babyRigids[1] = Instantiate(babyBall).GetComponent<Rigidbody2D>();

        Vector2 dir = GetComponent<Rigidbody2D>().velocity;

        babyRigids[0].isKinematic = false;
        babyRigids[0].transform.position = transform.position + transform.up * 0.5f;
        babyRigids[0].velocity = Quaternion.AngleAxis(20, Vector3.forward) * dir;

        babyRigids[1].isKinematic = false;
        babyRigids[1].transform.position = transform.position + transform.up * -0.5f;
        babyRigids[1].velocity = Quaternion.AngleAxis(-20, Vector3.forward) * dir;

        GetComponent<SpriteRenderer>().sprite = babySprite;
        transform.localScale = new Vector3(0.6f, 0.6f);
    }

    void OnDestroy()
    {
        if(babyRigids[0] != null)
        {
            Destroy(babyRigids[0].gameObject);
            Destroy(babyRigids[1].gameObject);
        }
    }
}
