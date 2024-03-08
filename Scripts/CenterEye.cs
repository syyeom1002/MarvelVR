using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterEye : MonoBehaviour
{
    ParticleSystem ps;
    List<ParticleSystem.Particle> inside = new List<ParticleSystem.Particle>();
    // Start is called before the first frame update
    void Start()
    {
        ps = GameObject.Find("GunLaserEffect").GetComponent<ParticleSystem>();
    }

    private void OnParticleTrigger()
    {
        ps.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, inside);
        foreach (var particle in inside)
        {
            Debug.Log("ÆÄÆ¼Å¯");

        }
    }
}
