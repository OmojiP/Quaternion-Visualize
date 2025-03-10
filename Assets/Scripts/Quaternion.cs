using UnityEngine;

/// <summary>
/// w + xi + yj + zk
/// </summary>
public struct MyQuaternion
{
    public float w;
    public float x;
    public float y;
    public float z;

    /// <summary>
    /// x, y, z
    /// </summary>
    public Vector3 Vector => new Vector3(x, y, z);
    public float SqNorm => w*w + x*x + y*y + z*z;
    public float Norm => Mathf.Sqrt(SqNorm);
    public MyQuaternion Conjugate => new MyQuaternion(w, -Vector);
    public MyQuaternion Inverse => Conjugate / SqNorm;
    
    public static MyQuaternion operator *(float s, MyQuaternion q)
        => new MyQuaternion(q.w*s, q.Vector*s);
    public static MyQuaternion operator *(MyQuaternion q, float s)
        => s * q;
    public static MyQuaternion operator /(MyQuaternion q, float s)
        => q * (1 / s);
    public static MyQuaternion operator +(MyQuaternion a, MyQuaternion b)
        => new MyQuaternion(a.w + b.w , a.Vector + b.Vector);
    public static MyQuaternion operator -(MyQuaternion a, MyQuaternion b)
        => new MyQuaternion(a.w - b.w , a.Vector - b.Vector);

    public static MyQuaternion operator *(MyQuaternion a, MyQuaternion b)
    {
        Vector3 cross = Vector3.Cross(a.Vector, b.Vector);
        return new MyQuaternion(
            w: a.w * b.w - Vector3.Dot(a.Vector, b.Vector),
            x: a.w * b.x + b.w * a.x + cross.x,
            y: a.w * b.y + b.w * a.y + cross.y,
            z: a.w * b.z + b.w * a.z + cross.z
        );
    }
    public static MyQuaternion operator /(MyQuaternion a, MyQuaternion b)
        => a * b.Inverse;

    public MyQuaternion(float w, float x, float y, float z)
    {
        this.w = w;
        this.x = x;
        this.y = y;
        this.z = z;
    }

    
    public MyQuaternion(float w, Vector3 vector)
    {
        this.w = w;
        this.x = vector.x;
        this.y = vector.y;
        this.z = vector.z;
    }

    public static MyQuaternion Unit(Vector3 vector, float theta)
    {
        // cos + u sin
        var u = vector.normalized;
        return new MyQuaternion(Mathf.Cos(theta / 2f), u * Mathf.Sin(theta / 2f));
    }

    public static Vector3 Rotate(Vector3 p, Vector3 rotAxis, float theta)
    {
        var q_p = new MyQuaternion(0, p);
        var q = Unit(rotAxis, theta);
        var q_c =  q.Conjugate;
        return ((q * q_p) * q_c).Vector;
    }

    public static MyQuaternion qp(Vector3 p, Vector3 rotAxis, float theta)
    {
        var q_p = new MyQuaternion(0, p);
        var q = Unit(rotAxis, theta);
        var q_c =  q.Conjugate;
        return (q * q_p);
    }
    public static MyQuaternion pqc(Vector3 p, Vector3 rotAxis, float theta)
    {
        var q_p = new MyQuaternion(0, p);
        var q = Unit(rotAxis, theta);
        var q_c =  q.Conjugate;
        return (q_p * q_c);
    }
    public static MyQuaternion pq(Vector3 p, Vector3 rotAxis, float theta)
    {
        var q_p = new MyQuaternion(0, p);
        var q = Unit(rotAxis, theta);
        var q_c =  q.Conjugate;
        return (q_p * q);
    }
    public static MyQuaternion qcp(Vector3 p, Vector3 rotAxis, float theta)
    {
        var q_p = new MyQuaternion(0, p);
        var q = Unit(rotAxis, theta);
        var q_c =  q.Conjugate;
        return (q_c * q_p);
    }
}

public static class MyQuaternionExtention
{
    public static string ToString(MyQuaternion q){
        return $"w: {q.w}, x: {q.x}, y: {q.y}, z:{q.z}";
    }

    public static string ToString(Quaternion q){
        return $"w: {q.w}, x: {q.x}, y: {q.y}, z:{q.z}";
    }
}