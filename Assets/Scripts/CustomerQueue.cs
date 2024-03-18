using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerQueue : Interactable
{
    [SerializeField] private GameObject[] CustomerPrefabs;
    
    private Queue<GameObject> _queue = new Queue<GameObject>();

    public override void ShowInteraction()
    {
        Interactions.SetActive(true);
        sr.color = new Color(255, 255, 255, 1f);
        Debug.Log("Hi");
        GameObject nextCustomer = _queue.Peek();
        Debug.Log(nextCustomer.name);
        if (nextCustomer) {
            Debug.Log(nextCustomer.GetComponent<CustomerController>().PlaceOrder());
        }

    }

    public GameObject NewCustomerGroup()
    {
    
        GameObject new_customer = Instantiate(CustomerPrefabs[0], new Vector3(0.23f, -0.6f, 0f), Quaternion.identity);
        _queue.Enqueue(new_customer);
        return new_customer;
    
    }

    public GameObject GetNextCustomerGroup()
    {
        GameObject result;

        if (_queue.TryDequeue(out result)) {
            return result;
        } else {
            Debug.Log("No more Customers in Queue");
            return null;
        }
    }

    public void OnAddOrder()
    {
        string orderName = "";
        GameObject nextCustomer = _queue.Peek();
        if (nextCustomer) {
            orderName = nextCustomer.GetComponent<CustomerController>().PlaceOrder();
        }

        if (shack.Menu.ContainsKey(orderName)) {
            shack.order_queue.Enqueue(orderName);
        }
        // get next customers order
        // add to queue
    }

    public void DoAction()
    {
        // display interaction menu
    }

    public void AddOrder(string name)
    {
        // add order to queue
    }

}
