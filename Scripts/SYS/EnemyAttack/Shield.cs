using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using System;
using UnityEditor;
using Unity.VisualScripting;
using TMPro;

public class Shield : MonoBehaviour
{
    [SerializeField] private GameSceneMain gameSceneMain;
    [SerializeField] private Transform centerEyeAnchor; //눈의 위치
    public Transform lHandAnchor; //왼쪽 손의 위치 
    public Transform pos; //방패의 위치
    public float av = 0f;//컨트롤러 속도

    private float moveSpeed = 15;
    private float dot;
    private Vector3 dir;
    private float degrees = 0;
    private Coroutine moveCoroutine;
    private Vector3 targetDir;

    private bool isShoot = true;
    private bool isGrab;

    Dictionary<GameObject, float> dic = new Dictionary<GameObject, float>();
    private float detectRadius = 10f;
    private Vector3 startPos;
    private IEnumerator shakeRoutine;
    private TrailRenderer trailRenderer;
    private GameObject target;
    public AudioClip audioClip;
    private bool isPlay;
    private void Start()
    {
        //방패 진동
        this.shakeRoutine = this.CoShake(0.005f);
        this.startPos = this.transform.localPosition;
        this.trailRenderer = this.GetComponent<TrailRenderer>();
    }

    void FixedUpdate()
    {
        //컨트롤러 속도 구하기
        var velocity = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.LTouch); 
        this.av = Mathf.Abs((float)Math.Truncate(((velocity.x + velocity.y + velocity.z) / 3.0f) * 100f) / 100f); //.normalized;

        //팔을 스윙했는가 ?
        if (OVRInput.Get(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch) && this.av >= 0.45f) 
        {
            this.isShoot = false;
            this.isPlay = false;
        }

