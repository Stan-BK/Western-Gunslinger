using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    public bool isShoot;
    public ParticleSystem particleSystem;
    public Animator GunAnimation;

    // Update is called once per frame
    void Update()
    {
        if (isShoot)
        {
            isShoot = false;
            FireParticle();
        }
    }

    void Shoot()
    {
        GunAnimation.SetTrigger("Shoot");
    }

    void LoadBullet()
    {
        GunAnimation.SetTrigger("Load");
    }
    
    void FireParticle()
    {
        particleSystem.Play();   
    }

    #region 用户行为
    
    public void OnLoadBullet()
    {
        LoadBullet();
    } 
    
    public void OnShoot()
    {
        Shoot();
    }
    
    #endregion
}
