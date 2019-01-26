using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Random9Box : MonoBehaviour, Collider2DManager.Listener
{
    public Collider2D target;
    public Collider2D boxPrefab;
    public GameObject wormPrefab;
    [Range(0, 0.05f)]
    public float wormsPerUnit = 0.02f;

    private Dictionary<Vector2, Collider2D> boxes = new Dictionary<Vector2, Collider2D>();

    private void Start()
    {
        Collider2DManager.instance.AddListener(this);
        Generate(Vector2.zero);
    }

    private void Generate(Vector2 pos)
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector2 boxPos = new Vector2(pos.x + x, pos.y + y);
                if (!boxes.ContainsKey(boxPos))
                {
                    boxes.Add(boxPos, InstantiateCollider(boxPos));
                }
            }
        }
    }

    private Collider2D InstantiateCollider(Vector2 pos)
    {
        GameObject bcObj = GameObject.Instantiate(boxPrefab.gameObject, transform);
        Transform bcTargetTransform = boxPrefab.gameObject.transform;
        Vector3 bcPos = new Vector3(
            bcTargetTransform.localScale.x * pos.x,
            bcTargetTransform.localScale.y * pos.y,
            bcTargetTransform.position.z
        );
        bcObj.transform.position = bcPos;
        if (pos != Vector2.zero)
        {
            SpawnRandomObjects(bcPos - bcTargetTransform.localScale, bcPos + bcTargetTransform.localScale);
        }
        return bcObj.GetComponent<Collider2D>();
    }

    public void OnCollision(Collider2D collider, Collider2D other)
    {
        if (other != target)
        {
            return;
        }
        foreach (var boxEntry in boxes)
        {
            if (collider == boxEntry.Value)
            {
                Generate(boxEntry.Key);
                return;
            }
        }
    }

    private void SpawnRandomObjects(Vector3 topLeft, Vector3 bottomRight)
    {
        float rand = Random.value;
        Vector3 areaVector = bottomRight - topLeft;
        float area = areaVector.x * areaVector.y;
        if (area * wormsPerUnit > rand)
        {
            GameObject.Instantiate(wormPrefab, topLeft + areaVector / 2f, Quaternion.identity, transform);
        }
    }
}
