using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order
{
    private string _name;
    public string Name => _name;
    
    private float _cookTime;
    public float CookTime => _cookTime;

    private float _price;
    public float Price => _price;

    // public Image picture;

    public Order(string name, float price, float cookTime) 
    {
        _name = name;
        _cookTime = cookTime;
        _price = price;
    }
    
}
