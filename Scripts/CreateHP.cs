using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CreateHP : MonoBehaviour
{
    [SerializeField] private Transform rightHandAnchor;
    [SerializeField] private GameObject playerHp;
    [SerializeField] private GameObject hpLight;
    void Update()
    {
        DrawArrow.ForDebug(this.rightHandAnchor.position, rightHandAnchor.right, 0, Color.red, ArrowType.Solid);
        DrawArrow.ForDebug(Vector3.zero, Vector3.up, 0, Color.green, ArrowType.Solid);

        var dot = Vector3.Dot(Vector3.up, this.rightHandAnchor.right);
        if (dot > 0.85f)
        {
            this.playerHp.SetActive(true);
            this.hpLight.SetActive(true);
        }
        else
        {
            this.playerHp.SetActive(false);
            this.hpLight.SetActive(false);
        }
    }
}
