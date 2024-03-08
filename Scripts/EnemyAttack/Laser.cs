using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Laser : MonoBehaviour
{
    private LineRenderer lr;
    [SerializeField] private Transform attackPos;

    private GameObject main;
    public System.Action onGetHit;
    public System.Action onStopGetHit;
    private GameObject hpAnim;

    private void Start()
    {
        this.lr = GetComponent<LineRenderer>();
        main = GameObject.Find("GameSceneMain");
        hpAnim = GameObject.Find("PlayerHp");
    }

    void Update()
    {
        lr.SetPosition(0, this.attackPos.position);
        RaycastHit hit;

        Debug.DrawRay(this.attackPos.position, this.attackPos.forward * 50f, Color.red);
        if (Physics.Raycast(this.attackPos.position, this.attackPos.forward * 50f, out hit))
        {
            if (hit.collider.CompareTag("Shield"))
            {
                lr.SetPosition(1, hit.point);
                Quaternion rot = Quaternion.LookRotation(-hit.normal);
                this.onGetHit();
                //Debug.Log("방패에 맞았다");
                GameSceneMain gameMain = main.GetComponent<GameSceneMain>();
                gameMain.IncreaseHp();
            }
            //몸에 맞았을 때
            else if (hit.collider.CompareTag("MainCamera"))
            {
                //Debug.Log("레이저에 맞았다");
                //this.onHitLaser();
                //this.hp -= 1;
                GameSceneMain gameMain = main.GetComponent<GameSceneMain>();
                gameMain.DecreaseHp();
                //hp감소
            }
            else //쉴드 말고 다른거에 부딪힐 때
            {
                lr.SetPosition(1, hit.point);
                Quaternion rot = Quaternion.LookRotation(-hit.normal);
                //방패 진동 멈추기
                this.onStopGetHit();
                //Animator hpanim = this.hpAnim.GetComponent<Animator>();
                //hpanim.SetBool("IsHit", false);
            }
        }
        onStopGetHit();
    }
    
}

