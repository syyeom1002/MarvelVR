using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeGenerator : MonoBehaviour
{
    public AudioClip thunderAudio;
    private void Start()
    {
        StartCoroutine(CoMoveUp());
        StartCoroutine(CoSound());
    }

    // Update is called once per frame
    void Update()
    {

        this.transform.Rotate(Vector3.up * 500f * Time.deltaTime, Space.World);
    }

    private IEnumerator CoMoveUp()
    {
        while (true)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(-4.39f,9.18f,13.42f), 0.08f);
            yield return null;
        }
    }

    private IEnumerator CoSound()
    {
        yield return new WaitForSeconds(2);
        GetComponent<AudioSource>().PlayOneShot(thunderAudio);
    }
}
