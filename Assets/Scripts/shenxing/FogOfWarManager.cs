using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FogOfWarManager : MonoBehaviour
{
    [SerializeField] private GameObject grid;
    [SerializeField] private Material darkAreaMaterial;

    private Dictionary<TilemapRenderer, Material> originalMaterials = new Dictionary<TilemapRenderer, Material>();

    private void Start()
    {
        GridDuplicator.Instance.DuplicateGrid();
    }

    public void TurnOffTheLight()
    {
        if (darkAreaMaterial == null)
        {
            Debug.LogError("Dark Area Material未找到，请确保已放置在Resources/Materials目录下。");
        }

        if (grid == null)
        {
            Debug.LogError("Grid对象未设置，请在Inspector中设置。");
            return;
        }

        // 遍历Grid下的所有一级子物体，获取TilemapRenderer组件
        foreach (Transform child in grid.transform)
        {
            TilemapRenderer tilemapRenderer = child.GetComponent<TilemapRenderer>();
            if (tilemapRenderer != null)
            {
                // 保存原始材质（如果尚未保存）
                if (!originalMaterials.ContainsKey(tilemapRenderer))
                {
                    originalMaterials[tilemapRenderer] = tilemapRenderer.material;
                }

                // 将Tilemap的Material设置为Dark Area Material
                tilemapRenderer.material = darkAreaMaterial;
            }
        }
    }

    public void TurnOnTheLight()
    {
        if (grid == null)
        {
            Debug.LogError("Grid对象未设置，请在Inspector中设置。");
            return;
        }

        // 恢复所有Tilemap的原始材质
        foreach (Transform child in grid.transform)
        {
            TilemapRenderer tilemapRenderer = child.GetComponent<TilemapRenderer>();
            if (tilemapRenderer != null && originalMaterials.ContainsKey(tilemapRenderer))
            {
                tilemapRenderer.material = originalMaterials[tilemapRenderer];
            }
        }

        Debug.Log("战争迷雾已消除，地图恢复明亮");
    }
}