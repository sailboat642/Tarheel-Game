using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerController : MonoBehaviour
{
    GameObject[] PathPoints;
    private int pointsIndex;

    [SerializeField] private float moveSpeed;
    [SerializeField] private Transform target;
    [SerializeField] private float separation = 0.5f;
    private Vector3 offset = new Vector3(0f, 0f, -1.0f);
    
    void Awake() 
    {
        //PathPoints = GameObject.FindGameObjectsWithTag("CustomerPath");
        //Debug.Log(PathPoints.Length);
        //pointsIndex = 0;
    }

    void Update()
    {
        Vector3 difference = target.transform.position - transform.position;
        if (difference.magnitude > separation) {
            Vector3 direction = difference/difference.magnitude;
            transform.position = transform.position + (direction * moveSpeed * Time.deltaTime);
        } 
        
    }

    // void FollowPlayer()
    // {
    //     // Limitations
    //     // 2 unit gap: distance

    // }

}
