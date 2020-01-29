using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class NewBehaviourScript : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        //vbButtonObject = GameObject.Find("actionButton");

        


        //vbButtonObject.GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);
        
    }

    public GameObject projectile;
    public float fireDelta = 0.5F;

    private float nextFire = 0.5F;
    private GameObject newProjectile;
    private float myTime = 0.0F;

    void Update()
    {
        myTime = myTime + Time.deltaTime;
            nextFire = myTime + fireDelta;
            newProjectile = Instantiate(projectile, transform.position, transform.rotation) as GameObject;

            // create code here that animates the newProjectile

            nextFire = nextFire - myTime;
            myTime = 0.0F;
    }
}
