using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class beh : MonoBehaviour
{
    public float speed = 10f;
    Rigidbody rigidbody;
    Vector3 movement;
    void Awake()
    {
        // 최초로 로딩될 때 딱 한번 실행
        rigidbody = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Run(h, v);
        // rigidbody and 
    }
    void OnDestory()
    {
        // 오브젝트삭제나 씬이바뀔때 실행
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Run(float h , float v)
    {
        movement.Set(h, 0, v);
        movement = movement.normalized * speed * Time.deltaTime;

        rigidbody.MovePosition(transform.position + movement);
    }
}
