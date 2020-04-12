using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    private Objective objectiveScript;
    private bool ringActive = false;

    private void Start()
    {
        objectiveScript = FindObjectOfType<Objective>();
    }

    public void ActivateRing()
    {
        ringActive = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"OnTriggerEnter {this.name}");
        if (ringActive)
        {
            objectiveScript.NextRing();
        }
    }
}
