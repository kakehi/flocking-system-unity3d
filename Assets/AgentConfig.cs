using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentConfig : MonoBehaviour
{
    public float Rc;
    public float Rs;
    public float Ra;

    public float Kc;
    public float Ks;
    public float Ka;

    public float maxA;
    public float maxV;

    void Start()
    {
        maxA = 10;
        maxV = 10;
    }
}   
