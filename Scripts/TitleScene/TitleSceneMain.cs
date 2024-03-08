using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneMain : MonoBehaviour
{
    // Update is called once per frame
    public AudioClip audioClip;
    private bool isPlay;
    void Update()
    {
        if(OVRInput.GetDown(OVRInput.Button.Any)|| OVRInput.Get(OVRInput.Axis1D.Any) > 0)
        {
            StartCoroutine(this.CoSceneChange());
            
            if (!isPlay)
            {
                GetComponent<AudioSource>().PlayOneShot(audioClip);
                this.isPlay = true;
            }
        }         
    }

    private IEnumerator CoSceneChange()
    {
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene("SelectCharacter");
    }
}
