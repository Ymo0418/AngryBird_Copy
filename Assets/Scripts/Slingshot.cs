using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Slingshot : MonoBehaviour
{
    public LineRenderer[] lr;
    public Transform[] stripPos;
    public Transform center;
    public Transform idlePos;
    public Transform waitingPoint;

    public Vector3 curPos;

    public float maxLen;
    public float bottomBoundary;

    KeyCode[] keyCodes = { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4 };

    CircleCollider2D dragCollider;
    bool isMouseDown;

    public GameObject[] ballPrefabs;
    GameObject ballObj; //Current holding ball
    GameObject firstBall, secondBall; //Shooted balls

    Rigidbody2D ballRigid;
    Collider2D ballCollider;

    public float ballPosOffset;
    public float force;
    

    void Awake()
    {
        dragCollider = GetComponent<CircleCollider2D>();
    }

    void Start()
    {
        lr[0].positionCount = 2;
        lr[1].positionCount = 2;
        lr[0].SetPosition(0, stripPos[0].position);
        lr[1].SetPosition(0, stripPos[1].position);
        
        ResetStrip();
        CreateBall(0);
    }

    void Update()
    {
        //Press 'R' to restart
        if (Input.GetKey(KeyCode.R))
            SceneManager.LoadScene("AngryBird_Copy");

        //Pull slingshot
        if(isMouseDown)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10;

            curPos = Camera.main.ScreenToWorldPoint(mousePos);
            curPos = center.position + Vector3.ClampMagnitude(curPos - center.position, maxLen);
            curPos = ClampBoundary(curPos); //To keep sling over ground

            SetStrips(curPos);
        }

        //Can spawn ball
        if (dragCollider.enabled)
        {
            //Check pressing number 1,2,3,4
            for (int i = 0; i < keyCodes.Length; i++)
            {
                if (Input.GetKey(keyCodes[i]))
                {
                    CreateBall(i);
                }
            }
        }
        else
        {
            CheckBallsDone();
        }
    }

    //1-Normal / 2-Speed / 3-Split / 4-Bomb
    void CreateBall(int i)
    {
        //Can't spawn another ball while spawning ball
        CancelInvoke();
        dragCollider.enabled = false;
        ResetStrip();
        Destroy(ballObj);

        ballObj = Instantiate(ballPrefabs[i], waitingPoint) as GameObject;
        ballRigid = ballObj.GetComponent<Rigidbody2D>();

        //Animation to slingshot
        ballObj.transform.DOJump(center.position, 2f, 1, 0.7f);
        Invoke("BallReady", 0.85f);
    }

    //Check If previous ball act done
    void CheckBallsDone()
    {
        if (ballObj == null)
        {
            //Two ball exist
            if (secondBall != null)
            {
                //Current ball act done
                if (secondBall.GetComponent<Normal>().ballDone)
                {
                    Destroy(firstBall);
                    firstBall = secondBall;
                    secondBall = null;

                    BallReady();
                }
            }
            else //One ball exist
            {
                if (firstBall.GetComponent<Normal>().ballDone)
                {
                    BallReady();
                }
            }
        }
    }

    void BallReady()
    {
        dragCollider.enabled = true;
    }

    private void OnMouseDown()
    {
        isMouseDown = true;
    }

    private void OnMouseUp()
    {
        isMouseDown = false;
        dragCollider.enabled = false;
        Shoot();
        ResetStrip();
    }

    void Shoot()
    {
        ballRigid.isKinematic = false;  //To effected by gravity
        Vector3 ballForce = (curPos - center.position) * force * -1;
        ballRigid.velocity = ballForce;

        if (firstBall == null) { firstBall = ballObj; }
        else { secondBall = ballObj; }

        ballObj = null;
    }

    void ResetStrip()
    {
        curPos = idlePos.position;
        SetStrips(idlePos.position);
    }

    void SetStrips(Vector3 position)
    {
        lr[0].SetPosition(1, position);
        lr[1].SetPosition(1, position);

        if(ballRigid)
        {
            Vector3 dir = position - center.position;
            ballRigid.transform.position = position + dir.normalized * ballPosOffset;
            ballRigid.transform.right = -dir.normalized;
        }
    }

    Vector3 ClampBoundary(Vector3 vector)
    {
        vector.y = Mathf.Clamp(vector.y, idlePos.position.y + bottomBoundary, 1000);
        return vector;
    }
}
