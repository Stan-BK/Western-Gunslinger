using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public bool isFire;
    public ParticleSystem particleSystem;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isFire)
        {
            isFire = false;
            Fire();
        }
    }
    
    void Fire()
    {
        particleSystem.Play();   
    }
}
