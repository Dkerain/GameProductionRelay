using System.Collections;
using System.Collections.Generic;
using NodeCanvas.Tasks.Actions;
using UnityEngine;

public class Transmit : MonoBehaviour
{
    private Transform TransmitTarget;

    // Start is called before the first frame update
    void Start()
    {
        TransmitTarget = transform.GetChild(0);
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject.FindWithTag("Player").transform.position = TransmitTarget.position;

    }

}
