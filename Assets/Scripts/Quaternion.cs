using UnityEngine;

/// <summary>
/// w + xi + yj + zk
/// </summary>
public struct Quaternion
{
    float w;
    float x;
    float y;
    float z;

    /// <summary>
    /// x, y, z
    /// </summary>
    public Vector3 Vector => new Vector3(x, y, z);
    public float SqNorm => w*w + x*x + y*y + z*z;
    public float Norm => Mathf.Sqrt(SqNorm);
    public Quaternion Conjugate => new Quaternion(w, -Vector);
    public Quaternion Inverse => Conjugate / SqNorm;
    
    public static Quaternion operator *(float s, Quaternion q)
        => new Quaternion(q.w*s, q.Vector*s);
    public static Quaternion operator *(Quaternion q, float s)
        => s * q;
    public static Quaternion operator /(Quaternion q, float s)
        => q * (1 / s);
    public static Quaternion operator +(Quaternion a, Quaternion b)
        => new Quaternion(a.w + b.w , a.Vector + b.Vector);
    public static Quaternion operator -(Quaternion a, Quaternion b)
        => new Quaternion(a.w - b.w , a.Vector - b.Vector);

    public static Quaternion operator *(Quaternion a, Quaternion b)
    {
        Vector3 cross = Vector3.Cross(a.Vector, b.Vector);
        return new Quaternion(
            w: a.w * b.w - Vector3.Dot(a.Vector, b.Vector),
            x: a.w * b.x + b.w * a.x + cross.x,
            y: a.w * b.y + b.w * a.y + cross.y,
            z: a.w * b.z + b.w * a.z + cross.z
        );
    }
    public static Quaternion operator /(Quaternion a, Quaternion b)
        => a * b.Inverse;

    public Quaternion(float w, float x, float y, float z)
    {
        this.w = w;
        this.x = x;
        this.y = y;
        this.z = z;
    }

    
    public Quaternion(float w, Vector3 vector)
    {
        this.w = w;
        this.x = vector.x;
        this.y = vector.y;
        this.z = vector.z;
    }

    public static Quaternion Unit(Vector3 vector, float theta)
    {
        // cos + u sin
        var u = vector.normalized;
        return new Quaternion(Mathf.Cos(theta), u * Mathf.Sin(theta));
    }

    public static Vector3 Rotate(Vector3 p, Vector3 rotAxis, float theta)
    {
        var q_p = new Quaternion(0, p);
        var q = Unit(rotAxis, theta / 2f);
        var q_c =  q.Conjugate;
        return ((q * q_p) * q_c).Vector;
    }
}