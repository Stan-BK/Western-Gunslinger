using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    public bool isFire;
    public ParticleSystem particleSystem;
    public Animator GunAnimation;
    public PlayerInputControl InputControl;

    #region 生命周期

    // Start is called before the first frame update
    private void Awake()
    {
        InputControl = new PlayerInputControl();

        InputControl.Player.Load.started += OnLoadBullet;
        InputControl.Player.Defend.started += OnDefend;
        InputControl.Player.Fire.started += OnFire;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFire)
        {
            isFire = false;
            FireParticle();
        }
    }

    private void OnEnable()
    {
        InputControl.Enable();
    }

    private void OnDisable()
    {
        InputControl.Disable();
    }

    #endregion
 

    void Fire()
    {
        GunAnimation.SetTrigger("Fire");
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
    
    private void OnLoadBullet(InputAction.CallbackContext obj)
    {
        LoadBullet();
    } 
    private void OnDefend(InputAction.CallbackContext obj)
    {
    } 
    private void OnFire(InputAction.CallbackContext obj)
    {
        Fire();
    }
    
    #endregion
}
