using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Category("CustomAction")]
public class EnterHItGame : ActionTask
{
    protected override void OnExecute()
    {
        GM.Instance.EnterHitGame();
    }
}
