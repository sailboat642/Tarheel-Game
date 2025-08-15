using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class CustomerController : MonoBehaviour
{
    string OrderPreference;
    BurgerShack shack;
    private string OrderName;
    private State _state = State.DEFAULT;

    private Rigidbody2D rb;

    private bool isMoving;
    public Animator animator;

    private Transform target;
    
    public float speed = 200f;
    public float nextWaypointDistance = 0.15f;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    

    public State GetState() {
        return _state;
    }
    

    public enum State {
        DEFAULT,
        WAITING,
        EATING
    }


    void Awake() 
    {
        GameObject controller = GameObject.FindGameObjectsWithTag("GameController")[0];
        shack = controller.GetComponent<BurgerShack>();
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
    }


    public void Sit(Transform location)
    {
        rb.position = (Vector2)location.position;
        _state = State.WAITING;
        animator.SetBool("IsSitting", true);
        animator.SetBool("IsMoving", false);
    }



    public string PlaceOrder()
    {
        if (OrderName != null) {
            return OrderName;
        }


        int n = 1;
        foreach (var pair in shack.Menu) {
            int chance = Random.Range(0, n);
            if (chance == 0) {
                OrderName = pair.Key;
            }
            n++;
        }

        return OrderName;
    }

    private IEnumerator EatFood()
    {
        yield return new WaitForSeconds(1f);
        animator.SetBool("IsEating", true);
        yield return new WaitForSeconds(12f);
        animator.SetBool("IsEating", false);
    }

    public void OnFoodRecieved()
    {
        _state = State.EATING;
        StartCoroutine(EatFood());
    }

    public void SetTargetForPath(Transform target)
    {
        this.target = target;
    }

    public void FollowObject()
    {
        InvokeRepeating("UpdatePath", 0, 0.5f);
    }

    public void StopMoving()
    {
        target = null;
        CancelInvoke("UpdatePath");
        path = null;
        rb.velocity = Vector2.zero;
        Debug.Log("Stop");
        animator.SetBool("IsMoving", false);
    }

    public void LeaveDiner()
    {
        Transform closest = null;
        float cur_distance = 9999.999f;
        foreach (Transform exit in shack.Exits) {
            float exit_distance = (exit.position - transform.position).magnitude;
            if (cur_distance > exit_distance) {
                closest = exit;
                cur_distance = exit_distance;
            }
            Debug.Log(cur_distance);
        }

        target = closest;
        animator.SetBool("IsSitting", false);
        animator.SetBool("IsMoving", true);
        animator.SetBool("IsEating", false);

        UpdatePath();

    }

    public void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Exit")){
            Destroy(this.gameObject);
        }
    }



    /********************************************************************
    ***************** PathFinding ***************************************
    ********************************************************************/

    void UpdatePath()
    {
        seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error) {
            path = p;
            currentWaypoint = 0;
        }
    }

    void FixedUpdate()
    {
        if (path == null)
            return;
        
        if (currentWaypoint >= path.vectorPath.Count) {
            reachedEndOfPath = true;
            return;
        } else {
            reachedEndOfPath = false;
        }

        float distanceToTarget =  ((Vector2)target.position - rb.position).magnitude;
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;

        if (distanceToTarget > 0.1) {
            rb.velocity = speed * direction;
        } else {
            rb.velocity = Vector2.zero;
        }

        

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance) {
            currentWaypoint++;
        }





        if (rb.velocity == Vector2.zero) {
            animator.SetBool("IsMoving", false);
        } else {
            animator.SetBool("IsMoving", true);
            animator.SetFloat("MoveX", rb.velocity.x);
            animator.SetFloat("MoveY", rb.velocity.y);
        }
    }

}
