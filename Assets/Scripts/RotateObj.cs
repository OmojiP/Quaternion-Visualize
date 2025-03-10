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
            this.transform.position = MyQuaternion.Rotate(transform.position, Vector3.right, Mathf.PI/12f);
        }
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            this.transform.position = MyQuaternion.Rotate(transform.position, Vector3.right, -Mathf.PI/12f);
        }
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            this.transform.position = MyQuaternion.Rotate(transform.position, Vector3.up, Mathf.PI/12f);
        }
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            this.transform.position = MyQuaternion.Rotate(transform.position, Vector3.up, -Mathf.PI/12f);
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
