using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.UI;

public class RotateObjs : MonoBehaviour
{
    [SerializeField] UVSpherePlacer uVSpherePlacer;

    private Func<Vector3, Vector3, float, Vector3> qFunc;

    [SerializeField] Button setQPButton;
    [SerializeField] Button setQcPButton;
    [SerializeField] Button setPQButton;
    [SerializeField] Button setPQcButton;
    [SerializeField] Button setQPQcButton;

    void Start()
    {
        setQPButton.onClick.AddListener(SetModeQP);
        setQcPButton.onClick.AddListener(SetModeQcP);
        setPQButton.onClick.AddListener(SetModePQ);
        setPQcButton.onClick.AddListener(SetModePQc);
        setQPQcButton.onClick.AddListener(SetModeQPQc);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            RotateObj(Vector3.right, destroyCancellationToken).Forget();
        }
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            RotateObj(Vector3.left, destroyCancellationToken).Forget();
        }
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            RotateObj(Vector3.up, destroyCancellationToken).Forget();
        }
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            RotateObj(Vector3.down, destroyCancellationToken).Forget();
        }
    }

    async UniTask RotateObj(Vector3 axis, CancellationToken token)
    {
        float angle = 0;

        Transform[] objs = uVSpherePlacer.Spheres.Select(x => x.transform).ToArray();
        Vector3[] objsInitialPos = new Vector3[objs.Length];

        for (int i = 0; i < objs.Length; i++)
        {
            objsInitialPos[i] = objs[i].position;
        }

        while(angle <= Mathf.PI * 2)
        {
            for (int i = 0; i < objs.Length; i++)
            {
                objs[i].position = qFunc.Invoke(objsInitialPos[i], axis, angle);
            }

            angle += Mathf.PI/12f;

            await UniTask.Delay(1000/24, cancellationToken:token);
        }
    }

    void SetModeQPQc()
    {
        qFunc = MyQuaternion.Rotate;
    }
    void SetModeQP()
    {
        qFunc = MyQuaternion.qp;
    }
    void SetModePQ()
    {
        qFunc = MyQuaternion.pq;
    }
    void SetModeQcP()
    {
        qFunc = MyQuaternion.qcp;
    }
    void SetModePQc()
    {
        qFunc = MyQuaternion.pqc;
    }
}
