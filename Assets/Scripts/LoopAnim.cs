using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LoopAnim : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        GetComponent<Animation>().wrapMode = WrapMode.Loop;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
