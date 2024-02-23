using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    private SpriteRenderer sr;

    public void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.color = new Color(255, 255, 255, 0.8f);
    }
    public void ShowInteraction()
    {
        Debug.Log("Interacting with " + gameObject.name);
        sr.color = new Color(255, 255, 255, 1f);
    }
    
    public void HideInteraction()
    {
        Debug.Log("Disconnecting with " + gameObject.name);
        sr.color = new Color(255, 255, 255, 0.8f);
    }

    public void DoAction() {
        return;
    }
}
