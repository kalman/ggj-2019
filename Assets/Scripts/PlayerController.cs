using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float healthBase = 10f;
    public List<GameObject> shellPrefabs = new List<GameObject>();
    public SpriteRenderer healthBar;

    private Collider2D m_collider2D;
    private Rigidbody2D m_rbody2D;
    private Animator m_spriteAnimator;

    private Vector2 moveVector = Vector2.zero;
    private float level;
    private float health;

    void Awake()
    {
        m_collider2D = GetComponent<Collider2D>();
        m_rbody2D = GetComponent<Rigidbody2D>();
        m_spriteAnimator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        SelectShell(shellPrefabs[0]);
        level = 1f;
        AddHealth(healthBase / 2f);
    }

    void Update()
    {
        float hInput = Input.GetAxisRaw("Horizontal");
        float vInput = Input.GetAxisRaw("Vertical");
        bool submitDown = Input.GetButtonDown("Submit");
        moveVector = new Vector2(hInput, vInput).normalized * moveSpeed;
        m_spriteAnimator.SetBool("Walk", moveVector != Vector2.zero);
    }

    void FixedUpdate()
    {
        m_rbody2D.velocity = moveVector;
    }

    private void SelectShell(GameObject shellPrefab)
    {
        GameObject shell = GameObject.Instantiate(shellPrefab, transform);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Edible edible = other.GetComponent<Edible>();
        if (edible != null)
        {
            AddHealth(edible.value);
            edible.Eat();
        }
    }

    private void AddHealth(float addHealth)
    {
        float maxHealth = level * healthBase;
        health = Mathf.Min(health + addHealth, maxHealth);
        float healthScale = health / maxHealth;
        float pxPerUnit = healthBar.sprite.pixelsPerUnit;
        healthBar.transform.localScale = new Vector3(healthScale, 1f, 1f);
        // TODO: Make this fill in from the left.
    }
}