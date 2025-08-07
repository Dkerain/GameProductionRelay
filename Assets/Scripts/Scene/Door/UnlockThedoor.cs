using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockThedoor : MonoBehaviour
{
    public GameObject[] Doors;
    // Start is called before the first frame update
    public void Unlock()
    {
        foreach (GameObject Door in Doors)
        {
            Door.GetComponent<Door>().Unlocked();
            Debug.Log("¿ªÆô" + Door);
        }

    }
}
