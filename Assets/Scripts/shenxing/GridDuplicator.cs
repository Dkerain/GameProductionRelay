using System;
using UnityEngine;
using UnityEngine.Tilemaps;


public class GridDuplicator : MonoBehaviour
{
    [Header("新Tilemap的Material")]
    public Material newTilemapMaterial;
    [Header("新Tilemap的Sorting Layer名称")]
    public string newSortingLayerName = "Default";
    
    // 单例模式
    public static GridDuplicator Instance;

    private void Awake()
    {
        Instance = this;
    }

    [ContextMenu("复制当前Grid并修改子物体排序")]
    public void DuplicateGrid()
    {
        // 1. 复制当前GameObject
        GameObject clone = Instantiate(gameObject, transform.parent);
        
        // 2. 改名字方便区分
        clone.name = gameObject.name + "_Copy";

        // 3. 遍历一级子物体
        foreach (Transform child in clone.transform)
        {
            TilemapRenderer tilemapRenderer = child.GetComponent<TilemapRenderer>();
            if (tilemapRenderer != null)
            {
                // 4. 修改 Tilemap 的 Material
                if (newTilemapMaterial != null)
                {
                    tilemapRenderer.material = newTilemapMaterial;
                }
                // 修改 Sorting Layer
                tilemapRenderer.sortingLayerName = newSortingLayerName;
            }
        }

        Debug.Log($"已复制 {gameObject.name} 并修改子物体的 SortingLayer 为 {newSortingLayerName}");
    }
}