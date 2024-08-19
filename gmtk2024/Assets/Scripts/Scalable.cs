using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scalable : MonoBehaviour
{
    public float Scale;
    // Start is called before the first frame update
    void Start()
    {
        Scale = gameObject.transform.localScale.x;
    }

}
