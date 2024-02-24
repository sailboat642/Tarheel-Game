using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerController : MonoBehaviour
{
    // GameObject[] PathPoints;
    // private int pointsIndex;

    // [SerializeField] private float moveSpeed;
    // [SerializeField] private Transform target;
    // [SerializeField] private float separation = 0.5f;
    // private Vector3 offset = new Vector3(0f, 0f, -1.0f);
    
    void Awake() 
    {
        //PathPoints = GameObject.FindGameObjectsWithTag("CustomerPath");
        //Debug.Log(PathPoints.Length);
        //pointsIndex = 0;
    }

    public void Sit(Transform location)
    {
        Collider2D collider = GetComponent<Collider2D>();
        collider.enabled = false;
        transform.position = location.position;
    }
}