        if (this.isShoot == false)
        {
            //버튼에서 손이 떼어져있고, 속도가 어느정도 감소했을 때
            if (!OVRInput.Get(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch) && this.av < 0.06f)
            {
                if (isPlay == false)
                {
                    GetComponent<AudioSource>().PlayOneShot(audioClip);
                    this.isPlay = true;
                }

                Debug.LogFormat("<color=lime>손을 떼었을 때 날라감</color>");

                this.trailRenderer.enabled = true;
                this.isShoot = true;
                this.pos.SetParent(null);

                this.dir = this.centerEyeAnchor.forward;
                this.moveCoroutine = this.StartCoroutine(this.CoMove());

            }

        }
        //다시 잡았을 때
        if (isGrab == true)
        {
            this.pos.transform.position = Vector3.MoveTowards(this.pos.transform.position, lHandAnchor.position, 10f * Time.deltaTime);
            isShoot = true;
            
            if (Vector3.Distance(this.pos.transform.position, lHandAnchor.position) < 0.2f)
            {
                Debug.Log("다시 잡았다");
                this.Init();
                isGrab = false;
                this.trailRenderer.enabled = false;

                Debug.LogFormat("this.moveCoroutine: {0}", this.moveCoroutine);
                Debug.LogFormat("moveToEnemyRoutine : {0}", this.moveToEnemyRoutine);

                if (this.moveToEnemyRoutine != null)
                    StopCoroutine(this.moveToEnemyRoutine);

                if (this.moveCoroutine != null)
                    StopCoroutine(this.moveCoroutine);
            }
        }
        
    }


    IEnumerator CoMove()
    {
        //DrawArrow.ForDebug(this.pos.position, centerEyeAnchor.forward, 1f, Color.red, ArrowType.Solid);
        //Debug.LogFormat("<color=red>targetdot: {0}</color>", targetDot);
        this.Dorotate();
        var target = gameSceneMain.FindEnemyDir();
        
        //적이 있는 경우
        if (target != null)
        {
            this.targetDir = (target.transform.position - this.pos.transform.position).normalized;

            //지상적 몸쪽으로 날라가기
            if (target.name == "GroundEnemy(Clone)")
            {
                targetDir.y += 0.08f;
            }
        }
        //적이 없는 경우
        else if (target == null)
        {
            targetDir = (this.lHandAnchor.position - this.pos.position).normalized;
        }

        //내적하기
        this.dot = Vector3.Dot(this.pos.forward, centerEyeAnchor.forward); //손의 앞방향과 시선 내적
        var targetDot = Vector3.Dot(targetDir, centerEyeAnchor.forward); // 타겟의 방향과 시선 내적
        
        while (true)
        {
            //손과 눈의 위치가 비슷할때(30도 이하)
            if (this.dot > 0.8f)
            {
                //타겟방향으로 던졌을때 타겟방향으로 날라감
                if (targetDot > 0.9f)
                {
                    this.pos.Translate(targetDir * this.moveSpeed * Time.fixedDeltaTime, Space.World);
                }

                //다른 쪽으로 던졌을때 눈의 앞방향으로 날라감
                else
                {
                    this.pos.Translate(dir * this.moveSpeed * Time.fixedDeltaTime, Space.World);
                }
                
            }
            //손과 눈의 위치가 다를 때 손의 앞방향으로 날라가기
            else
            {

                targetDir = this.pos.forward;
                this.pos.Translate(targetDir * this.moveSpeed * Time.fixedDeltaTime, Space.World);

            }

            if (OVRInput.Get(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch) && this.pos.position != lHandAnchor.position)
            {
                isGrab = true;
                StopCoroutine(this.moveCoroutine);
                //DrawArrow.ForDebug(this.pos.transform.position, dir, 10f, Color.blue, ArrowType.Solid);
            }

            yield return null;
        }

    }

    public void Init()
    {
        Debug.Log("위치 초기화 완료");
        this.pos.SetParent(lHandAnchor);
        this.pos.localPosition = new Vector3(0, 0, 0);
        this.pos.localRotation = Quaternion.Euler(0, 0, 0);
        this.transform.localRotation=Quaternion.Euler(0, -83, 0);
        DOTween.Kill("rotateTween");
    }
    
    public void Dorotate()
    {
        this.transform.DORotate(new Vector3(-90, -83, 0), 1f).SetId("rotateTween");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            if (this.moveCoroutine != null) this.StopCoroutine(this.moveCoroutine);
            if (this.moveToEnemyRoutine != null) this.StopCoroutine(this.moveToEnemyRoutine); 


            this.target = this.FindNextTarget();
            Debug.LogFormat("<color=lime>collision.gameObject:{0}</color>",collision.gameObject.name);
            
            if (target != null)
            {
                this.RotateShield(target);

                if (target.name == "GroundEnemy(Clone)")
                {
                    this.moveToEnemyRoutine = this.StartCoroutine(this.CoMoveToEnemy(target.transform.position + new Vector3(0, 1, 0), () => {

                        Debug.LogFormat("onMovecomplete");

                        dir = (this.lHandAnchor.position - this.pos.position).normalized;
                        this.moveCoroutine = this.StartCoroutine(this.CoMove());
                    }));
                }
                else
                {
                    this.moveToEnemyRoutine = this.StartCoroutine(this.CoMoveToEnemy(target.transform.position, () => {

                        Debug.LogFormat("onMovecomplete");

                        dir = (this.lHandAnchor.position - this.pos.position).normalized;
                        this.moveCoroutine = this.StartCoroutine(this.CoMove());
                    }));
                }


            }
            else  // 모든 적을 맞추고 타겟이 없음
            {
                Debug.Log("<color=yellow>=> 타겟이 없음</color>");
                this.target = null;
                //되돌아옴 
                this.moveCoroutine = this.StartCoroutine(this.CoMove());
            }

        }

        else if (collision.collider.CompareTag("Plane"))
        {

            if (this.isShaking == false)
            {
                this.isShaking = true;
                //cam.Shake();
            }
            this.onCollisionPlane(collision.GetContact(0).point);
        }

        else //부딪힌게 enemy가 아님
        {
            dir = (this.lHandAnchor.position - this.pos.position).normalized;
        }

    }

    private void RotateShield(GameObject target)
    {
        if (target != null)
        {
            DOTween.Kill("rotateTween");
            //적 쳐다보기
            var dir = (target.transform.position - this.transform.position).normalized;
            this.transform.up = dir;
            
            if (this.transform.position.x < target.transform.position.x)
            {
                degrees = 270;
                Debug.LogFormat("<color=yellow>오른쪽에 있음 : {0}</color>", degrees);

            }
            else if (this.transform.position.x > target.transform.position.x)
            {
                degrees = 90;
                Debug.LogFormat("<color=yellow>왼쪽에 있음 : {0}</color>", degrees);
            }
            else
            {
                Debug.LogFormat("<color=yellow>같은 위치에 있음 : {0}</color>", degrees);
            }
            this.transform.rotation = this.transform.rotation * Quaternion.Euler(0, degrees, 0);
        }
        else if (target == null)
        {
            this.Dorotate();
        }
        
    }

    
    private GameObject FindNextTarget()
    {

        Collider[] enemyCols = Physics.OverlapSphere(this.pos.transform.position, this.detectRadius, 1 << 6);
        this.dic.Clear();
        
        for (int i = 0; i < enemyCols.Length; i++)
        {
            var col = enemyCols[i];
            this.dic.Add(col.gameObject, Vector3.Distance(col.transform.position, this.pos.transform.position));
        }

        if (this.dic.Count > 0 )
        {
            var nearestDistance = this.dic.Values.Min();
            var target = this.dic.FirstOrDefault(x => x.Value == nearestDistance).Key;

            return target;
        }
        else
        {
            dir = (this.lHandAnchor.position - this.pos.position).normalized;
            return null;
        }

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.pos.transform.position, this.detectRadius);
    }

    private Coroutine moveToEnemyRoutine;

    //두번째 부터 사용 
    private IEnumerator CoMoveToEnemy(Vector3 targetPosition, System.Action onComplete)
    {
        Debug.LogFormat("<color=cyan> <size=20>CoMoveToEnemy </size> </color>");
        while (true) {

            var dir = (targetPosition - this.pos.position).normalized;
            DrawArrow.ForDebug(this.pos.transform.position, dir, 10f, Color.red, ArrowType.Solid);
            
            //this.pos.transform.Translate(dir * this.moveSpeed * Time.deltaTime, Space.World);
            this.pos.transform.position = Vector3.MoveTowards(this.pos.transform.position, targetPosition, 0.5f);
            var dis = Vector3.Distance(this.pos.transform.position, targetPosition);
            
            if (dis < 0.1f)
            {
                Debug.Log("<color=yellow>=>dis<0.1f</color>");
                onComplete();

                yield break;
            }
            yield return null;
        }

    }



    //------------------------- 방패 진동 -----------------------------------
    private void OnParticleCollision(GameObject other)
    {
        
        this.StartCoroutine(this.shakeRoutine);
        if (this.shakeRoutine != null) StopCoroutine(this.shakeRoutine);
    }

    public void Shake()
    {
        this.StartCoroutine(this.shakeRoutine);
    }
    public void StopShake()
    {
        this.StopCoroutine(this.shakeRoutine);
    }

    //방패 진동하는 코루틴 함수
    private IEnumerator CoShake(float magnitude)
    {
        while (true)
        {
            transform.localPosition = (Vector3)UnityEngine.Random.insideUnitSphere * magnitude + startPos;
            yield return null;
        }
    }


    public System.Action<Vector3> onCollisionPlane;
    public System.Action onCollisionEnemy;
   // public CameraShaking cam;
    public bool isShaking = false;
   
}
