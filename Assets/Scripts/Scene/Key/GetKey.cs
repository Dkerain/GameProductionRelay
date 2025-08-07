using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetKey : MonoBehaviour
{
    public int keyNo;
    
    public void Getkey()
    {
        GameObject.Find("KeySystem").GetComponent<KeySystem>().key[keyNo]= true;
    }
}
