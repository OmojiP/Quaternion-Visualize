using System.Collections.Generic;
using UnityEngine;

// GameObjectのペアを扱うクラス
[System.Serializable]
public class GameObjectPair
{
    public GameObject object1;
    public GameObject object2;
}

public class LineConnector : MonoBehaviour
{
    [SerializeField] private List<GameObjectPair> objectPairs = new(); // Inspectorで設定可能
    private List<LineRenderer> lineRenderers = new();
    [SerializeField] private Material lineMaterial;
    [SerializeField] private float lineWidth = 0.05f;

    void Start()
    {
        foreach (var pair in objectPairs)
        {
            CreateLine(pair.object1, pair.object2);
        }
    }

    void Update()
    {
        for (int i = 0; i < objectPairs.Count; i++)
        {
            UpdateLine(i, objectPairs[i].object1?.transform.position, objectPairs[i].object2?.transform.position);
        }
    }

    // LineRendererを作成
    private void CreateLine(GameObject obj1, GameObject obj2)
    {
        if (obj1 == null || obj2 == null) return;

        GameObject lineObj = new GameObject("Line");
        LineRenderer lr = lineObj.AddComponent<LineRenderer>();

        lr.material = lineMaterial ?? new Material(Shader.Find("Sprites/Default"));
        lr.startWidth = lr.endWidth = lineWidth;
        lr.positionCount = 2;
        lr.useWorldSpace = true;
        lr.startColor = Color.white;
        lr.endColor = Color.white;

        lineRenderers.Add(lr);
    }

    // 線の位置を更新
    private void UpdateLine(int index, Vector3? pos1, Vector3? pos2)
    {
        if (index < lineRenderers.Count && pos1.HasValue && pos2.HasValue)
        {
            lineRenderers[index].SetPosition(0, pos1.Value);
            lineRenderers[index].SetPosition(1, pos2.Value);
        }
    }

    // 外部からリストを更新できるようにする
    public void SetObjectPairs(List<GameObjectPair> newPairs)
    {
        objectPairs = newPairs;
        foreach (var lr in lineRenderers)
        {
            Destroy(lr.gameObject);
        }
        lineRenderers.Clear();
        
        foreach (var pair in objectPairs)
        {
            CreateLine(pair.object1, pair.object2);
        }
    }
}
