using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class FadeOut : MonoBehaviour
{
    private Animator animator;
    private GameObject Particle;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        animator.enabled = true;
        if (Particle)
            Particle.GetComponent<ParticleSystem>()?.Stop();
    }

    public void Fadeout(bool isNeedParticle)
    {
        if (!gameObject.activeSelf) return;
        animator.SetTrigger("FadeOut");
        if (isNeedParticle)
        {
            if (Particle == null)
            {
                var particle = Addressables.LoadAssetAsync<GameObject>("Particle");
                particle.Completed += handle =>
                {
                    Particle = Instantiate(handle.Result);
                    Particle.transform.SetParent(gameObject.transform);
                    Particle.transform.localPosition = Vector3.zero;
                    Particle.transform.localRotation = new Quaternion(0, 0, 0, 0);
                    Particle.transform.localScale = Vector3.one;
                    Particle.SetActive(true);
                };
            }
            else
            {
                Debug.Log(Particle.GetComponent<ParticleSystem>());
                Particle.GetComponent<ParticleSystem>().Play();
            }
        }
    }
}
