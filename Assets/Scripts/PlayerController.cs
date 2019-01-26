using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
// [RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;

    private Collider2D m_collider2D;
    private Rigidbody2D m_rbody2D;

    private Vector2 moveVector = Vector2.zero;

    void Awake()
    {
        m_collider2D = GetComponent<Collider2D>();
        m_rbody2D = GetComponent<Rigidbody2D>();
    }

    void Start()
    {

    }

    void Update()
    {
        float hInput = Input.GetAxisRaw("Horizontal");
        float vInput = Input.GetAxisRaw("Vertical");
        bool submitDown = Input.GetButtonDown("Submit");
        moveVector = new Vector2(hInput, vInput).normalized * moveSpeed; // * Time.deltaTime;
    }

    void FixedUpdate()
    {
        m_rbody2D.velocity = moveVector;
    }
}
