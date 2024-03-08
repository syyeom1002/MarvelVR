using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialAerialEnemy : MonoBehaviour
{
    [SerializeField]
    private SphereCollider childCollider;
    [SerializeField]
    private GameObject getHitEffect;
    private Rigidbody rBody;
    private SphereCollider parentCollider;
    private Animator anim;
    public AudioClip audioClip;
    private void Start()
    {
        this.anim = this.gameObject.GetComponent<Animator>();
        this.rBody = this.gameObject.GetComponent<Rigidbody>();
        this.parentCollider = this.gameObject.GetComponent<SphereCollider>();
        this.rBody.useGravity = false;
    }


    public void GetHit()
    {
        Destroy(this.gameObject, 0.8f);
        this.rBody.useGravity = true; //¹Ù´ÚÀ¸·Î ¶³¾îÁö°Ô 
        this.parentCollider.enabled = false;
        this.childCollider.enabled = true;
        this.anim.enabled = false;

        this.rBody.AddForce(transform.forward * -150f);
        this.getHitEffect.SetActive(true);
        GetComponent<AudioSource>().PlayOneShot(audioClip);
    }
}
