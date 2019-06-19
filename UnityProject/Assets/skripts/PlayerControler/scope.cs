using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scope : MonoBehaviour
{

    public Animator Animator;

    public GameObject scopeOverply;

    private bool isScoped = false;

    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            isScoped = !isScoped;
            Animator.SetBool("scoped", isScoped);

            if (isScoped)
                StartCoroutine(OnScoped());
            else
                OnUnScoped();
        }
    }

    IEnumerator OnScoped()
    {
        yield return new WaitForSeconds(0.15F);

        scopeOverply.SetActive(true);
    }
    void OnUnScoped()
    {
        scopeOverply.SetActive(false);
    }
}
