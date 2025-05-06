using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RanitaCelebracion : MonoBehaviour
{
    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Celebrar()
    {
        anim.SetTrigger("CelebrarTrigger");
    }
}