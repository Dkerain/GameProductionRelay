using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家相机出生点
/// </summary>
public class LevelController : MonoBehaviour
{
    private void Start()
    {
        var player = GameObject.FindWithTag("Player");
        var camera = GameObject.FindWithTag("MainCamera");
        // 找到场景中挂载了SpawnPoint MonoBehavior的物体
        var spawnPoint = GameObject.FindObjectOfType<SpawnPoint>();

        if (!player || !camera)
        {
            throw new Exception("场景中缺少玩家和相机");
        }

        if (spawnPoint)
        {
            var pos = (Vector2)spawnPoint.transform.position;
            player.transform.position = new Vector3(pos.x, pos.y, camera.transform.position.z);
            camera.transform.position = new Vector3(pos.x, pos.y, camera.transform.position.z);
        }
    }
}