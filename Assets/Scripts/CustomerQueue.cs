using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerQueue : Interactable
{
    public GameObject[] customers;
    // Start is called before the first frame update
    public GameObject NextCustomer()
    {
        GameObject new_customer = Instantiate(customers[0], new Vector3(0.23f, -0.6f, 0f), Quaternion.identity);
        return new_customer;
    }
}
