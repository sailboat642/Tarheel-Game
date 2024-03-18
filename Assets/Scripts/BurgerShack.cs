using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurgerShack : MonoBehaviour
{
    private GameObject player;
    private float tips;

    private float _totalSales;
    public Dictionary<string, Order> Menu;

    public GameObject queueRef;
    private CustomerQueue customerQueue;
    public Queue<string> order_queue = new Queue<string>();

    

    public int n_customers;
    public float interval;
    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        customerQueue = queueRef.GetComponent<CustomerQueue>();


        PlayerController playerController = player.GetComponent<PlayerController>();
        
        Main();
        
    }

    private void Main()
    {
        GenerateMenu();
        StartCoroutine(CallCustomersWithInterval(n_customers, interval));
    }

    private void GenerateMenu()
    {
        Menu = new Dictionary<string, Order>();
        Menu.Add("Kenny J", new Order("Kenny J", 5.00f, 12f));
        Menu.Add("Mookie", new Order("Mookie", 5.25f, 10f));
        Menu.Add("Paco", new Order("Paco", 6.00f, 15f));

    }

    IEnumerator CallCustomersWithInterval(int n, float interval)
    {
        for (int i = 0; i < n; i++) {
            GameObject recent_customer = customerQueue.NewCustomerGroup();
            yield return new WaitForSeconds(interval);
        }
    }

    /* ***********************************************************
    ****************** Stove code ********************************
    ************************************************************ */
    
    private bool stoveIsFree = true;

    void Update() 
    {
        if (stoveIsFree) {
            StartCoroutine(CookFood());
        }
    }


    IEnumerator CookFood() {

        stoveIsFree = false;

        string orderName = "";

        if (order_queue.TryDequeue(out orderName)) {
            Debug.Log(orderName);
            Order curOrder = Menu[orderName];
            Debug.LogFormat("Cooking {0} for {1} seconds", curOrder.Name, curOrder.CookTime);
            yield return new WaitForSeconds(curOrder.CookTime);
            Debug.Log("Food is cooked");
        }

        stoveIsFree = true;
    }
}
