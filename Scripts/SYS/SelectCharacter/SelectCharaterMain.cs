using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using Unity.VisualScripting;
using Oculus.Interaction.Input;


public class SelectCharaterMain : MonoBehaviour
{
    [SerializeField] private CharacterButton[] characterButtons;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private TMP_Text txtName;
    [SerializeField] private GameObject[] wholeBodyModelGo;
    [SerializeField] private GameObject[] star;
    [SerializeField] private GameObject centerEyeAnchor;
    [SerializeField]
    private Animator anim;
    public AudioClip[] audioClips;

    private MyOVRScreenFade OFade;
    private float normalizedPositionX ;
    private bool isMoved;
    private int num;
    private float time = 0f;
    private bool isAppear; 

    public static bool isClearCaptain = false;
    // Start is called before the first frame update
    void Start()
    {
        OFade=this.centerEyeAnchor.transform.GetComponent<MyOVRScreenFade>();
        if (isClearCaptain==false)
        {
            this.scrollRect.normalizedPosition = new Vector2(0.4f, 0);
            this.num = 4;
            this.normalizedPositionX = 0.4f;
        }
        else//클리어하고 왔으면
        {
            this.PlayMoveSound(4);
            this.scrollRect.normalizedPosition = new Vector2(0.2f, 0);
            this.normalizedPositionX = 0.2f;
            this.StartCoroutine(this.CoUnLock());
        }
    }

    // Update is called once per frame
    void Update()
    {
        this.ActiveButton();
        this.NameChange();
        this.ActiveWholeBody();
        this.StartCoroutine(CoThumb());
        this.time += Time.deltaTime;
        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
        {
            OFade.FadeOut();
            this.PlayMoveSound(1);
            //캐릭터 선택 오디오
        }
    }

    private IEnumerator CoUnLock()
    {
        yield return new WaitForSeconds(1.5f);
        this.anim.SetBool("isClear", true);
        //오디오 삽입
        this.PlayMoveSound(3);
        yield return new WaitForSeconds(1.5f);
        this.num = 3;
    }

