using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShield : MonoBehaviour
{
    [SerializeField]
    SphereCollider coll;
    // Start is called before the first frame update
    void Start()
    {
        coll.enabled = false;

    }

   
    // Update is called once per frame
    void Update()
    {
        
    }
}
