using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lis : MonoBehaviour
{
    public GameObject PBRMouse;
    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.name == "LeftCube")
        {
            PBRMouse.gameObject.transform.Rotate(new Vector3(0f,270f,0f));
        }
        if (col.gameObject.name == "RightCube")
        {
            PBRMouse.gameObject.transform.Rotate(new Vector3(0f, 90f, 0f));
        }
        if (col.gameObject.name == "EndCube")
        {

        }
    }
    void OnTriggerStay(Collider col)
    {
        if(col.gameObject.name== "LeftCube")
        {
            Debug.Log("LeftCube stay");
        }
    }
    void OnTriggerExit(Collider col)
    {
        if(col.gameObject.name== "LeftCube")
        {
            Debug.Log("LeftCube Exit");
        }
    }


    
}
