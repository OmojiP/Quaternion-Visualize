using System.Collections.Generic;
using UnityEngine;

public class UVSpherePlacer : MonoBehaviour
{
    [SerializeField] private GameObject objectPrefab;
    [SerializeField] private int longitudeCount = 10; // 経度の分割数
    [SerializeField] private int latitudeCount = 10;  // 緯度の分割数
    [SerializeField] private float sphereRadius = 5f; // 球の半径

    public GameObject[] Spheres {get; private set;}

    void Start()
    {
        if (objectPrefab == null)
        {
            Debug.LogError("プレハブが設定されていません");
            return;
        }

        Spheres = GenerateSphere();
    }

    GameObject[] GenerateSphere()
    {
        List<GameObject> spheres = new();

        for (int lat = 0; lat <= latitudeCount; lat++)
        {
            float theta = Mathf.PI * lat / latitudeCount; // 0 ~ π (北極から南極)
            float y = Mathf.Cos(theta);                   // Y座標
            float radiusAtLat = Mathf.Sin(theta);         // 現在の緯度での半径

            for (int lon = 0; lon < longitudeCount; lon++)
            {
                float phi = 2 * Mathf.PI * lon / longitudeCount; // 0 ~ 2π
                float x = radiusAtLat * Mathf.Cos(phi);
                float z = radiusAtLat * Mathf.Sin(phi);

                Vector3 position = new Vector3(x, y, z) * sphereRadius;
                var g = Instantiate(objectPrefab, position, Quaternion.identity, transform);
                spheres.Add(g);
            }
        }

        return spheres.ToArray();
    }
}
