using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// When particle system stops playing, destroy object
/// </summary>
public class SelfDestructOnParticleStop : MonoBehaviour
{
    ParticleSystem particleSystem;
    private void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if(!particleSystem.isPlaying)
        {
            Destroy(this.gameObject);
            return;
        }
        return;
    }
}
