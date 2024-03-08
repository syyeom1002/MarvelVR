using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMove : MonoBehaviour
{
    //public enum eLight
    //{
    //    FAST,MEDIUM,SLOW
    //}
    private Vector3 dir=Vector3.zero;
    public float maxSpeed = 1f;
    private void Start()
    {
        this.dir = Vector3.forward;
        this.StartCoroutine(CoMove());
    }

    private IEnumerator CoMove()
    {
        while (true)
        {
            this.transform.Translate(this.dir * Random.Range(1f, this.maxSpeed) * Time.deltaTime);
            if (this.transform.localPosition.z > -0.05)
            {
                this.dir=this.transform.forward * -1;
            }
            else if (this.transform.localPosition.z <=0.09)
            {
                this.dir = this.transform.forward * 1;
            }
            yield return null;
        }
    }
}
