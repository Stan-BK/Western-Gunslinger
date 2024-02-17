using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour
{
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.enabled = false;
    }

    public void Fadeout()
    {
        animator.enabled = true;
    }
}
