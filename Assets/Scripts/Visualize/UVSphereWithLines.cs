using UnityEngine;
using System.Collections.Generic;

public class UVSphereWithLines : MonoBehaviour
{
    [SerializeField] private int latitudeCount = 10;  // 緯度の分割数
    [SerializeField] private int longitudeCount = 10; // 経度の分割数
    [SerializeField] private float radius = 5f;       // 球の半径
    [SerializeField] private GameObject pointPrefab;  // 球上のポイントのPrefab
    [SerializeField] private Material lineMaterial;   // 線のマテリアル
    [SerializeField] private Material verMaterial0;
    [SerializeField] private Material verMaterial1;

    public List<Transform> points = new List<Transform>(); // 配置したポイントのリスト
    private List<(Transform, Transform)> pointPairs = new (); // 配置したポイントのリスト
    List<LineRenderer> lr = new ();

    public void StartSphere()
    {
        GenerateUVSphere();
        DrawLines();
    }

    void Update()
    {
        UpdateLines();
    }


    void GenerateUVSphere()
    {
        for (int lat = 0; lat <= latitudeCount; lat++)
        {
            float theta = Mathf.PI * lat / latitudeCount; // 緯度（0〜π）
            for (int lon = 0; lon < longitudeCount; lon++)
            {
                float phi = 2 * Mathf.PI * lon / longitudeCount; // 経度（0〜2π）

                // 球面座標をデカルト座標に変換
                Vector3 pos = new Vector3(
                    radius * Mathf.Sin(theta) * Mathf.Cos(phi),
                    radius * Mathf.Cos(theta),
                    radius * Mathf.Sin(theta) * Mathf.Sin(phi)
                );

                // 点を生成
                GameObject point = Instantiate(pointPrefab, pos, Quaternion.identity, transform);
                if(lon == 0)
                {
                    if(lat <= latitudeCount/2){
                        point.GetComponent<MeshRenderer>().material = verMaterial0;
                    }else{
                        point.GetComponent<MeshRenderer>().material = verMaterial1;
                    }
                }
                points.Add(point.transform);
            }
        }
    }

    void DrawLines()
    {
        // 緯線（横方向の線）
        for (int lat = 0; lat <= latitudeCount; lat++)
        {
            for (int lon = 0; lon < longitudeCount; lon++)
            {
                int currentIndex = lat * longitudeCount + lon;
                int nextIndex = lat * longitudeCount + (lon + 1) % longitudeCount; // 緯度方向の隣

                CreateLine(points[currentIndex].position, points[nextIndex].position);
                pointPairs.Add( (points[currentIndex], points[nextIndex]) );
            }
        }

        // 経線（縦方向の線）
        for (int lat = 0; lat < latitudeCount; lat++)
        {
            for (int lon = 0; lon < longitudeCount; lon++)
            {
                int currentIndex = lat * longitudeCount + lon;
                int nextIndex = (lat + 1) * longitudeCount + lon; // 経度方向の隣

                CreateLine(points[currentIndex].position, points[nextIndex].position);
                pointPairs.Add( (points[currentIndex], points[nextIndex]) );
            }
        }
    }

    void CreateLine(Vector3 start, Vector3 end)
    {
        GameObject lineObj = new GameObject("Line");
        lineObj.transform.parent = transform; // 親オブジェクトの子にする
        LineRenderer lineRenderer = lineObj.AddComponent<LineRenderer>();

        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.material = lineMaterial;
        lineRenderer.useWorldSpace = true; // ワールド座標で線を描く

        lr.Add(lineRenderer);

        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }

    void UpdateLines()
    {
        for (int i = 0; i < pointPairs.Count; i++)
        {
            lr[i].SetPosition(0, pointPairs[i].Item1.position);
            lr[i].SetPosition(1, pointPairs[i].Item2.position);
        }
    }
}
