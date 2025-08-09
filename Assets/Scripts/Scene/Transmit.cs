using System;
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
        DoTransmit(GameObject.FindWithTag("Player"));
    }

    public void DoTransmit(GameObject o)
    {
        o.transform.position = new Vector3(TransmitTarget.position.x, TransmitTarget.position.y, o.transform.position.z);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
        var transmitTarget = transform.GetChild(0);
        if (transmitTarget)
        {
            Gizmos.DrawWireSphere(transmitTarget.position, 0.5f);
            Gizmos.DrawLine(transform.position, transmitTarget.position);
        }
    }
}
