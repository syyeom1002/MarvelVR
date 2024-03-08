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
                //Debug.Log("���п� �¾Ҵ�");
                GameSceneMain gameMain = main.GetComponent<GameSceneMain>();
                gameMain.IncreaseHp();
            }
            //���� �¾��� ��
            else if (hit.collider.CompareTag("MainCamera"))
            {
                //Debug.Log("�������� �¾Ҵ�");
                //this.onHitLaser();
                //this.hp -= 1;
                GameSceneMain gameMain = main.GetComponent<GameSceneMain>();
                gameMain.DecreaseHp();
                //hp����
            }
            else //���� ���� �ٸ��ſ� �ε��� ��
            {
                lr.SetPosition(1, hit.point);
                Quaternion rot = Quaternion.LookRotation(-hit.normal);
                //���� ���� ���߱�
                this.onStopGetHit();
                //Animator hpanim = this.hpAnim.GetComponent<Animator>();
                //hpanim.SetBool("IsHit", false);
            }
        }
        onStopGetHit();
    }
    
}

