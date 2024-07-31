using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class GroundEnemy : MonoBehaviour
{
    private Animator anim;
    private float time = 0;
    private int hp;
    private int maxHp;
    public Rigidbody rb;
    public GameObject dieEff;
    public GameObject laser;
    public GameObject sparkEff;
    //public AudioClip audioClip;

    private Coroutine corotine;
    private Coroutine activeCoroutine;
    private Coroutine soundCoroutine;
    //public TrailRenderer trailRenderer;
    //public GameObject enemyShieldGo;
    //GameObject enemyShield;
    private BoxCollider collider;

    private void OnEnable()
    {
        if (this.corotine != null)
        {
            this.corotine = StartCoroutine(CoMove());
            this.time = 0;
            //this.collider.enabled = true;
            StartCoroutine(CoActiveShield());
            //StartCoroutine(this.CoPlayLaserSound());
        }
        if (this.activeCoroutine != null)
        {
            Debug.Log("===================�ڷ�ƾ �����ض�=====================");
            StopCoroutine(this.activeCoroutine);
        }
    }
    //// Start is called before the first frame update
    void Start()
    {
        this.anim = this.GetComponent<Animator>();
        this.rb = this.GetComponent<Rigidbody>();
        this.collider = this.GetComponent<BoxCollider>();
        this.corotine = StartCoroutine(CoMove());
        StartCoroutine(CoActiveShield());
        //this.soundCoroutine=StartCoroutine(this.CoPlayLaserSound());
        Debug.Log("�ڷ�ƾ ȣ��");
        
    }
    void Update()
    {
        this.time += Time.deltaTime;
       
    }

    private IEnumerator CoActiveShield()
    {
        yield return new WaitForSeconds(3f);
        this.collider.enabled = true;
    }
    public IEnumerator CoMove()
    {
        while (true)
        {
            if (this.time < 3)
            {
                this.anim.SetInteger("State", 0);

            }
            else if (this.time >= 3 && this.time <= 3.1)
            {
                this.anim.SetInteger("State", 1); //���� �̵�   
                this.rb.velocity = Vector3.back;
                this.transform.position += this.rb.velocity;



            }
            else if (this.time > 7 && this.time <= 7.1)
            {//������ �̵�
                this.anim.SetInteger("State", 2);
                this.rb.velocity = Vector3.forward;
                this.transform.position += this.rb.velocity;

            }
            else if (this.time > 10)
            {
                this.time = 0;
            }
            yield return null;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.collider.CompareTag("Shield"))
        {
            StopCoroutine(this.corotine);
           
            this.collider.enabled = false;
            this.anim.SetBool("IsDie", true); //�״� �ִϸ��̼�
            dieEff.SetActive(true); //�״� ����Ʈ
            laser.SetActive(false); //������ ����
            this.activeCoroutine = this.StartCoroutine(this.UnActive());
            Debug.Log("�׾���");

            ////�� ���� �� ������ ����Ʈ
            for (int i = 0; i < collision.contactCount; i++)
            {
                ContactPoint contactPoint = collision.GetContact(i);

            }

            ContactPoint cp = collision.GetContact(0);

            var rot = Quaternion.LookRotation(-1 * cp.normal);
            GameObject spark = Instantiate(this.sparkEff, cp.point, rot);
            Destroy(spark, 0.5f);
            //StopCoroutine(this.soundCoroutine);
        }

    }

    //private IEnumerator CoPlayLaserSound()
    //{
    //    yield return new WaitForSeconds(3f);
    //    while (true)
    //    {
    //        GetComponent<AudioSource>().PlayOneShot(audioClip);
    //        yield return new WaitForSeconds(0.6f);
    //    }
    //}

    private IEnumerator UnActive()
    {
        yield return new WaitForSeconds(2f);
        Debug.Log("===================�ڷ�ƾ �ϴ���=====================");
        this.gameObject.SetActive(false);
        dieEff.SetActive(false);
        //this.collider.enabled = true;
        laser.SetActive(true);
        OBJManager.instance.RelaseObject(this.gameObject);

    }





}
