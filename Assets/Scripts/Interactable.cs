using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    protected SpriteRenderer sr;
    [SerializeField] protected GameObject Interactions;
    protected BurgerShack shack;

    public void Awake()
    {
        GameObject controller = GameObject.FindGameObjectsWithTag("GameController")[0];
        shack = controller.GetComponent<BurgerShack>();
        sr = GetComponent<SpriteRenderer>();
        sr.color = new Color(255, 255, 255, 0.8f);
    }
    public virtual void ShowInteraction()
    {
        Interactions.SetActive(true);
        sr.color = new Color(255, 255, 255, 1f);
    }
    
    public void HideInteraction()
    {
        Interactions.SetActive(false);
        sr.color = new Color(255, 255, 255, 0.8f);
    }

    public void DoAction() {
        return;
    }
}
