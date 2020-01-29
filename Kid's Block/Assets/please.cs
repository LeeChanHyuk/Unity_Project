using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class please : MonoBehaviour
{
    public GameObject PBRMouse;
    int a = 0;
    int b = 0;
    
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "RightCube" && a==0)
        {
            PBRMouse.gameObject.transform.Rotate(new Vector3(0f, 90f, 0f));
            a = 1;
            b = 0;
        }
        if (col.gameObject.name == "LeftCube" && b==0)
        {
            PBRMouse.gameObject.transform.Rotate(new Vector3(0f, 270f, 0f));
            b = 1;
            a = 0;
        }
        if(col.gameObject.name=="EndCube")
        {
        }
    }
    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.name == "RightCube")
        {
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.name == "RightCube")
        {
        }
    }

    void Start()
    {
        PBRMouse.gameObject.transform.localPosition = new Vector3(0F, 0f, 0f);
    }

    void Update()
    {
        if (PBRMouse.transform.eulerAngles.y<50)
        {
            PBRMouse.gameObject.transform.position += new Vector3(0F, 0f, 1f);
        }
        else if (PBRMouse.transform.eulerAngles.y>50 && PBRMouse.transform.eulerAngles.y<140)
        {
            PBRMouse.gameObject.transform.position += new Vector3(1F, 0f, 0f);
        }
        else if (PBRMouse.transform.eulerAngles.y>140 && PBRMouse.transform.eulerAngles.y<230)
        {
            PBRMouse.gameObject.transform.position -= new Vector3(0F, 0f, 1f);
        }
        else if (PBRMouse.transform.eulerAngles.y>230)
        {
            PBRMouse.gameObject.transform.position -= new Vector3(1F, 0f, 0f);
        }
    }
    public void reseting()
    {
        PBRMouse.gameObject.transform.localPosition = new Vector3(0F, 0f, 0f);
        PBRMouse.gameObject.transform.eulerAngles = new Vector3(0F, 0f, 0f);
    }
}