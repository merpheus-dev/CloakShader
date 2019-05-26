using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Cloak : MonoBehaviour
{
    public Material[] Materials;
    public AudioClip CloakEnable;
    public AudioClip CloakDisable;

    private bool isInTransition = false;
    private AudioSource aso;

    private void Awake()
    {
        aso = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (isInTransition)
            return;
        if (Input.GetKeyDown(KeyCode.I) || (Materials[0].GetFloat("_FalseValue") == -1 && Input.GetMouseButtonDown(0)))
        {
            isInTransition = true;
            if (Materials[0].GetFloat("_FalseValue") == 1)
                aso.PlayOneShot(CloakEnable);
            StartCoroutine(WaitUntilTransitionOver());
        }
    }

    IEnumerator WaitUntilTransitionOver()
    {
        if (Materials[0].GetFloat("_FalseValue") == -1f)
            aso.PlayOneShot(CloakDisable);
        float lerpAmount = 0f;
        float targetValue = Materials[0].GetFloat("_FalseValue") == 1f ? -1f : 1f;
        while (lerpAmount < 1f)
        {
            foreach (var mat in Materials)
            {
                mat.SetFloat("_FalseValue", Mathf.Lerp(mat.GetFloat("_FalseValue"), targetValue, lerpAmount));
            }
            lerpAmount +=Time.deltaTime;
            yield return null;
        }
        isInTransition = false;
    }

    private void OnDisable()
    {
        foreach (var mat in Materials)
        {
            mat.SetFloat("_FalseValue", 1f);
        }
    }
}
