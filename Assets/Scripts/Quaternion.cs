using Unity.VisualScripting;
using UnityEditor.AnimatedValues;
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

    public Vector2 xy => new Vector2(x, y);
    public Vector2 yz => new Vector2(y, z);
    public Vector2 zx => new Vector2(z, x);
    public Vector2 wx => new Vector2(w, x);
    public Vector2 wy => new Vector2(w, y);
    public Vector2 wz => new Vector2(w, z);

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
        => new Quaternion(
            w: a.w *b.w - Dot(a.Vector, b.Vector),
            x: Dot(a.wx, b.wx) + Cross(a.yz, b.yz),
            y: Dot(a.wy, b.wy) + Cross(a.zx, b.zx),
            z: Dot(a.wz, b.wz) + Cross(a.xy, b.xy)
        );
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
        var q = Unit(rotAxis, theta);
        var q_c =  q.Conjugate;
        return (q * q_p * q_c).Vector;
    }

    public static float Dot(Vector2 a, Vector2 b)
    {
        return Vector2.Dot(a, b);
    }
    public static float Cross(Vector2 a, Vector2 b)
    {
        return a.x * b.y - a.y * b.x;
    }

}