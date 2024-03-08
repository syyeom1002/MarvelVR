using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneMain : MonoBehaviour
{ 
    [SerializeField]
    private Shield shield;
    [SerializeField]
    private GameObject aerialEnemyPrefab;
    public Transform[] points;
    [SerializeField]
    private GameObject centerEyeAnchor;
    [SerializeField]
    private PlayerController playerController;
    public GameObject[] pathGo;

    private AerialEnemy[] enemyGo;
    public int enemyNum=0;
    Dictionary<GameObject, float> mainDic = new Dictionary<GameObject, float>();
    private float detectEnemyRadius=20;
    private OVRScreenFade OFade;
    public bool isFirstEnemy=true;
    public bool isLast = false;
    public bool isSecond = true;


    private GameObject fill;
    private GameObject hpTxt;
    private GameObject hpAnim;
    private float hp = 100;
  

   


    private void Start()
    {
        OFade = this.centerEyeAnchor.transform.GetComponent<OVRScreenFade>();
        OFade.FadeIn();

        fill = GameObject.Find("fill");
        hpTxt = GameObject.Find("HPtxt");
        hpAnim = GameObject.Find("PlayerHp");
       
    }
    private void Update()
    {
        this.enemyGo = GameObject.FindObjectsOfType<AerialEnemy>();

        foreach (var enemy in enemyGo)
        {
            var laser=enemy.GetComponentInChildren<Laser>();
            laser.onGetHit = () =>
            {
               this.shield.Shake();
            };
            laser.onStopGetHit = () =>
            {
                this.shield.StopShake();
            };
        }

        this.FindShield();

        
    }

   

    public void CreateEnemy()
    {
        if (this.enemyGo.Length == 0)
        {
            //첫번째 포인트에서 생성되는 적임(최대 8마리 3-3-2)
            if (isFirstEnemy == true)
            {
                if (enemyNum < 6)
                {
                    //적 3마리 생성
                    for (int i = 0; i < 3; i++)
                    {
                        Instantiate(this.aerialEnemyPrefab, this.points[i].position, Quaternion.identity);
                        enemyNum++;
                    }
                }
                else if (enemyNum >= 6 && enemyNum < 8)
                {
                    //2마리 생성
                    for (int i = 0; i < 2; i++)
                    {
                        Instantiate(this.aerialEnemyPrefab, this.points[i].position, Quaternion.identity);
                        enemyNum++;
                    }
                }
                else
                {
                    //더이상 생성하지 X
                    //첫번째 인디케이터 생성
                    this.pathGo[0].SetActive(true);
                    this.playerController.GetComponent<PlayerController>().enabled = true;
                    return;
                }
            }
            //두번째 포인트에서 생성되는 적임(최대 7마리 2-2-1)
            else
            {
                
                if (enemyNum < 2)
                {
                    for (int i=3;i<5; i++)
                    {
                        Instantiate(this.aerialEnemyPrefab, this.points[i].position, Quaternion.identity);
                        enemyNum++;
                        
                    }
                }
                else if (enemyNum >= 2 && enemyNum < 4)
                {
                    for (int i = 5; i < 7; i++)
                    {
                        Instantiate(this.aerialEnemyPrefab, this.points[i].position, Quaternion.identity);
                        enemyNum++;
                    }
                }
                else if (enemyNum >= 4&&enemyNum<5 )
                {
                    this.isLast = true;
                    Instantiate(this.aerialEnemyPrefab, this.points[7].position, Quaternion.identity);
                    enemyNum++;
                }
                else
                {
                    //두번째 인디케이터 생성
                    this.pathGo[1].SetActive(true);
                    if (this.isSecond == true)
                    {
                        this.playerController.SecondMove();
                        this.isSecond = false;
                    }
                    
                    return;
                }
            }
        }
    }

    private void FindShield()
    {
        var shieldGo = GameObject.FindWithTag("Shield");
        var dis = Vector3.Distance(this.centerEyeAnchor.transform.position, shieldGo.transform.position);
        if (dis > 20f&& OVRInput.Get(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch))
        {
            this.shield.Init();
        }
    }
   

    public GameObject FindEnemyDir()
    {
        Collider[] enemyCols = Physics.OverlapSphere(this.centerEyeAnchor.transform.position, detectEnemyRadius, 1 << 6);
        this.mainDic.Clear();
        for (int i = 0; i < enemyCols.Length; i++)
        {
            var col = enemyCols[i];
            Vector3 mainDir = (col.transform.position - this.centerEyeAnchor.transform.position).normalized;
            var dot = Vector3.Dot(this.shield.pos.forward, mainDir);
            this.mainDic.Add(col.gameObject,dot );
        }

        if (this.mainDic.Count > 0)
        {
            Debug.Log(mainDic.Count);
            var nearestDir = this.mainDic.Values.Max();//가장 방향이 비슷한 적
            var target = this.mainDic.FirstOrDefault(x => x.Value == nearestDir).Key;
            return target;
        }
        else
        {
           Debug.Log("반경안에 적이 없습니다.");
           return null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.centerEyeAnchor.transform.position, 20);
    }

    public void DecreaseHp()
    {
        Image hpGauge = this.fill.GetComponent<Image>();

        hpGauge.fillAmount -= 0.001f;
        TMP_Text hptxt = this.hpTxt.GetComponent<TMP_Text>();

        this.hp -= 0.1f;
        hptxt.text = this.hp.ToString("F0");
        if (hpGauge.fillAmount == 0)
        {

            hptxt.text = string.Format("{0}", 0);

        }
        Animator hpanim = this.hpAnim.GetComponent<Animator>();
        hpanim.SetBool("IsHit", true);
    }


    public void IncreaseHp()
    {
        Image hpGauge = this.fill.GetComponent<Image>();

        hpGauge.fillAmount += 0.001f;
        TMP_Text hptxt = this.hpTxt.GetComponent<TMP_Text>();

        if (this.hp < 100)
        {
            this.hp += 0.1f;
        }
        hptxt.text = this.hp.ToString("F0");
        if (hpGauge.fillAmount == 1)
        {

            hptxt.text = string.Format("{0}", 100);
        }
        Animator hpanim = this.hpAnim.GetComponent<Animator>();
        hpanim.SetBool("IsHit", false);
    }
}
