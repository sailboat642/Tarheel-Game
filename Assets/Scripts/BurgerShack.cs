using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurgerShack : MonoBehaviour
{
    private GameObject player;
    public GameObject queueRef;
    private CustomerQueue customerQueue;
    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        customerQueue = queueRef.GetComponent<CustomerQueue>();
        PlayerController playerController = player.GetComponent<PlayerController>();
        Debug.Log("hello");
        StartCoroutine(CallCustomersWithInterval(5, 5.0f));
    }

    IEnumerator CallCustomersWithInterval(int n, float interval)
    {
        Debug.Log("hello");
        for (int i = 0; i < n; i++) {
            GameObject recent_customer = customerQueue.NewCustomerGroup();
            yield return new WaitForSeconds(interval);
            Debug.Log("New Customer");
        }
    }

}
