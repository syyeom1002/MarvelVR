using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameObject target;
    [SerializeField]
    private float targetOffset = -0.5f;

    private void Start()
    {
        this.target=GameObject.Find("CenterEyeAnchor");
        //Debug.LogFormat("<color=lime>{0}</color>", target.name);
        
    }
    private void Update()
    {
        this.LookPlayer();
    }
    public void LookPlayer()
    {
        var lookAtPos = this.target.transform.position + (this.target.transform.up * this.targetOffset);
        this.transform.LookAt(lookAtPos);
    }
}
