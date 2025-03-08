using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RotateObj : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            this.transform.position = Quaternion.Rotate(transform.position, Vector3.up, Mathf.PI/2f);
        }
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            this.transform.position = Quaternion.Rotate(transform.position, Vector3.down, Mathf.PI/2f);
        }
        if(Input.GetKeyDown(KeyCode.LeftAlt))
        {
            this.transform.position = Quaternion.Rotate(transform.position, Vector3.left, Mathf.PI/2f);
        }
        if(Input.GetKeyDown(KeyCode.RightAlt))
        {
            this.transform.position = Quaternion.Rotate(transform.position, Vector3.right, Mathf.PI/2f);
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
