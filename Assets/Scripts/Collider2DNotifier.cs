using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Collider2DNotifier : MonoBehaviour
{
    private Collider2D m_collider2D;

    void Awake()
    {
        m_collider2D = GetComponent<Collider2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Collider2DManager.instance.NotifyCollision(m_collider2D, other);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Collider2DManager.instance.NotifyCollision(m_collider2D, other);
    }
}
