using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Keisan : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var q = MyQuaternion.Unit(Vector3.up, Mathf.PI/2);
        var p = new MyQuaternion(0, 2, 3, 5);

        Debug.Log($"q = {MyQuaternionExtention.ToString(q)}");
        Debug.Log($"p = {MyQuaternionExtention.ToString(p)}");
        Debug.Log($"q p = {MyQuaternionExtention.ToString(q*p)}");
        Debug.Log($"p q = {MyQuaternionExtention.ToString(p*q)}");
        Debug.Log($"q* p = {MyQuaternionExtention.ToString(q.Conjugate*p)}");
        Debug.Log($"p q* = {MyQuaternionExtention.ToString(p * q.Conjugate)}");
        Debug.Log($"q p q = {MyQuaternionExtention.ToString(q* p * q)}");
        Debug.Log($"q* p q* = {MyQuaternionExtention.ToString(q.Conjugate* p * q.Conjugate)}");
        Debug.Log($"q p q* = {MyQuaternionExtention.ToString(q* p * q.Conjugate)}");

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
