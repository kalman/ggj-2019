using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomWalker : MonoBehaviour
{
    public float restDuration = 1f;
    public float walkDuration = 1f;
    public float walkSpeed = 1f;
    public float jitter = 1f;
    public string animatorWalkTag = "Walk";

    private Animator m_animator;

    private Vector3 walkDirection;
    private bool isWalking;
    private Timer timer;

    void Start()
    {
        // Must be instantiated in Start because it's setup by setting on a
        // prefab.
        m_animator = GetComponentInChildren<Animator>();
        isWalking = true;
        timer = new Timer(walkDuration);
        InitWalkDirection();
    }

    void Update()
    {
        if (timer.Update(Time.deltaTime))
        {
            isWalking = !isWalking;
            if (isWalking)
            {
                timer = new Timer(walkDuration * Jitter());
                InitWalkDirection();
            }
            else
            {
                timer = new Timer(restDuration * Jitter());
            }
            if (m_animator != null)
            {
                m_animator.SetBool(animatorWalkTag, isWalking);
            }
            return;
        }

        if (isWalking)
        {
            transform.position += walkDirection * walkSpeed * Time.deltaTime;
        }
    }

    private void InitWalkDirection()
    {
        walkDirection = new Vector3(Random.value - 0.5f, Random.value - 0.5f, 0).normalized;
        // TODO: Look in the direction.
    }

    private float Jitter()
    {
        return Mathf.Max((Random.value - 0.5f) * jitter, 0f);
    }
}
