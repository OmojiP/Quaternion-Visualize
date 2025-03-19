using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    float speed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        if(Input.GetKey(KeyCode.W))
        {
            pos.y += speed*Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.S))
        {
            pos.y -= speed*Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            pos.x -= speed*Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.RightArrow))
        {
            pos.x += speed*Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.UpArrow))
        {
            pos.z += speed*Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.DownArrow))
        {
            pos.z -= speed*Time.deltaTime;
        }
        transform.position = pos;
    }
}
