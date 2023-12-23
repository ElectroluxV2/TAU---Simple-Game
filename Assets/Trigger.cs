using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    public GameObject WinUI;

    public static UnityEvent E_GameEnd = new();

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Instantiate(WinUI);
            
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            
            
            
            E_GameEnd.Invoke();
        }
    }
    
    
}
