using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float speed = 5f;
    [SerializeField] private float jump = 5f;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && rb.linearVelocity.y == 0)
        {
            Debug.Log("Jump!");
            rb.AddForce(Vector3.up * jump, ForceMode.Impulse);
        }
        
    }
}
