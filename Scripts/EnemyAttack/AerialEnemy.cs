using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class AerialEnemy : MonoBehaviour
{
    [SerializeField] private SphereCollider childCollider;
    [SerializeField] private GameObject laserStaticEffect;
    [SerializeField] private Laser laser;
    [SerializeField] private GameObject getHitEffect;
    private GameSceneMain gameSceneMain;
    private Coroutine coroutine;
    private Coroutine randomMoveCoroutine;
    private SphereCollider parentCollider;
    private float time=0f;
    private Vector3 position;
    public GameObject sparkEff;
    private void Start()
    {
        this.gameSceneMain = GameObject.FindObjectOfType<GameSceneMain>();
        this.parentCollider = this.gameObject.GetComponent<SphereCollider>();
        this.getHitEffect.SetActive(false);
        this.Move();
       
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Shield"))
        {
            this.GetHit(this.gameObject,-200);
            //for (int i = 0; i < collision.contactCount; i++)
            //{
            //    ContactPoint contactPoint = collision.GetContact(i);

            //}

            //ContactPoint cp = collision.GetContact(0);

            //var rot = Quaternion.LookRotation(-1 * cp.normal);
            //GameObject spark = Instantiate(this.sparkEff, cp.point, rot);
            //Destroy(spark, 0.5f);
        }
    }
    
    public void GetHit(GameObject target,float force)
    {
        var tRbody = target.gameObject.GetComponent<Rigidbody>();
        var tAerialEnemy = target.GetComponent<AerialEnemy>();
        tRbody.useGravity = true;
        Destroy(target.gameObject, 1f);
        tAerialEnemy.parentCollider.enabled = false;
        tAerialEnemy.childCollider.enabled = true;
        tRbody.AddForce(transform.forward * force);
        tRbody.AddForce(transform.right * Random.Range(-100,100));
        tAerialEnemy.getHitEffect.SetActive(true);
        tAerialEnemy.laserStaticEffect.SetActive(false);
        tAerialEnemy.laser.enabled = false;
        tAerialEnemy.StopCoroutine();
        Debug.Log("GetHit");
        
    }

    public void StopCoroutine()
    {
        if (this.coroutine != null) StopCoroutine(this.coroutine);
        if (this.randomMoveCoroutine != null) StopCoroutine(this.randomMoveCoroutine);
    }

    private void Move()
    {
        if (gameSceneMain.isFirstEnemy)
        {
            this.coroutine = this.StartCoroutine(this.CoFirstEnemyMove());
        }
        else if (gameSceneMain.isLast==true)
        {
            this.coroutine = this.StartCoroutine(this.CoLastEnemyMove());
        }
        else
        {
            this.coroutine = this.StartCoroutine(this.CoSecondEnemyMove());
            Debug.Log("두번째 적 움직이기");
        }
    }

    //-------------------------------------------------첫번째 적------------------------------------------------------------
    //생성되고 랜덤 위치로 이동
    private IEnumerator CoFirstEnemyMove()
    {
        this.position = this.transform.position + new Vector3(Random.Range(-0.6f, 0.6f), Random.Range(-0.5f, 1.0f), Random.Range(-8f, -4f));
        this.laserStaticEffect.SetActive(false);
        this.laser.enabled = false;
        while (true)
        {
            this.transform.LookAt(position);
            this.transform.Translate(Vector3.forward * Random.Range(1f,15f) * Time.deltaTime);
            var dis = Vector3.Distance(position, this.transform.position);
            if (dis < 0.2f)
            {
                this.coroutine=this.StartCoroutine(this.CoAttackAndMove());
                break;
            }

            yield return null;
        }
    }


    //공격-> 랜덤이동-> 공격-> 랜덤이동 ..
    private IEnumerator CoAttackAndMove()
    {
        this.position = this.transform.position + new Vector3(Random.Range(-0.8f, 1.2f), Random.Range(-1f, 0.8f), Random.Range(-1f, 1.2f));
        while (true)
        {
            this.time += Time.deltaTime;
            if (Vector3.Distance(position, this.transform.position)<0.2f)
            {
                //Debug.Log("목표 위치 변경 ");
                if (this.time > 0.5f)
                {
                    this.laserStaticEffect.SetActive(true);
                    this.laser.enabled = true;
                    this.time = 0;
                }
                yield return new WaitForSeconds(0.7f);
                position = this.transform.position + new Vector3(Random.Range(-0.8f, 0.8f), Random.Range(-0.5f, 0.7f), Random.Range(-0.6f, 1f));
            }
            else
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, position, 0.05f);
                this.laserStaticEffect.SetActive(false);
                this.laser.enabled = false;
            }

            yield return null;
        }
    }

    //-----------------------------------------------두번째 적 ---------------------------------------------------------
    //생성되고 랜덤 위치로 이동
    private IEnumerator CoSecondEnemyMove()
    {
        this.position = this.transform.position + new Vector3(Random.Range(-0.4f, 0.6f), Random.Range(0f, 1f), Random.Range(-3f, -2f));
        this.laserStaticEffect.SetActive(false);
        this.laser.enabled = false;
        while (true)
        {
            this.transform.LookAt(position);
            this.transform.Translate(Vector3.forward * Random.Range(1f, 5f) * Time.deltaTime);
            var dis = Vector3.Distance(position, this.transform.position);
            if (dis < 0.2f)
            {
                this.coroutine = this.StartCoroutine(this.CoSecondAttackAndMove());
                break;
            }

            yield return null;
        }
    }
    private IEnumerator CoSecondAttackAndMove()
    {
        this.position = this.transform.position + new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.1f, 0.2f), Random.Range(-0.2f, 0.2f));
        while (true)
        {
            this.time += Time.deltaTime;
            if (Vector3.Distance(position, this.transform.position) < 0.2f)
            {
                //Debug.Log("목표 위치 변경 ");
                if (this.time > 0.3f)
                {
                    this.laserStaticEffect.SetActive(true);
                    this.laser.enabled = true;
                    this.time = 0;
                }
                yield return new WaitForSeconds(0.6f);
                this.position = this.transform.position + new Vector3(Random.Range(-0.4f, 0.5f), Random.Range(-0.2f, 0.25f), Random.Range(-0.45f, 0.45f));
            }
            else
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, position, 0.05f);
                this.laserStaticEffect.SetActive(false);
                this.laser.enabled = false;
            }

            yield return null;
        }
    }

    //-------------------------------------------마지막 하나 적 움직임----------------------------------------------------
    private IEnumerator CoLastEnemyMove()
    {
        this.laserStaticEffect.SetActive(false);
        this.laser.enabled = false;
        yield return new WaitForSeconds(1f);
        var position = gameSceneMain.points[4].position;
        while (true)
        {
            this.transform.LookAt(position);
            this.transform.Translate(Vector3.forward * 12f * Time.deltaTime);
            var dis = Vector3.Distance(position, this.transform.position);
            if (dis < 0.4f)
            {
                this.randomMoveCoroutine = StartCoroutine(this.CoLastAttackAndMove());
                break;
            }
            yield return null;
        }
    }

    private IEnumerator CoLastAttackAndMove()
    {
        this.position = this.transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.1f, 0.1f), Random.Range(-0.5f, 0.5f));
        while (true)
        {
            if (Vector3.Distance(position, this.transform.position) < 0.2f)
            {
                yield return new WaitForSeconds(0.6f);
                this.position = this.transform.position + new Vector3(Random.Range(-0.35f, 0.6f), Random.Range(-0.1f, 0.25f), Random.Range(-0.9f, 0.8f));
            }
            else
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, position, 0.25f);
            }

            yield return null;
        }
    }
}
