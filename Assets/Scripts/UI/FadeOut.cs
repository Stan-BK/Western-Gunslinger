using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.Util;

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
        return;
        if (isNeedParticle)
        {
            if (Particle == null)
            {
                StartCoroutine((WaitForLoadParticle()));
            }
            else
            {
                Particle.GetComponent<ParticleSystem>().Play();
            }
        }
    }

    IEnumerator WaitForLoadParticle()
    {
        // var locationHandler = Addressables.LoadResourceLocationsAsync("Particle");
        // yield return locationHandler;
        // var location = locationHandler.Result[0];
        //
        // var particleHandler = Addressables.LoadAssetAsync<GameObject>(location);
        // yield return particleHandler;
        var particleHandler = Addressables.LoadAssetAsync<GameObject>("Particle");
        // Addressables.LoadResourceLocationsAsync("Particle").Completed += (e) =>
        // {
        //     foreach (var VARIABLE in e.Result)
        //     {
        //         Debug.Log(VARIABLE.);
        //     }
        // };
        // Debug.Log(ResourceManagerConfig.ShouldPathUseWebRequest(Addressables.LibraryPath + "Particle"));
        yield return particleHandler;
        if (particleHandler.Status == AsyncOperationStatus.Failed)
        {
            Debug.Log(particleHandler.OperationException);
        }
        else
        {
            Particle = Instantiate(particleHandler.Result);
            Particle.transform.SetParent(gameObject.transform);
            Particle.transform.localPosition = Vector3.zero;
            Particle.transform.localRotation = new Quaternion(0, 0, 0, 0);
            Particle.transform.localScale = Vector3.one;
            Particle.SetActive(true);
        }
    }
}
