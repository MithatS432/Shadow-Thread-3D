using System.Collections.Generic;
using UnityEngine;

public class AIHeatmap : MonoBehaviour
{
    public static AIHeatmap Instance;

    public float cellSize = 2f;
    public int maxSamplesForRed = 20;

    Dictionary<Vector2Int, int> heatData = new Dictionary<Vector2Int, int>();

    void Awake()
    {
        Instance = this;
    }

    public void RegisterPosition(Vector3 position)
    {
        Vector2Int cell = WorldToCell(position);

        if (!heatData.ContainsKey(cell))
            heatData[cell] = 0;

        heatData[cell]++;
    }

    Vector2Int WorldToCell(Vector3 pos)
    {
        return new Vector2Int(
            Mathf.FloorToInt(pos.x / cellSize),
            Mathf.FloorToInt(pos.z / cellSize)
        );
    }

    void OnDrawGizmos()
    {
        if (heatData == null) return;

        foreach (var kvp in heatData)
        {
            float t = Mathf.Clamp01((float)kvp.Value / maxSamplesForRed);
            Gizmos.color = Color.Lerp(Color.blue, Color.red, t);

            Vector3 worldPos = new Vector3(
                kvp.Key.x * cellSize + cellSize / 2,
                0.05f,
                kvp.Key.y * cellSize + cellSize / 2
            );

            Gizmos.DrawCube(worldPos, Vector3.one * cellSize * 0.9f);
        }
    }
}
