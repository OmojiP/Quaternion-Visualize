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
    [SerializeField] UVSphereWithLines sphereWithLines;

    private Func<Vector3, Vector3, float, MyQuaternion> qFunc;

    private static readonly int numRotateStep = 50;
    private static readonly float rotateAngle = Mathf.PI;

    [SerializeField] Button setQPButton;
    [SerializeField] Button setQcPButton;
    [SerializeField] Button setPQButton;
    [SerializeField] Button setPQcButton;
    [SerializeField] Button setQPQcButton;
    [SerializeField] Button step2Buttonqp2pqc;
    [SerializeField] Button step2Buttonpqc2qp;
    [SerializeField] Text modeText;

    void Start()
    {
        setQPButton.onClick.AddListener(SetModeQP);
        setQcPButton.onClick.AddListener(SetModeQcP);
        setPQButton.onClick.AddListener(SetModePQ);
        setPQcButton.onClick.AddListener(SetModePQc);
        setQPQcButton.onClick.AddListener(SetModeQPQc);

        step2Buttonqp2pqc.onClick.AddListener( async () =>
        {
            SetModeQP();
            await RotateObj(Vector3.up, destroyCancellationToken);
            SetModePQc();
            await RotateObj(Vector3.up, destroyCancellationToken);
        });
        step2Buttonpqc2qp.onClick.AddListener( async () =>
        {
            SetModePQc();
            await RotateObj(Vector3.up, destroyCancellationToken);
            SetModeQP();
            await RotateObj(Vector3.up, destroyCancellationToken);
        });

        SetModeQPQc();
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

        Transform[] objs = sphereWithLines.points.ToArray();
        Vector3[] objsInitialPos = new Vector3[objs.Length];

        for (int i = 0; i < objs.Length; i++)
        {
            objsInitialPos[i] = objs[i].position;
        }

        for (int r = 0; r <= numRotateStep; r++)
        {
            angle = rotateAngle * r / numRotateStep;

            for (int i = 0; i < objs.Length; i++)
            {
                objs[i].position = qFunc.Invoke(objsInitialPos[i], axis, angle).Vector;
            }

            await UniTask.Delay(2000/numRotateStep, cancellationToken:token);
        }
    }

    void SetModeQPQc()
    {
        qFunc = (p, axis, angle) => 
        {
            return new MyQuaternion(0, MyQuaternion.Rotate(p, axis, angle));
        };
        modeText.text = "mode:\nQ P Q*";
    }
    void SetModeQP()
    {
        qFunc = MyQuaternion.qp;
        modeText.text = "mode:\nQ P";
    }
    void SetModePQ()
    {
        qFunc = MyQuaternion.pq;
        modeText.text = "mode:\nP Q";
    }
    void SetModeQcP()
    {
        qFunc = MyQuaternion.qcp;
        modeText.text = "mode:\nQ* P";
    }
    void SetModePQc()
    {
        qFunc = MyQuaternion.pqc;
        modeText.text = "mode:\nP Q*";
    }
}
