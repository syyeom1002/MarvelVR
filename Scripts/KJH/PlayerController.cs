using Meta.WitAi;
using Oculus.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public enum eEventType
    { 
        INDEX_TRIGGER_PULL_WHEN_ENEMY_DETECTED,
        COLLISION_WITH_PLANE
    }
    [SerializeField]
    private GameObject Light;
    [SerializeField]
    private PathMain pathMain;
    [SerializeField]
    private GameObject[] indicatorGo; 
    [SerializeField]
    private Transform[] pos;
    //[SerializeField] Transform leapPos;
    [SerializeField] ParticleSystem warp;
    [SerializeField] private GameObject leapTutorial;
    [SerializeField] private GameObject meleeAttackUI;
    [SerializeField] private GameObject tutorialAerialEnemy;
    [SerializeField] private GameObject softBodySlam;
    public GameObject leapPoint;
    public float ms = 0.025f;
    public AudioClip[] audioClips;
    private Coroutine coroutine;
    public GameObject bike;
    //float curTime = 0;
    //float warpTime = 0.5f;

    private Animator anim;
    public GameObject fallingBridge;
    void Start()
    {
        this.Move();
        StartCoroutine(CoDetectEnemy());
        this.anim = fallingBridge.GetComponent<Animator>();
    }

    public void Move()
    {
        this.coroutine = this.StartCoroutine(this.CoMove());
    }

    public void SecondMove()
    { 
        this.coroutine = this.StartCoroutine(this.CoSecondMove());
    }

    private IEnumerator CoMove()
    {
        yield return new WaitForSeconds(1.3f);
        while (true)
        {
            var dis = Vector3.Distance(this.transform.position, indicatorGo[0].transform.position);

            if (dis < 9f)
            {
                var dir = pos[1].position - this.transform.position;
                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 0.2f);
                this.transform.position = Vector3.MoveTowards(this.transform.position, pos[1].position, ms);
            }
            else
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, pos[0].position, ms);
            }
            yield return null;
        }
    }

    private IEnumerator CoSecondMove()
    {
        yield return new WaitForSeconds(1.3f);
        while (true)
        {
            var dis = Vector3.Distance(this.transform.position, indicatorGo[1].transform.position);
            if (dis < 7f)
            {
                var dir = pos[4].position - this.transform.position;
                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 0.3f);
                this.transform.position = Vector3.MoveTowards(this.transform.position, pos[4].position, ms);
            }
            else if(dis<11f&&dis>7f)
            {
                var dir = pos[3].position - this.transform.position;
                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 0.2f);
                this.transform.position = Vector3.MoveTowards(this.transform.position, pos[3].position, ms); 
            }
            else
            {
                var dir = pos[2].position - this.transform.position;
                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 0.2f);
                this.transform.position = Vector3.MoveTowards(this.transform.position, pos[2].position, ms);
            }
            yield return null;
        }
    }
    public GameObject ex;
    public AudioClip elecAudio;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Point"))
        {
            Debug.Log("목적지에 도착했습니다");
            //인디케이터 지우고 이동 멈추기
            this.indicatorGo[0].SetActive(false);
            
            if (this.coroutine != null) StopCoroutine(this.coroutine);

            //근거리 공격 튜토리얼
            Instantiate(this.meleeAttackUI);
            //튜토리얼용 적 생성
            this.tutorialAerialEnemy.SetActive(true);
        }
        else if (other.CompareTag("SecondPoint"))
        {
            this.indicatorGo[1].SetActive(false);
            if (this.coroutine != null) StopCoroutine(this.coroutine);
            this.leapPoint.SetActive(true);
            //리프 튜토리얼 나오기
            Instantiate(this.leapTutorial);
            
        }
        else if (other.CompareTag("Falling"))
        {
            Debug.Log("부딫힘");
            this.anim.SetInteger("State",1);
            this.Light.SetActive(false);
            ex.SetActive(true);
            GetComponent<AudioSource>().PlayOneShot(elecAudio);
        }
        else if (other.CompareTag("ShotGun"))
        {
            bike.SetActive(true);
        }
    }





    //----------------------------------------------근거리 공격---------------------------------------------------

    Dictionary<GameObject, float> dic = new Dictionary<GameObject, float>();
    Queue<float> posQueue = new Queue<float>();
    private bool isGrabIndexTrigger = false;
    private float delta = 0f;
    private float av;
    private int radius=10;
    [SerializeField] private GameObject shieldEff;
    //[SerializeField] private GameObject hitEff; //바닥 이펙트
    [SerializeField] private Shield shield;
    [SerializeField] private ParticleSystem hitParticle;
    public Transform detectPlane;
    [SerializeField] private AerialEnemy aerialEnemy;
    [SerializeField] private TutorialAerialEnemy tutorialEnemy;
    [SerializeField] private GameObject hitGround;
    public IEnumerator CoDetectEnemy()
    {
        while (true)
        {
            Collider[] enemyCols = Physics.OverlapSphere(this.transform.position, this.radius, 1 << 6);
            this.dic.Clear();
            var velocity = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.LTouch); //.normalized;
            this.av = Mathf.Abs((float)Math.Truncate(((velocity.x + velocity.y + velocity.z) / 3.0f) * 100f) / 100f);

            for (int i = 0; i < enemyCols.Length; i++)
            {
                var col = enemyCols[i];
                this.dic.Add(col.gameObject, Vector3.Distance(col.transform.position, this.shield.lHandAnchor.position));
            }

            if (this.dic.Count > 0)
            {
                var nearestDistance = this.dic.Values.Min();
                var target = this.dic.FirstOrDefault(x => x.Value == nearestDistance).Key;
                var dir = target.transform.position - this.transform.position;
                Ray ray = new Ray(this.transform.position, dir);
                Debug.DrawRay(ray.origin, ray.direction * 1000f, Color.red);

                if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch))
                {

                    //그랩했을 때 위치값 재기
                    this.isGrabIndexTrigger = true;
                    this.shieldEff.gameObject.SetActive(true);
                    this.detectPlane.gameObject.SetActive(true);

                    EventDispatcher.instance.SendEvent((short)eEventType.INDEX_TRIGGER_PULL_WHEN_ENEMY_DETECTED);
                    GetComponent<AudioSource>().PlayOneShot(audioClips[0]);

                }
                else if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch))
                {
                    this.isGrabIndexTrigger = false;
                    this.shieldEff.gameObject.SetActive(false);
                    this.detectPlane.gameObject.SetActive(false);
                    Debug.Log("버튼 뗏다");
                }

                //적이 감지된 ui를 띄우던 , 바닥을 찍기, 
                //바닥을 찍었다
                this.shield.onCollisionPlane = (Vector3 coll) =>
                {
                    if (this.posQueue.Count == 0)   //이전 위치가 없다
                    {
                        Debug.Log("이전 위치가 없습니다.");
                        return;
                    }

                    if (this.posQueue.Peek() > this.detectPlane.transform.position.y && this.av > 0.5f)
                    {
                        Debug.Log("====> 위에서 찍음");

                        //큐를 비움 
                        this.posQueue.Clear();
                        this.delta = 0;
                        this.isGrabIndexTrigger = false;
                        var bodySlam = Instantiate(this.softBodySlam, new Vector3(coll.x, 0.1f, coll.z), Quaternion.Euler(-90f,0,0));
                        Destroy(bodySlam, 2.0f);
                        if (target.tag == "TutorialEnemy")
                        {
                            this.tutorialEnemy.GetHit();//얘는 사운드 나오는데 
                        }
                        else
                        {
                            this.aerialEnemy.GetHit(target, -100f);// 얘는 사운드 안나옴
                            GetComponent<AudioSource>().PlayOneShot(audioClips[1]);
                        }
                        EventDispatcher.instance.SendEvent((short)eEventType.COLLISION_WITH_PLANE);
                        this.shieldEff.gameObject.SetActive(false);
                        this.hitParticle.gameObject.SetActive(true);
                        this.detectPlane.gameObject.SetActive(false);
                        this.hitGround.gameObject.SetActive(true);
                        //this.hitEff.SetActive(true);
                        //햅틱 넣기
                        OVRInput.SetControllerVibration(0.9f, 0.5f, OVRInput.Controller.LTouch);
                        OVRInput.SetControllerVibration(0.9f, 0.5f, OVRInput.Controller.RTouch);

                        Invoke("TurnOff", 1f);
                    }
                    else
                    {
                        Debug.Log("===> 아래에서 찍음");
                    }

                };
            }
            else
            {
                this.shieldEff.gameObject.SetActive(false);
            }

            if (this.isGrabIndexTrigger)
            {
                this.delta += Time.deltaTime;
                if (delta > 1f)
                {
                    delta = 0;
                    Debug.LogFormat("==> {0}", this.shield.lHandAnchor.transform.position.y);
                    if (this.posQueue.Count > 10)
                    {
                        this.posQueue.Dequeue();
                    }
                    this.posQueue.Enqueue(this.shield.lHandAnchor.transform.position.y);
                }
            }

            yield return null;
        }
    }

    private void TurnOff()
    {
        //this.hitEff.SetActive(false);
        this.hitParticle.gameObject.SetActive(false);
    }

   
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, this.radius);
    }
}
