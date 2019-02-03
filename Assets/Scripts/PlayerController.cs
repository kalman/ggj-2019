using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    PlayerController() { instance = this; }

    public float moveSpeed = 1f;
    public Image healthBar;
    [Range(0f, 1f)]
    public float foodHealthIncrease = 0.1f;
    public Color shellGoodColor = new Color(0, 0, 1f, 0.5f);
    public Color shellBadColor = new Color(1f, 0, 0, 0.5f);

    private Collider2D m_collider2D;
    private Rigidbody2D m_rbody2D;
    private Animator m_spriteAnimator;
    private SpriteRenderer m_shellSpriteRenderer;

    private List<SpriteRenderer> nearbyShells = new List<SpriteRenderer>();
    private Vector2 moveVector = Vector2.zero;
    private int level;
    private float health;

    public int Level()
    {
        return this.level;
    }

    void Awake()
    {
        m_collider2D = GetComponent<Collider2D>();
        m_rbody2D = GetComponent<Rigidbody2D>();
        m_spriteAnimator = GetComponentInChildren<Animator>();
        foreach (var r in GetComponentsInChildren<SpriteRenderer>())
        {
            if (r.tag == "Shell")
            {
                m_shellSpriteRenderer = r;
                break;
            }
        }
    }

    void Start()
    {
        level = 0;
        AddHealth(0.5f);
    }

    void Update()
    {
        float hInput = Input.GetAxisRaw("Horizontal");
        float vInput = Input.GetAxisRaw("Vertical");
        bool submitDown = Input.GetButtonDown("Submit");
        moveVector = new Vector2(hInput, vInput).normalized * moveSpeed;
        m_spriteAnimator.SetBool("Walk", moveVector != Vector2.zero);

        if (hInput != 0f)
        {
            foreach (var r in GetComponentsInChildren<SpriteRenderer>())
            {
                if (r.tag != "HealthBar")
                {
                    r.flipX = hInput < 0f;
                }
            }
        }

        if (submitDown)
        {
            SwapShell();
        }
    }

    void FixedUpdate()
    {
        m_rbody2D.velocity = moveVector;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Food")
        {
            AddHealth(foodHealthIncrease);
            GameObject.Destroy(other.gameObject);
            return;
        }

        if (other.tag == "Shell")
        {
            if (nearbyShells.Count > 0)
            {
                nearbyShells[nearbyShells.Count - 1].color = Color.white;
            }
            // TODO: Change color if the player gets enough health while the
            // shell is active.
            var shellRenderer = other.GetComponent<SpriteRenderer>();
            shellRenderer.color = health == 1f ? shellGoodColor : shellBadColor;
            nearbyShells.Add(shellRenderer);
            return;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Shell")
        {
            var shellRenderer = other.GetComponent<SpriteRenderer>();
            shellRenderer.color = Color.white;
            nearbyShells.Remove(shellRenderer);
            if (nearbyShells.Count > 0)
            {
                nearbyShells[nearbyShells.Count - 1].color =
                    health == 1f ? shellGoodColor : shellBadColor;
            }
        }
    }

    private void AddHealth(float addHealth)
    {
        health = Mathf.Min(health + addHealth, 1f);
        healthBar.fillAmount = health;
    }

    private void SwapShell()
    {
        if (health < 1f || nearbyShells.Count == 0)
        {
            return;
        }
        var nearbyShell = nearbyShells[nearbyShells.Count - 1];
        m_shellSpriteRenderer.sprite = nearbyShell.sprite;
        nearbyShells.Remove(nearbyShell);
        GameObject.Destroy(nearbyShell.gameObject);
    }
}