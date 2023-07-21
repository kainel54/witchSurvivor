using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{    
    [SerializeField]
    private float speed = 3f;

    private Rigidbody2D rigid = null;
    private Animator animator = null;
    public Vector2 moveDirection { get; private set; }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        rigid.MovePosition(rigid.position + moveDirection * Time.fixedDeltaTime * speed);
    }

    private void LateUpdate()
    {
        animator.SetFloat("Speed", moveDirection.magnitude);
        if(moveDirection.x != 0)
        {
            transform.localRotation = moveDirection.x < 0 ? Quaternion.Euler(0f, 180f, 0f) : Quaternion.identity;
        }
    }

    private void OnMove(InputValue inputValue)
    {
        moveDirection = inputValue.Get<Vector2>();
    }
}
