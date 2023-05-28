using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Particle : MonoBehaviour
{
    private void Start()
    {
        Invoke("destroyParticle", 2f);
    }
    private void destroyParticle()
    {
        Destroy(this.gameObject);
    }
}