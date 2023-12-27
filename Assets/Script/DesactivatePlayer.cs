using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesactivatePlayer : MonoBehaviour
{
    public GameObject playerPanel;
    
    void Start()
    {
        playerPanel.SetActive(false);
    }
}
