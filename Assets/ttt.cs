using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ttt : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("Submit").active = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            GameObject.Find("Submit").active = true;
        }

    }
}