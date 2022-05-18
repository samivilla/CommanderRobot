using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoreCreator : MonoBehaviour
{
    public enum GoreOrigin
    {
        larm,
        rarm,
        lleg,
        rleg,
        head
    }
    public float Force = 30;
    public float GoreDuration = 2;
    public GameObject Torso;
    public GameObject LArm;
    public GameObject RArm;
    public GameObject LLeg;
    public GameObject RLeg;
    public GameObject Head;
    public void MakeGore(GoreOrigin origin, GameObject prefab)
    {
        GameObject gore = Instantiate(prefab);
        GameObject parent = Torso;
        switch (origin)
        {
            case GoreOrigin.head:
                parent = Head;
                break;
            case GoreOrigin.larm:
                parent = LArm;
                break;
            case GoreOrigin.rarm:
                parent = RArm;
                break;
            case GoreOrigin.lleg:
                parent = LLeg;
                break;
            case GoreOrigin.rleg:
                parent = RLeg;
                break;
        }

        gore.transform.position = parent.transform.position;
        gore.transform.rotation = parent.transform.rotation;
        Destroy(gore, GoreDuration);
        if (gore.TryGetComponent(out Rigidbody2D body))
        {
            body.AddForceAtPosition ((gore.transform.position - Torso.transform.position).normalized * Random.Range(Force, Force + 20), Torso.transform.position - gore.transform.position );
        }
    }
    public void MakeLarmGore(GameObject prefab)
    {
        MakeGore(GoreOrigin.larm, prefab);
    }
    public void MakeRarmGore(GameObject prefab)
    {
        MakeGore(GoreOrigin.rarm, prefab);
    }
    public void MakeRlegGore(GameObject prefab)
    {
        MakeGore(GoreOrigin.rleg, prefab);
    }
    public void MakeLlegGore(GameObject prefab)
    {
        MakeGore(GoreOrigin.lleg, prefab);
    }
    public void MakeHeadGore(GameObject prefab)
    {
        MakeGore(GoreOrigin.head, prefab);
    }
}
