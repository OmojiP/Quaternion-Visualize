using UnityEngine;

public class LookAt : MonoBehaviour
{
    [SerializeField] Transform target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // transform.LookAt(target, Vector3.up);
        MyLookAt(target.position);
    }

    void MyLookAt(Vector3 target)
    {
        Vector3 pos = transform.position;

        Vector3 r = (target - pos);

        float planeAngle = -Mathf.Atan2(r.z, r.x);

        var q1 = MyQuaternion.Unit(Vector3.up, planeAngle);

        float otherAngle = Mathf.Atan2(r.y, new Vector2(r.x, r.z).magnitude);
        Vector3 otherAxis = new Vector3(Mathf.Sin(otherAngle), 0, Mathf.Cos(otherAngle));

        var q2 = MyQuaternion.Unit(Vector3.forward, otherAngle);

        var q = q1 * q2;

        transform.rotation = new Quaternion(x: q.x, y: q.y, z: q.z, w:q.w);
    }
}
