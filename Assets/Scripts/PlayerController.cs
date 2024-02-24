using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject Main;
    private CustomInput input = null;
    private Animator animator;

    private bool isMoving;
    private Vector2 moveVector = Vector2.zero;
    public float moveSpeed = 10f;

    private Rigidbody2D rb;
    private PlayerStates _state = PlayerStates.DEFAULT;

    private GameObject SelectedInteractive;
    private GameObject[] Carrying = new GameObject[2];

    public enum PlayerStates {
        DEFAULT,
        GUIDING,
        SERVING,
        INTERACTING
    }

    private void Awake()
    {
        input = new CustomInput();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (moveVector != Vector2.zero) {
            animator.SetFloat("moveX", moveVector.x);
            animator.SetFloat("moveY", moveVector.y);
        }
        animator.SetBool("isMoving", isMoving);

        if (_state == PlayerStates.GUIDING) {
            GuideCustomer();
        }

    }

    void FixedUpdate()
    {
        rb.velocity = moveVector*moveSpeed;
    }

    private void OnEnable()
    {
        input.Enable();
        input.Player.Movement.performed += OnMovementPerformed;
        input.Player.Movement.canceled += OnMovementCancelled;
    }

    private void OnDisable()
    {
        input.Disable();
        input.Player.Movement.performed -= OnMovementPerformed;
        input.Player.Movement.canceled -= OnMovementCancelled;
    }

    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        moveVector = value.ReadValue<Vector2>();
        isMoving = true;
    }

    private void OnMovementCancelled(InputAction.CallbackContext value)
    {
        moveVector = Vector2.zero;
        isMoving = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Interactive")) {
            SelectedInteractive = other.gameObject;
            Interactable intObj =  other.GetComponent<Interactable>();
            intObj.ShowInteraction();
        }
        
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Interactive")) {
            SelectedInteractive = null;
            Interactable intObj =  other.GetComponent<Interactable>();
            intObj.HideInteraction();
        }
    }

    private void ChangeState (PlayerStates new_state)
    {
        _state = new_state;
        Debug.Log(new_state);
    }

    public void Interact()
    {
        switch (_state) {
            case PlayerStates.DEFAULT:
                RecieveNewCustomer();
                break;
            case PlayerStates.GUIDING:
                AssignCustomerToTable();
                break;
        }
    }


    /***********************************************************************
    ********************Interactions****************************************
    ************************************************************************/

    // ------------------ Customer -------------------------
    public void GuideCustomer()
    {
        Vector3 difference = transform.position - Carrying[0].transform.position;
        if (difference.magnitude > 0.2) {
            Vector3 direction = difference/difference.magnitude;
            Carrying[0].transform.position = Carrying[0].transform.position + (direction * moveSpeed * 0.6f * Time.deltaTime);
        }
    }

    
    // ------------------   Table  -------------------------
    
    public void AssignCustomerToTable()
    {
        if (SelectedInteractive != null && _state == PlayerStates.GUIDING) {
            Table table = SelectedInteractive.GetComponent<Table>();
            if (table.isFree()) {
                table.SeatCustomer(Carrying[0].GetComponent<CustomerController>(), true);
                Carrying[0] = null;
                ChangeState(PlayerStates.DEFAULT);
            }
        }
    }

    // ------------------  Checkin Counter ------------------

    public void RecieveNewCustomer()
    {
        if (SelectedInteractive != null && _state == PlayerStates.DEFAULT) {
            CustomerQueue checkoutCounter = SelectedInteractive.GetComponent<CustomerQueue>();
            Carrying[0] = checkoutCounter.GetNextCustomerGroup();
            if (Carrying[0] != null) {
                ChangeState(PlayerStates.GUIDING);
            }
        }
    }


    // ------------------ Order Counter ---------------------
}
