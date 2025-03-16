using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System;
using UnityEngine.UI;

public class RotateObjs : MonoBehaviour
{
    [SerializeField] UVSphereWithLines sphereWithLines;
    [SerializeField] Slider thetaSlider;
    [SerializeField] Text thetaText;
    [SerializeField] Text speedText;
    [SerializeField] Slider speedSlider;

    private Func<MyQuaternion, Vector3, float, MyQuaternion> qFunc;

    private static readonly int numRotateStep = 50;

    [SerializeField] Button setQPButton;
    [SerializeField] Button setQcPButton;
    [SerializeField] Button setPQButton;
    [SerializeField] Button setPQcButton;
    [SerializeField] Button setQPQcButton;
    [SerializeField] Button step2Buttonqp2pqc;
    [SerializeField] Button step2Buttonpqc2qp;
    [SerializeField] Text modeText;
    [SerializeField] Text arrowText;

    private MyQuaternion[] myQhistory;
    private Transform[] objs;

    void Start()
    {
        sphereWithLines.StartSphere();
        objs = sphereWithLines.points.ToArray();
        myQhistory = new MyQuaternion[objs.Length];
        for (int i = 0; i < objs.Length; i++)
        {
            myQhistory[i] = new MyQuaternion(0, objs[i].position);
        }

        setQPButton.onClick.AddListener(SetModeQP);
        setQcPButton.onClick.AddListener(SetModeQcP);
        setPQButton.onClick.AddListener(SetModePQ);
        setPQcButton.onClick.AddListener(SetModePQc);
        setQPQcButton.onClick.AddListener(SetModeQPQc);
        thetaSlider.onValueChanged.AddListener((x) => 
        {
            thetaSlider.SetValueWithoutNotify(RoundToNearest(x, 15));
            thetaText.text = $"{thetaSlider.value.ToString("F0")}°";
        });
        speedSlider.onValueChanged.AddListener((x) => 
        {
            speedText.text = $"{x.ToString("F2")}[s]";
        });

        step2Buttonqp2pqc.onClick.AddListener( async () =>
        {
            SetModeQP();
            await RotateObj(Vector3.up, destroyCancellationToken);
            await UniTask.Delay(200, cancellationToken: destroyCancellationToken);
            SetModePQc();
            await UniTask.Delay(800, cancellationToken: destroyCancellationToken);
            await RotateObj(Vector3.up, destroyCancellationToken);
        });
        step2Buttonpqc2qp.onClick.AddListener( async () =>
        {
            SetModePQc();
            await RotateObj(Vector3.up, destroyCancellationToken);
            await UniTask.Delay(200, cancellationToken: destroyCancellationToken);
            SetModeQP();
            await UniTask.Delay(800, cancellationToken: destroyCancellationToken);
            await RotateObj(Vector3.up, destroyCancellationToken);
        });

        SetModeQPQc();

        thetaSlider.value = 90;
        speedSlider.value = 1;
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
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
    }

    bool isRotating = false;

    async UniTask RotateObj(Vector3 axis, CancellationToken token)
    {
        if(isRotating) return;
        isRotating = true;

        arrowText.text = (axis == Vector3.up) ? "←" : (axis == Vector3.down) ? "→" : (axis == Vector3.left) ? "↓" : (axis == Vector3.right) ? "↑" : "?";

        float angle = 0;
        float rotateAngle = thetaSlider.value * Mathf.Deg2Rad;

        for (int r = 0; r <= numRotateStep; r++)
        {
            angle = rotateAngle * r / numRotateStep;

            for (int i = 0; i < objs.Length; i++)
            {
                var q = qFunc.Invoke(myQhistory[i], axis, angle);
                objs[i].position = q.Vector;
                
                if(numRotateStep == r)
                {
                    myQhistory[i] = q;
                }
            }

            await UniTask.Delay((int)(speedSlider.value*1000)/numRotateStep, cancellationToken:token);
        }
        isRotating = false;
    }

    void SetModeQPQc()
    {
        qFunc = MyQuaternion.qpqc;
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

    float RoundToNearest(float value, float step)
    {
        return Mathf.Round(value / step) * step;
    }
}
