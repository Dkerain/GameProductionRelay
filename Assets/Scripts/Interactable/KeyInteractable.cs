using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyInteractable : BaseInteractable
{
    public int keyNo;
    
    protected override void OnInteract()
    {
        if (GameObject.Find("KeySystem").GetComponent<KeySystem>().key[keyNo])
        {
            GetComponent<GetKey>().Getkey();
            Destroy(gameObject); // 立即销毁
        }
    }
}