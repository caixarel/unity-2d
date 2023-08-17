using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitFX : MonoBehaviour
{
    [Header("Flash FX")]
    [SerializeField] private Material hitMat;
     private Material originalMat;

    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMat = sr.material;
    }

    private IEnumerator FlashFX()
    {
        sr.material = hitMat;

        yield return new WaitForSeconds(.2f);

        sr.material = originalMat;
    }

    private void RedColorBlink()
    {
        if (sr.color != Color.white) sr.color = Color.white;
        else sr.color = Color.red;
        Debug.Log(sr.color != Color.white);
    }

    private void CancelRedBlink()
    {
        CancelInvoke();
        sr.color = Color.white;
    }
}
