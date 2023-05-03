using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Components
    Rigidbody _rb;
    Collider _collider;

    // Data
    [SerializeField] float _moveForce;
    [SerializeField] float _jumpHeight;
    [SerializeField] float _targetVelocity;

    // Movement
    Vector3 _moveDir;
    bool _isJumping;
    bool _pressedJump;
    
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckGround();
        PlayerInput();
    }

    void FixedUpdate()
    {
        PlayerMovement();

        if (_pressedJump)
        {
            Jump();
        }
    }

    void PlayerInput()
    {
        float xInput = Input.GetAxisRaw("Horizontal");
        
        _moveDir = Vector3.right * xInput;

        //if (_moveDir.magnitude != 0 )
        //{
            //transform.rotation = Quaternion.LookRotation(_moveDir, Vector3.up);
        //}

        // Jump 
        if (Input.GetButtonDown("Jump") && !_isJumping && !_pressedJump)
        {
            Debug.Log("Jumped!");
            _pressedJump = true;
        }
    }

    void PlayerMovement()
    {
        if (_rb.velocity.magnitude < _targetVelocity)
        {
            _rb.AddForce(_moveDir * _moveForce, ForceMode.Acceleration);
        }
    }

    void Jump()
    {
        _pressedJump = false;
        _isJumping = true;
        float jumpForce = Mathf.Sqrt(_jumpHeight * Physics.gravity.y * -2) * _rb.mass;
        _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void CheckGround()
    {
        if (Physics.BoxCast(transform.position, _collider.bounds.extents * 2, Vector3.down,
            out RaycastHit hit, transform.rotation, 0.1f) && _rb.velocity.y < 0)
        {            
            if (hit.collider.tag == "Ground")
            {
                _isJumping = false;
            }
        }
    }
}
