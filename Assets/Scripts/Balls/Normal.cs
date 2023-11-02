using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Normal : MonoBehaviour
{
    protected Rigidbody2D rigid;

    [SerializeField]
    protected bool firstTouched = false;
    protected bool skillUsed = false;

    protected ParticleSystem particleSys; //For trail effect

    public bool ballDone = false;

    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        rigid.isKinematic = true;

        particleSys = GetComponent<ParticleSystem>();
        particleSys.Pause();
    }

    void Update()
    {
        if (!skillUsed && Input.GetKey(KeyCode.Space) && !firstTouched)
        {
            skillUsed = true;
            Skill();
        }
    }

    void FixedUpdate()
    {
        //Shoot ~~ touched with other object
        if (!firstTouched && !rigid.isKinematic)
        {
            //Ball look moving direction
            float angle = Mathf.Atan2(rigid.velocity.y, rigid.velocity.x) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, 0, angle);

            particleSys.Play();
        }

        CheckBallDone();
    }

    protected virtual void CheckBallDone()
    {
        if (firstTouched && rigid.IsSleeping())
        {
            BallDone();
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.CompareTag("Boundary"))
        {
            BallDone();
        }

        firstTouched = true;
        particleSys.Pause();
    }

    public void BallDone()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        rigid.isKinematic = true;

        ballDone = true;
    }

    //Write at other special balls
    protected virtual void Skill() { }
}
