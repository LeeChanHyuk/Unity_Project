using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class doit : MonoBehaviour
{
    public GameObject PBRMouse;
    public bool walktriggered = false;
    public bool backtriggered = false;
    public bool key = false;
    Animator anim;
    void Start()
    {
        anim = PBRMouse.GetComponent<Animator>();
    }

    
    // Update is called once per frame
    void Update()
    {
        if (walktriggered==true && backtriggered==true)
        {
            key = false;
        }
        else if(walktriggered==true)
        {
            key = true;
            if (PBRMouse.transform.eulerAngles.y == 0 || PBRMouse.transform.eulerAngles.y == 360)
            {
                PBRMouse.gameObject.transform.position -= new Vector3(1F, 0f, 0f);
            }
            else if (PBRMouse.transform.eulerAngles.y == 90)
            {
                PBRMouse.gameObject.transform.position -= new Vector3(0F, 0f, 1f);
            }
            else if (PBRMouse.transform.eulerAngles.y == 180)
            {
                PBRMouse.gameObject.transform.position += new Vector3(1F, 0f, 0f);
            }
            else if (PBRMouse.transform.eulerAngles.y == 270)
            {
                PBRMouse.gameObject.transform.position += new Vector3(0F, 0f, 1f);
            }
        }
        else if(backtriggered)
        {
            key = true;
            if (PBRMouse.transform.eulerAngles.y == 0 || PBRMouse.transform.eulerAngles.y == 360)
            {
                PBRMouse.gameObject.transform.position += new Vector3(1F, 0f, 0f);
            }
            else if (PBRMouse.transform.eulerAngles.y == 90)
            {
                PBRMouse.gameObject.transform.position += new Vector3(0F, 0f, 1f);
            }
            else if (PBRMouse.transform.eulerAngles.y == 180)
            {
                PBRMouse.gameObject.transform.position -= new Vector3(1F, 0f, 0f);
            }
            else if (PBRMouse.transform.eulerAngles.y == 270)
            {
                PBRMouse.gameObject.transform.position -= new Vector3(0F, 0f, 1f);
            }
        }
        else if(walktriggered==false && backtriggered==false)
        {
            key = false;
        }

        if (key==true)
            anim.SetBool("isRun", true);
        else
            anim.SetBool("isRun", false);
    }

    public void walking()
    {
        if (walktriggered == true)
        {
            walktriggered = false;
        }
        else
            walktriggered = true;
    }

    public void backing()
    {
        PBRMouse.gameObject.transform.Rotate(new Vector3(0f, 180f, 0f)); // 오른쪽
        if (backtriggered == true)
            backtriggered = false;
        else
            backtriggered = true;
    }
    public void reseting()
    {
        PBRMouse.gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
    }
}
