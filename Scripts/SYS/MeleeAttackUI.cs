using Meta.WitAi.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackUI : MonoBehaviour
{
    public GameObject step1High;
    public GameObject step2High;
    public GameObject setp1;
    public GameObject setp2;

    private Animator anim;
    private GameSceneMain gameSceneMain;
    private bool isCreate = false;
    public AudioClip audioClip;
    public void SetInActiveStep1High(short evtType)
    {
        if (evtType == (short)PlayerController.eEventType.INDEX_TRIGGER_PULL_WHEN_ENEMY_DETECTED)
        {
            this.step1High.gameObject.SetActive(false);
            this.setp1.SetActive(true);
            this.step2High.gameObject.SetActive(true);
            this.setp2.SetActive(false);

            EventDispatcher.instance.RemoveEventHandler((short)PlayerController.eEventType.INDEX_TRIGGER_PULL_WHEN_ENEMY_DETECTED, this.SetInActiveStep1High);
            GetComponent<AudioSource>().PlayOneShot(audioClip);
        }
        
    }

    public void TutorialClear(short evtType)
    {
        if (evtType == (short)PlayerController.eEventType.COLLISION_WITH_PLANE)
        {
            this.step2High.gameObject.SetActive(false);
            this.setp2.SetActive(true);
            Destroy(this.gameObject, 0.8f);
            //길 없애기
            this.gameSceneMain.pathGo[0].SetActive(false);

            this.isCreate = true;
            this.StartCoroutine(this.CoCreate());

            EventDispatcher.instance.RemoveEventHandler((short)PlayerController.eEventType.COLLISION_WITH_PLANE, this.TutorialClear);

            Debug.Log("<color=lime>step1High를 비활성화 하고 이벤트를 제거합니다.</color>");
        }

    }

    private void Start()
    {
        EventDispatcher.instance.AddEventHandler((short)PlayerController.eEventType.INDEX_TRIGGER_PULL_WHEN_ENEMY_DETECTED, this.SetInActiveStep1High);
        EventDispatcher.instance.AddEventHandler((short)PlayerController.eEventType.COLLISION_WITH_PLANE, this.TutorialClear);
        this.step1High.SetActive(true);
        this.setp1.SetActive(false);
        this.gameSceneMain = GameObject.FindObjectOfType<GameSceneMain>();
        this.anim = GetComponent<Animator>();
        this.StartCoroutine(this.CoCreate());
    }

    private IEnumerator CoCreate()
    {
        yield return new WaitForSeconds(0.5f);
        while (true)
        {
            if (isCreate == true)
            {
                this.gameSceneMain.isFirstEnemy = false;
                this.gameSceneMain.enemyNum = 0;
                isCreate = false;
                Debug.Log("적 생성 진행중");
            }
            yield return null;
        }
    }
}
