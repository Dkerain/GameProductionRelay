using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockInteractable : BaseInteractable
{
    protected override void OnInteract()
    {
        GetComponent<UnlockThedoor>().Unlock();
        Destroy(gameObject); // 立即销毁
    }
}