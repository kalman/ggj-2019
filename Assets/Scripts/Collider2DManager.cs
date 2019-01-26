using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collider2DManager : MonoBehaviour
{
    public interface Listener
    {
        void OnCollision(Collider2D collider, Collider2D other);
    }

    public static Collider2DManager instance;
    private Collider2DManager() { instance = this; }

    private HashSet<Listener> listeners = new HashSet<Listener>();

    public void AddListener(Listener listener)
    {
        if (!listeners.Contains(listener))
        {
            listeners.Add(listener);
        }
    }

    public void NotifyCollision(Collider2D collider, Collider2D other)
    {
        foreach (var listener in listeners)
        {
            listener.OnCollision(collider, other);
        }
    }
}
