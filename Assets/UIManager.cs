using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject UI;
    
    void Update()
    {
        UI.SetActive(Input.GetKey(KeyCode.Tab));   
    }
}
