using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLeap : MonoBehaviour
{
    [SerializeField] private Transform rayPoint;
    private float fireRange = 100;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] GameObject warp;
    [SerializeField] Transform leapPos;
    [SerializeField] GameObject block;
    public GameObject groundMain;
    public GameObject objManager;
    public GameObject groundSpawnPoints;
    private Coroutine coroutine;
    float curTime = 0f;
    float warpTime = 0.5f;
    private Animator anim;
    public GameObject car;
    // Start is called before the first frame update
    void Start()
    {
        this.lineRenderer.gameObject.SetActive(false);
        this.warp.gameObject.SetActive(false);
        this.anim = car.GetComponent<Animator>();
    }

    void Update()
    {
        this.DrawLine();
        //==============================레이쏘기===================================
        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
        {
            //A 버튼 눌렀을 때 레이켜주고
            this.lineRenderer.gameObject.SetActive(true);
        }
        if(OVRInput.GetUp(OVRInput.Button.One, OVRInput.Controller.RTouch))
        {
            this.lineRenderer.gameObject.SetActive(false);
            
        }
       

    }
    //레이쏴서 맞는 로직
    private void DrawLine()
    {
        var ray = new Ray(this.rayPoint.position, this.rayPoint.right);
        Debug.DrawRay(ray.origin, ray.direction * this.fireRange, Color.red);
        
        RaycastHit hit;
        if (Physics.Raycast(ray.origin, ray.direction, out hit, this.fireRange))
        {
            if (hit.collider.CompareTag("LeapPoint") && OVRInput.GetUp(OVRInput.Button.One, OVRInput.Controller.RTouch))
            {
                Debug.Log("리프에 맞았다");
                this.coroutine = StartCoroutine(CoWarp());
                curTime = 0;
            }
            
        }
        var end = this.rayPoint.position + (this.rayPoint.right * fireRange);

        this.lineRenderer.SetPosition(0, rayPoint.position);
        this.lineRenderer.SetPosition(1, end);

    }
    private Rigidbody leapRbody;
    private Collider blockColl;

    //리프 이동 로직
    IEnumerator CoWarp()
    {
        while (curTime < warpTime)
        {
            this.warp.gameObject.SetActive(true); //주변 바람효과
            this.lineRenderer.gameObject.SetActive(false); //라인 켜고
            curTime += Time.deltaTime;

            this.transform.position = Vector3.Lerp(this.transform.position, this.leapPos.position, 0.1f); 
            this.leapRbody = leapPos.gameObject.GetComponent<Rigidbody>();
            leapRbody.isKinematic = true; //원 물리영향 안받게해서 덜덜거림 방지
            this.blockColl = block.gameObject.GetComponent<Collider>(); 
            blockColl.isTrigger = true; //바닥 트리거 체크 켜서 덜덜거림 방지
            //this.gameObject.GetComponent<Rigidbody>().isKinematic = true; //플레이어 물리영향 안받게 하기
            
            yield return null;

        }
        this.transform.position = new Vector3(-15f, 10f, 12f); //새포지션으로 이동
        this.transform.localRotation = Quaternion.Euler(-0.004f, 89.645f, -0.106f);
        this.warp.gameObject.SetActive(false); //바람효과 끄고

        leapRbody.isKinematic = false;
        blockColl.isTrigger = false;
        //this.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        //this.gameObject.GetComponent<Rigidbody>().constraints =RigidbodyConstraints.FreezeRotation;
        //this.gameObject.GetComponent<CapsuleCollider>().isTrigger = false;

        //-----------------------------------------------
        groundMain.SetActive(true);
        groundSpawnPoints.SetActive(true);
        objManager.SetActive(true);
        this.car.SetActive(true);
    }

}
