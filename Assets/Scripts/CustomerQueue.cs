using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerQueue : Interactable
{
    [SerializeField] private GameObject[] CustomerPrefabs;
    private Queue<GameObject> _queue = new Queue<GameObject>();
    // Start is called before the first frame update
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
            Debug.Log("Customer assigned succesfully");
            return result;
        } else {
            Debug.Log("No more Customers in Queue");
            return null;
        }
    }
}
