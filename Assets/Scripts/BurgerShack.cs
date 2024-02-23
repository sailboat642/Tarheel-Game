using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurgerShack : MonoBehaviour
{
    private GameObject player;
    public GameObject queueRef;
    private CustomerQueue customerQueue;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        customerQueue = queueRef.GetComponent<CustomerQueue>();
        player.GetComponent<PlayerController>().customer = customerQueue.NextCustomer();

    }

}
