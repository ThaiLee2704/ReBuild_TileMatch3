using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWhenParticleDone : MonoBehaviour
{
    private ParticleSystem ps;

    private void OnEnable()
    {
        ps = GetComponent<ParticleSystem>();
        StartCoroutine(DeAvtive());
    }

    IEnumerator DeAvtive()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
