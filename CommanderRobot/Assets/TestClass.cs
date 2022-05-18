using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestClass : MonoBehaviour
{
    public void DoParticle(GameObject Particle)
    {
        GameObject p = Instantiate(Particle);
        p.transform.position = transform.position;
        if (p.TryGetComponent(out ParticleSystem ps))
        {
            ps.Play();
        }
    }
}
