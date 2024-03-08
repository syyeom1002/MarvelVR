using DG.Tweening;
using Meta.WitAi;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class SpaceStone : MonoBehaviour
{
    [SerializeField] private GameObject missionClearPrefab;
    [SerializeField] private GameObject rHandWithStone;//프리팹으로 넣으면 직접 찾아줘야함
    [SerializeField] private GameObject rCustomHand;//프리팹으로 넣으면 직접 찾아줘야함
    [SerializeField] private GameObject lightEffect;
    [SerializeField] private AudioSource ingameBGMAudio;
    public AudioClip audioClip;
    private OVRScreenFade OFade;
    private GameObject centerEyeAnchor;
    private bool isSelect = true;
    private bool isGrab = false;
    [SerializeField]
    private Transform rHand;
    private MeshRenderer meshRenderer;

    void Start()
    { 
        this.centerEyeAnchor = GameObject.Find("CenterEyeAnchor");
        OFade = this.centerEyeAnchor.transform.GetComponent<OVRScreenFade>();
        this.meshRenderer = this.GetComponent<MeshRenderer>();
        this.StartCoroutine(this.CoMoveUp());
        GetComponent<AudioSource>().PlayOneShot(audioClip);
        DontDestroyOnLoad(lightEffect);
    }

    private void Update()
    {
        if (this.isGrab == true)
        {
            this.rHandWithStone.transform.localPosition = rHand.position;
            this.rHandWithStone.transform.localRotation = rHand.rotation;
        }


        this.transform.Rotate(Vector3.up *10f* Time.deltaTime, Space.World);
        
    }

    public void OnWhenSelect()
    {
        this.isGrab = true;
        if (isSelect == true)
        {
            //미션완료 팝업
            Instantiate(this.missionClearPrefab);
            ingameBGMAudio.Stop();
            //조금 있다가 페이드아웃
            this.StartCoroutine(CoFadeOut());
            this.StartCoroutine(CoIncreaseScale());
        }

        this.rHandWithStone.SetActive(true);
        this.rCustomHand.SetActive(false);
        this.meshRenderer.enabled = false;
        SelectCharaterMain.isClearCaptain = true;
    }

    public void OnWhenUnSelect()
    {
        this.isGrab = false;
        this.isSelect = false;

        this.rHandWithStone.SetActive(false);
        this.rCustomHand.SetActive(true);
        this.meshRenderer.enabled = true;
    }

    private IEnumerator CoFadeOut()
    {
        yield return new WaitForSeconds(4.5f);
        OFade.FadeOut();
    }

    
    private IEnumerator CoMoveUp()
    {
        yield return new WaitForSeconds(2.0f);
        while (true)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(-4.4f, 13.5f, 13.5f), 0.02f);
            var dis = Vector3.Distance(new Vector3(-4.4f, 13.5f, 13.5f), this.transform.position);
            if (dis < 0.1f)
            {
                //Debug.Log("위로 이동 멈춰라");
                this.StartCoroutine(this.CoMove());
                break;
            }
            yield return null;
        }
    }

    private IEnumerator CoMove()
    {
        yield return new WaitForSeconds(1f);
        while (true)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, this.centerEyeAnchor.transform.position + new Vector3(0, -0.2f, 0), 0.1f);
            var dis = Vector3.Distance(centerEyeAnchor.transform.position + new Vector3(0, -0.2f, 0), this.transform.position);
            if (dis < 0.6f)
            {
                break;
            }
            yield return null;
        }
    }

    private IEnumerator CoIncreaseScale()
    {
        yield return new WaitForSeconds(3.5f);
        this.lightEffect.SetActive(true);
        while (true)
        {
            this.lightEffect.transform.localScale += new Vector3(0.0015f, 0.0015f, 0.0015f);
            yield return null;
        }
    }
}
