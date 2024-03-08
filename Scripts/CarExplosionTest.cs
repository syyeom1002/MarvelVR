using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CarExplosionTest : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Shield")
        {
            Debug.Log("shield");
            var rBody = this.gameObject.GetComponent<Rigidbody>();
            rBody.AddForce(transform.up * 1000f);
        }
    }
}
