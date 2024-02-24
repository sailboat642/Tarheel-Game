using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : Interactable
{
    private CustomerController[] Customers = new CustomerController[2];
    [SerializeField] private Transform[] SeatPos;
    private bool _isFree = true;

    public bool isFree() {
        return _isFree;
    }
    
    void Start()
    {
        Debug.Log(SeatPos.Length);
    }

    public void SeatCustomer(CustomerController customer, bool seatA)
    {

        if (customer == null) {
            Debug.Log("The customer does not exist");
            return;
        }

        _isFree = false;

        if (seatA) {
            Customers[0] = customer;
            customer.Sit(SeatPos[0]);
        } else {
            Customers[1] = customer;
            customer.Sit(SeatPos[1]);
        }
    }

    public void ClearTable()
    {
        return;
    }
}
