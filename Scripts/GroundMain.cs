using OVR.OpenVR;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class GroundMain : MonoBehaviour
{
    [SerializeField] private GameObject SpaceStoneGo;
    public OBJManager objManager;
    public GameObject enemyGo;
    
    private float curTime;
    private float maxTime;

    public List<Transform> spawnList = new List<Transform>();
    private int rdnSpawn;

   
    GameObject enemyShield;
    private GameObject fill;
    private GameObject hpTxt;
    private GameObject hpAnim;
    private float hp = 100;
    private GameObject groundEnemyGo;
    [SerializeField] private GameObject SpaceStoneEffect;
    private Animator anim;
    public GameObject car;
    public GameObject carLight;
    public AudioClip audioClip;
    private bool isPlay;
    private void Start()
    {
        
        fill = GameObject.Find("fill");
        hpTxt = GameObject.Find("HPtxt");
        hpAnim = GameObject.Find("PlayerHp");
        this.anim = car.GetComponent<Animator>();
    }
    private void Update()
    {
        curTime += Time.deltaTime;
        if (curTime > maxTime)
        {
            //3초마다 적스폰
            maxTime = 3;
            this.CreateGroundEnemy();
            curTime = 0;
        }
        this.groundEnemyGo=GameObject.Find("GroundEnemy(Clone)");
    }

    private void CreateGroundEnemy()
    {
       
        this.rdnSpawn = Random.Range(0, spawnList.Count);
        Debug.LogFormat("<color=lime><size=20> rdnSpawn : {0}</size></color>", rdnSpawn);
        if (spawnList.Count <= 0) 
        {
            if (groundEnemyGo == null)
            {
               
                this.anim.SetBool("IsFinish", true);
                StartCoroutine(CoCreateCube());
                if (this.isPlay == false)
                {
                    GetComponent<AudioSource>().PlayOneShot(audioClip);
                    this.isPlay = true;
                }
                this.carLight.SetActive(false);
            }
            return;
        }

        GameObject enemy = OBJManager.instance.GetEnemy("groundEnemy");

        if (enemy == null)
            return;

        var targetPosition = spawnList[rdnSpawn].transform.position;


        enemy.transform.position = targetPosition;
        enemy.transform.rotation = spawnList[rdnSpawn].transform.rotation;
       

        spawnList.Remove(spawnList[rdnSpawn]);

    }

    private IEnumerator CoCreateCube()
    {
        yield return new WaitForSeconds(1.5f);
        this.SpaceStoneEffect.SetActive(true);
        yield return new WaitForSeconds(3f);
        this.SpaceStoneGo.SetActive(true);

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
            //Debug.LogFormat("<color=lime>====hp : {0}=====</color>", this.hp);
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
