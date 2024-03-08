using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class GroundLaser : MonoBehaviour
{
    private GameObject main;

    public System.Action onHitLaser;

    private ParticleSystem ps;
    private List<ParticleSystem.Particle> inside = new List<ParticleSystem.Particle>();
    //private GameObject fill;
    //private GameObject hpTxt;
    //private GameObject hpAnim;

    //private float hp =100;
    private GameObject body;

    private void Start()
    {
        main = GameObject.Find("GameSceneMain");
        ps = GetComponent<ParticleSystem>();
        body = GameObject.Find("CenterEyeAnchor");
        Debug.Log(body);
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Shield"))
        {
            //Debug.Log("���п� �¾Ҵ�");
            GameSceneMain groundMain = main.GetComponent<GameSceneMain>();
            groundMain.IncreaseHp();
        }
        else if (other.CompareTag("MainCamera"))
        {
            //Debug.Log("���� ����");
            GameSceneMain groundMain = main.GetComponent<GameSceneMain>();
            groundMain.DecreaseHp();
        }

    }

    
    

    

}
        
    



