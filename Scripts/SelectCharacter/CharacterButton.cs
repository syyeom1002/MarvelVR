using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterButton : MonoBehaviour
{
    [SerializeField]
    private Image btnImage;
    [SerializeField]
    private GameObject bg;
    private Image image;
    private Color btnColor;
    private RectTransform rectTransform;
    private Animator anim;

    void Start()
    {
        this.image = this.btnImage.GetComponent<Image>();
        this.rectTransform = this.btnImage.GetComponent<RectTransform>();
        this.btnColor = image.color;
        this.anim = this.GetComponent<Animator>();
    }

    public void onPointerEnterActive()
    {
        //알파 증가
        btnColor.a = 1f;
        image.color = btnColor;
        //크기증가
        this.anim.SetInteger("State", 1);
        this.bg.SetActive(true);
    }

    public void onPointerExit()
    {
        btnColor.a = 0.5f;
        image.color = btnColor;
        //크기 감소
        rectTransform.sizeDelta = new Vector2(100, 100);
        this.anim.SetInteger("State", 0);
        this.bg.SetActive(false);
    }
    public void onPointerEnterUnActive()
    {
        this.anim.SetInteger("State", 1);
    }

}
