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
    string OrderPreference;
    BurgerShack shack;
    private string OrderName;
    private State _state = State.DEFAULT;

    private Rigidbody2D rb;

    private bool isMoving;
    private Vector2 moveVector;
    public float moveSpeed = 2.5f;

    private Animator animator;

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
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rb.velocity = moveVector*moveSpeed;
    }

    public void SetMovement(Vector2 moveVector, bool isMoving)
    {
        this.moveVector = moveVector;
        animator.SetBool("IsWalking", isMoving);
        if (isMoving) {
            animator.SetFloat("MoveX", moveVector.x);
            animator.SetFloat("MoveY", moveVector.y);
        }
    }

    public void Sit(Transform location)
    {
        Collider2D collider = GetComponent<Collider2D>();
        collider.enabled = false;
        transform.position = location.position;
        moveVector = Vector2.zero;
        _state = State.WAITING;
        animator.SetBool("IsSitting", true);
        animator.SetBool("IsWalking", false);
    }

    // private void CalculateMovement();

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
        yield return new WaitForSeconds(12f);
    }

    public void OnFoodRecieved()
    {
        _state = State.EATING;
        StartCoroutine(EatFood());
        // animation
        // Calculate Tip
        // Wait for other customer to finish eating
        // Leave Money
    }

}
