using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MissionUI : MonoBehaviour
{
    public Queue<Quaternion> parentRot;
    public Queue<Vector3> parentPos;
    public int followDelay;

    private Quaternion followRot;
    private Vector3 followPos;
    private Transform parent;
    public AudioClip audioClip;
    private void Awake()
    {
        this.parent = GameObject.Find("CenterEyeAnchor").transform;
        parentRot = new Queue<Quaternion>();
        parentPos = new Queue<Vector3>();
        GetComponent<AudioSource>().PlayOneShot(audioClip);
    }

    private void Update()
    {
        Watch();
        Follow();
    }

    void Watch()
    {
        //Queue FIFO
        if (!parentRot.Contains(parent.rotation))
        {
            parentRot.Enqueue(this.parent.rotation*Quaternion.Euler(new Vector3(10f,0,0)));
            parentPos.Enqueue(this.parent.position + this.parent.forward *1f);
        }

        if (parentRot.Count > followDelay)
        {
            this.followRot = parentRot.Dequeue();
            this.followPos = parentPos.Dequeue();
        }
        else if (parentRot.Count < followDelay)
        {
            this.followRot = this.parent.rotation * Quaternion.Euler(new Vector3(10f, 0, 0));
            this.followPos = this.parent.position+this.parent.forward*1f;
        }
    }

    void Follow()
    {
        this.transform.rotation = followRot;
        this.transform.position = followPos;
    }
}