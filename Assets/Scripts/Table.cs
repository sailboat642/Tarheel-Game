using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : Interactable
{
    public static List<Table> ActiveTables = new List<Table>();
    private CustomerController[] Customers = new CustomerController[2];
    [SerializeField] private Transform[] SeatPos;
    private bool _isFree = true;

    private State _state = State.DEFAULT;

    public enum State {
        DEFAULT,
        DIRTY,
        SERVING
    }

    void OnEnable() 
    {
        ActiveTables.Add(this);
    }

    void OnDisable()
    {
        ActiveTables.Remove(this);
    }

    public static bool HasOpenTable()
    {
        foreach(Table cur in ActiveTables) {
            if (cur.isFree()) return true;
        }

        return false;
    }


    public bool isFree() {
        return _isFree;
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

        _state = State.SERVING;
    }

    public void ClearTable()
    {
        _state = State.DIRTY;
    }
}
