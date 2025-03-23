using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class MySlerp : MonoBehaviour
{
    [SerializeField] bool isSlerp;

    // Start is called before the first frame update
    void Start()
    {
        sl = isSlerp ? Slerp : Lerp;
        SLStart(destroyCancellationToken).Forget();
    }

    Func<Quaternion, Quaternion, float, Quaternion> sl;

    async UniTask SLStart(CancellationToken token)
    {
        // Quaternion q0 = Quaternion.Euler(0, 0, 0);  // 開始回転
        // Quaternion q1 = Quaternion.Euler(0, 0, -180); // 終了回転
        Quaternion q0 = Quaternion.Euler(0, 0, 60);  // 開始回転
        Quaternion q1 = Quaternion.Euler(0, 0, 120); // 終了回転

        while(true)
        {
            await UniTask.Delay(500, cancellationToken: token);
            await Lerping(q0, q1, token);
        }
    }

    private async UniTask Lerping(Quaternion q0, Quaternion q1, CancellationToken token)
    {
        float t = 0f;
        while(true)
        {
            transform.rotation = sl.Invoke(q0, q1, t);

            if(t >= 1) break;

            t += 0.0125f;
            await UniTask.Delay(25, cancellationToken: token);
        }
    }


    Quaternion Slerp(Quaternion q0, Quaternion q1, float t)
    {
        float dot = Quaternion.Dot(q0, q1);
        
        // 内積が負なら符号反転（最短経路にする）
        if (dot < 0.0f) {
            q1 = new Quaternion(-q1.x, -q1.y, -q1.z, -q1.w);
            dot = -dot;
        }
        
        float theta = Mathf.Acos(dot);
        float sinTheta = Mathf.Sin(theta);

        float w0 = Mathf.Sin((1 - t) * theta) / sinTheta;
        float w1 = Mathf.Sin(t * theta) / sinTheta;

        return new Quaternion(
            w0 * q0.x + w1 * q1.x,
            w0 * q0.y + w1 * q1.y,
            w0 * q0.z + w1 * q1.z,
            w0 * q0.w + w1 * q1.w
        );
    }
    Quaternion Lerp(Quaternion q0, Quaternion q1, float t)
    {
        float dot = Quaternion.Dot(q0, q1);
        
        // 内積が負なら符号反転（最短経路にする）
        if (dot < 0.0f) {
            q1 = new Quaternion(-q1.x, -q1.y, -q1.z, -q1.w);
            dot = -dot;
        }
        
        float theta = Mathf.Acos(dot);
        float sinTheta = Mathf.Sin(theta);

        float w0 = (1 - t);
        float w1 = t;

        return new Quaternion(
            w0 * q0.x + w1 * q1.x,
            w0 * q0.y + w1 * q1.y,
            w0 * q0.z + w1 * q1.z,
            w0 * q0.w + w1 * q1.w
        ).normalized;
    }

}
