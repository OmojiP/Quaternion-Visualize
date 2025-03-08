using UnityEngine;

public class QuaternionRotationExample : MonoBehaviour
{
    public Vector3 initialPosition = new Vector3(1, 0, 0); // 初期座標
    public float angleDegrees = 90f; // 回転角度（度数法）
    public Vector3 rotationAxis = Vector3.up; // 回転軸

    void Start()
    {
        var q = (new MyQuaternion(1, 2, 3, 4) * new MyQuaternion(4, 3, 2, 1));
        Debug.Log($"{q.x}, {q.y}, {q.z}, {q.w}");
        var qu = (new Quaternion(2, 3, 4, 1) * new Quaternion(3, 2, 1, 4));
        Debug.Log($"{qu.x}, {qu.y}, {qu.z}, {qu.w}");


        UnityQ();
        MyQ();
    }

    void UnityQ()
    {
        // 角度をラジアンに変換
        float angleRadians = angleDegrees * Mathf.Deg2Rad;

        // クォータニオンの作成
        Quaternion q = Quaternion.AngleAxis(angleDegrees, rotationAxis);
        
        // 初期位置を純クォータニオンに変換 (w=0)
        Quaternion p = new Quaternion(0, initialPosition.x, initialPosition.y, initialPosition.z);

        // クォータニオンの回転計算: p' = q * p * q⁻¹
        Quaternion qConjugate = Quaternion.Inverse(q);
        Quaternion rotatedP = q * p * qConjugate;

        // 結果の座標を取得
        Vector3 newPosition = new Vector3(rotatedP.x, rotatedP.y, rotatedP.z);

        Debug.Log($"UnityQ 回転後の座標: {newPosition}");
    }
    
    void MyQ()
    {
        // 角度をラジアンに変換
        float angleRadians = angleDegrees * Mathf.Deg2Rad;

        // クォータニオンの作成
        MyQuaternion q = MyQuaternion.Unit(rotationAxis, angleRadians);
        
        // 初期位置を純クォータニオンに変換 (w=0)
        MyQuaternion p = new MyQuaternion(0, initialPosition);

        // クォータニオンの回転計算: p' = q * p * q⁻¹
        MyQuaternion rotatedP = q * p * q.Conjugate;

        // 結果の座標を取得
        Vector3 newPosition = rotatedP.Vector;

        Debug.Log($"MyQ 回転後の座標: {newPosition}");
    }
}