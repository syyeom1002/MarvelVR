using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMain : MonoBehaviour
{
    [SerializeField] private Shield shield;
    [SerializeField] private GameSceneMain gameSceneMain;
    [SerializeField] private GameObject missionPrefab;
    [SerializeField] private Transform centerEyeAnchor;
    public GameObject step1High;
    public GameObject step2High;
    public GameObject step3High;
    public GameObject setp1;
    public GameObject setp2;
    public GameObject setp3;
    public AudioClip[] audioClips;

    private bool isFinish;
    private bool isComplete;
    private Coroutine coroutine;
    private Animator anim;
    private float av = 0f;
    private void Start()
    {
        this.Active();
        this.coroutine=StartCoroutine(CoGrab());
        this.anim = GetComponent<Animator>();
    }

    private void Active()
    {
        this.step1High.SetActive(true);
        this.setp1.SetActive(false);
    }

    private void Update()
    {
        var velocity = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.LTouch);
        this.av = Mathf.Abs((float)Math.Truncate(((velocity.x + velocity.y + velocity.z) / 3.0f) * 100f) / 100f);

        if (isFinish == true)
        {
            this.gameSceneMain.CreateEnemy();
        }
    }

    private bool isRegrab = false;
    IEnumerator CoGrab()
    {
        while (true)
        {
            if (isRegrab == false)
            {
                if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch))
                {
                    Debug.Log("step1완료");
                    this.step1High.gameObject.SetActive(false);
                    this.setp1.SetActive(true);
                    this.step2High.gameObject.SetActive(true);
                    this.setp2.SetActive(false);
                    GetComponent<AudioSource>().PlayOneShot(audioClips[0]);
                }
                else if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch)&&this.av>=0)//원래 0.3
                {
                    Debug.Log("step2완료");

                    this.step2High.gameObject.SetActive(false);
                    this.setp2.SetActive(true);
                    this.step3High.gameObject.SetActive(true);
                    this.setp3.SetActive(false);
                    isRegrab = true;
                    GetComponent<AudioSource>().PlayOneShot(audioClips[0]);
                }
            }
            if (isRegrab == true)
            {
                if (OVRInput.Get(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch)&& Vector3.Distance(shield.pos.transform.position, shield.lHandAnchor.position) < 0.2f)
                {
                    if (this.isComplete == false)
                    {
                        this.anim.SetInteger("IsFinish", 0);
                        GetComponent<AudioSource>().PlayOneShot(audioClips[1]);
                        this.isRegrab = false;
                        //미션 나오기
                        yield return new WaitForSeconds(0.5f);
                        GameObject missionGo = Instantiate(this.missionPrefab);
                        Destroy(missionGo, 4.5f);
                        this.isComplete = true;
                        //Debug.Log("미션나오기");
                    }
                    

                    //적 생성 및 코루틴 중지
                    yield return new WaitForSeconds(3f);
                    this.isFinish = true;
                    if (this.coroutine != null) StopCoroutine(this.coroutine);
                }
     
            }
            yield return null;
        }
    }
}
