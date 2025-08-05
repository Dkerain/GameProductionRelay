using UnityEngine;
using System.Collections.Generic;

public class WhackSpawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnableCharacter
    {
        public string name;
        public GameObject prefab;
        [Range(0f, 1f)] public float spawnChance = 0.5f;
    }

    [Header("可生成角色")] public List<SpawnableCharacter> characters;
    [Header("生成位置")] public Transform[] spawnPoints;
    [Header("生成间隔")] public float spawnInterval = 1.5f; // 每隔多久尝试生成一次
    [Header("生成角色存活时间")] public float characterLifetime = 1.0f; // 生成出来后存在多久

    private float timer;

    void Start()
    {
        SetHoles();
    }

    private void SetHoles()
    {
        // 设置Mask的排序层级,遮挡角色
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            SpriteMask mask = spawnPoints[i].GetComponentInChildren<SpriteMask>();
            if (mask)
            {
                mask.frontSortingOrder = 9+10*i;
                mask.backSortingOrder = -1+10*i;
            }
        }
    }

    void Update()
    {
        if (!GM.Instance) return;
        if (!GM.Instance.enabled) return;

        if (GM.Instance && !IsGameRunning()) return;

        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnCharacter();
            timer = 0f;
        }
    }

    void SpawnCharacter()
    {
        if (spawnPoints.Length == 0 || characters.Count == 0) return;
        int holeIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[holeIndex];

        foreach (var character in characters)
        {
            if (Random.value <= character.spawnChance)
            {
                // 生成角色
                GameObject go = Instantiate(character.prefab, spawnPoint.position, Quaternion.identity);
                SetOrder(go, holeIndex);

                // Destroy(go, characterLifetime); // 自动销毁
                break; // 每次只生成一个
            }
        }
    }

    private bool IsGameRunning()
    {
        return GM.Instance != null && GM.Instance.enabled && Time.timeScale > 0;
    }

    private void SetOrder(GameObject go, int holeIndex)
    {
        SpriteRenderer renderer = go.GetComponent<SpriteRenderer>();
        if (renderer)
        {
            renderer.sortingOrder = holeIndex * 10;
            go.GetComponent<LievPopup>().Popup();
        }
    }
}