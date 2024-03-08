using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeapMain : MonoBehaviour
{
    public GameObject step1High;
    public GameObject step2High;
    
    public GameObject setp1;
    public GameObject setp2;

    private Animator anim;
    private GameSceneMain gameSceneMain;
    public AudioClip[] audioClips;
    private void Start()
    {
        this.gameSceneMain = GameObject.FindObjectOfType<GameSceneMain>();
        this.step1High.SetActive(true);
        this.setp1.SetActive(false);
        StartCoroutine(CoGrab());
        this.anim = GetComponent<Animator>();
    }

   
    IEnumerator CoGrab()
    {
        while (true)
        {

            if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
            {
                Debug.Log("step1완료");
                this.step1High.gameObject.SetActive(false);
                this.setp1.SetActive(true);
                this.step2High.gameObject.SetActive(true);
                this.setp2.SetActive(false);
                GetComponent<AudioSource>().PlayOneShot(audioClips[0]);
            }
            else if (OVRInput.GetUp(OVRInput.Button.One, OVRInput.Controller.RTouch))
            {
                Debug.Log("step2완료");
                this.step2High.gameObject.SetActive(false);
                this.setp2.SetActive(true);
                Destroy(this.gameObject, 5f);
                //길 없애기
                this.gameSceneMain.pathGo[1].SetActive(false);
                GetComponent<AudioSource>().PlayOneShot(audioClips[0]);
                GetComponent<AudioSource>().PlayOneShot(audioClips[1]);
            }

            yield return null;
        }
    }
}
