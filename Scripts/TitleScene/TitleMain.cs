using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMain : MonoBehaviour
{
    public GameObject pressKeyGo;
    public GameObject lightGo;
    void Start()
    {
        StartCoroutine(CoDelay());
    }

    private IEnumerator CoDelay()
    {
        yield return new WaitForSeconds(11.733f);
        lightGo.SetActive(true);
        yield return new WaitForSeconds(2f);
        pressKeyGo.SetActive(true);
    }
}
