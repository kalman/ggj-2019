using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edible : MonoBehaviour
{
    public float value = 1f;

    public void Eat()
    {
        Destroy(gameObject);
    }
}
