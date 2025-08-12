using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 战争迷雾（Fog of War）使用说明：
/// 建议点开Fog of War Test Scene场景，这个场景尽量简单，主要是为了测试战争迷雾功能。
///
/// 场景层级说明：
/// FogOfWarManagern：作用是将瓦片地图复制一份，给予新的Material和Sorting layer，复制出来的充当照亮部分
/// Grid：存放了Tilemap，即场景的地图，默认可见，但是是暗的
/// 主角：身上挂载了一个视野的脚本FieldOfView.cs，里面可以设置视野半径、视野角度等参数。
/// AlwaysLightObjects：放置了永远亮的物体
/// OnlyVisualWhenBeSeen：放置了只有被视野看到时才会显示的物体
/// 
/// 关键是三个材质（已放在Assets/Materials目录下）
/// A: Dark Area Material材质默认是可见的，但是比较暗
/// B: OnlyVisualWhenBeSeenaterialM材质默认不可见，只有被C材质覆盖时才会显示，显示时比较亮
/// C: View Material材质自身是透明不可见的，唯一作用是使B材质显示出来
/// 这三种材质赋给对应的sprite renderer或者tilemap renderer上，在加上
/// 设置好对应的Sorting Layer和Order in Layer（参考FogOfWarTestScene场景），就可以生效了。
///
/// 如果想平时在Scene场景时地图能亮一点，可以把地图的tielmap renderer的Material改为Sprite-Default
/// 
/// </summary>

public class FogOfWarManager : MonoBehaviour
{
    private void Start()
    {
        GridDuplicator.Instance.DuplicateGrid();
    }
}
