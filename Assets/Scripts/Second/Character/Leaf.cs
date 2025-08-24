using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : Character
{
    public override void OnHit()
    {
        base.OnHit();
        Debug.Log("Leaf hit test");
    }
}
