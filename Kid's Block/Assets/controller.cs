using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controller : MonoBehaviour
{
    GameObject gameObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.W))
        {
            this.transform.Translate(Vector3.forward * 0.1f * Time.deltaTime);
        }
        else if(Input.GetKey(KeyCode.A))
        {
            this.transform.Translate(Vector3.left * 0.1f * Time.deltaTime);
        }
        else if(Input.GetKey(KeyCode.S))
        {
            this.transform.Translate(Vector3.back * 0.1f * Time.deltaTime);
        }
        else if(Input.GetKey(KeyCode.D))
        {
            this.transform.Translate(Vector3.right * 0.1f * Time.deltaTime);
        }
        this.transform.Translate(Vector3.forward * 1.0f * Time.deltaTime);
    }
}
