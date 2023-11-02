using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyBall : Normal
{
    protected override void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();

        particleSys = GetComponent<ParticleSystem>();
    }
}
