using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fast : Normal
{
    float accel = 22f;

    protected override void Skill()
    {
        rigid.velocity *= (accel / rigid.velocity.magnitude);
    }
}
