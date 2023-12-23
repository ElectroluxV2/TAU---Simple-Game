using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinGameUI : MonoBehaviour
{

    public void Quit()
    {
        #if UNITY_STANDALONE
        Application.Quit();
        #endif
    }
}