    private IEnumerator CoThumb()
    {
        Vector2 thumbstickValueL = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch);
        Vector2 thumbstickValueR = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);

        if (isMoved == false)
        {
            if (thumbstickValueL.x  >0 ||thumbstickValueR.x>0)//right
            {
                this.MoveRight();
                isMoved = true;
                yield return new WaitForSeconds(0.3f);
                isMoved = false;
            }
            else if (thumbstickValueL.x< 0 || thumbstickValueR.x < 0)//left
            {
                this.MoveLeft();
                isMoved = true;
                yield return new WaitForSeconds(0.3f);
                isMoved = false;
            }
        }
    }

    private void MoveRight()
    {
        this.num++;
        this.time = 0;
        this.PlayMoveSound(0);
        if (num > 2 && num < 7)
        {
            normalizedPositionX += 0.2f;
            this.scrollRect.normalizedPosition = new Vector2(normalizedPositionX, 0);
        }

        if (num >= 9) num = 9;
        if (num >= 7)
        {
            normalizedPositionX = 1;
            this.scrollRect.normalizedPosition = new Vector2(1, 0);
        }
    }

    private void MoveLeft()
    {
        this.PlayMoveSound(0);
        this.num--;
        this.time = 0;
        if (num > 2 && num < 7)
        {
            normalizedPositionX -= 0.2f;
            this.scrollRect.normalizedPosition = new Vector2(normalizedPositionX, 0);
        }

        if (num <= 0) num = 0;
        if (num <=2 )
        {
            normalizedPositionX = 0;
            this.scrollRect.normalizedPosition = new Vector2(0, 0);
        }
    }

    //버튼 활성화
    private void ActiveButton()
    {
        //캡틴, 아이언맨
        for (int i = 3; i < 5; i++)
        {
            if (isClearCaptain == true)//미션 클리어했을때
            {
                if (i == num) this.characterButtons[i].onPointerEnterActive();
                else this.characterButtons[i].onPointerExit();
            }
            else//클리어 하지 않았을 때
            {
                if (num==4&&i==num) this.characterButtons[i].onPointerEnterActive();//캡틴
                else if (num == 3 && i == num) this.characterButtons[i].onPointerEnterUnActive();//아이언맨
                else this.characterButtons[i].onPointerExit();
            }
        }

        //나머지 버튼
        for (int i = 0; i < this.characterButtons.Length; i++)
        {
            if (i == num && i != 3 && i != 4) this.characterButtons[i].onPointerEnterUnActive();
            else if (i != 3 && i != 4)
            {
                this.characterButtons[i].onPointerExit();
            }
        }
    }

    private void ActiveWholeBody()
    {
        if (num == 3 && this.time > 1.5f&&isClearCaptain==true)
        {
            if (isAppear == false)
            {
                this.PlayMoveSound(2);//계속 호출됨
                isAppear = true;
            }

            this.wholeBodyModelGo[0].SetActive(true);
            this.wholeBodyModelGo[1].SetActive(false);
        }
        if (num == 4 && this.time > 1.5f)
        {
            if (isAppear == false)
            {
                this.PlayMoveSound(2);//계속 호출됨
                isAppear = true;
            }
            this.wholeBodyModelGo[0].SetActive(false);
            this.wholeBodyModelGo[1].SetActive(true);
        }
        else if(num!=3||num!=4&&time<1.5f)
        {
            isAppear = false;
            this.wholeBodyModelGo[0].SetActive(false);
            this.wholeBodyModelGo[1].SetActive(false);
        }
    }

    public void PlayMoveSound(int clipNum)
    {
        AudioClip clip = this.audioClips[clipNum];
        if (clipNum == 4)
        {
            GetComponent<AudioSource>().pitch = 1f;
        }
        GetComponent<AudioSource>().PlayOneShot(clip);
    }

    private void NameChange()
    {
        switch (num)
        {
            case 0:
                this.txtName.text = "Spiderman";
                for(int i = 0; i < 5; i++)
                {
                    if (i != 4)
                    {
                        this.star[i].SetActive(false);
                    }
                }
                this.star[4].SetActive(true);
                break;
            case 1:
                this.txtName.text = "Doctor Strange";
                for (int i = 0; i < 5; i++)
                {
                    if (i != 3)
                    {
                        this.star[i].SetActive(false);
                    }
                }
                this.star[3].SetActive(true);
                break;
            case 2:
                this.txtName.text = "Thor";
                for (int i = 0; i < 5; i++)
                {
                    if (i != 1)
                    {
                        this.star[i].SetActive(false);
                    }
                }
                this.star[1].SetActive(true);
                break;
            case 3:
                this.txtName.text = "Ironman";
                for (int i = 0; i < 5; i++)
                {
                    if (i != 2)
                    {
                        this.star[i].SetActive(false);
                    }
                }
                this.star[2].SetActive(true);
                break;
            case 4:
                this.txtName.text = "Captain America";
                for (int i = 0; i < 5; i++)
                {
                    if (i != 1)
                    {
                        this.star[i].SetActive(false);
                    }
                }
                this.star[1].SetActive(true);
                break;
            case 5:
                this.txtName.text = "Hulk";
                for (int i = 0; i < 5; i++)
                {
                    if (i != 0)
                    {
                        this.star[i].SetActive(false);
                    }
                }
                this.star[0].SetActive(true);
                break;
            case 6:
                this.txtName.text = "Black Widow";
                for (int i = 0; i < 5; i++)
                {
                    if (i != 1)
                    {
                        this.star[i].SetActive(false);
                    }
                }
                this.star[1].SetActive(true);
                break;
            case 7:
                this.txtName.text = "Black Panther";
                for (int i = 0; i < 5; i++)
                {
                    if (i != 2)
                    {
                        this.star[i].SetActive(false);
                    }
                }
                this.star[2].SetActive(true);
                break;
            case 8:
                this.txtName.text = "Captain Marvel";
                for (int i = 0; i < 5; i++)
                {
                    if (i != 3)
                    {
                        this.star[i].SetActive(false);
                    }
                }
                this.star[3].SetActive(true);
                break;
            case 9:
                this.txtName.text = "Scarlet Witch";
                for (int i = 0; i < 5; i++)
                {
                    if (i != 4)
                    {
                        this.star[i].SetActive(false);
                    }
                }
                this.star[4].SetActive(true);
                break;
        }
    }

}
