using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Random9Box : MonoBehaviour, Collider2DManager.Listener
{
    public static Random9Box instance;
    Random9Box() { instance = this; }

    public Collider2D target;
    public Collider2D boxPrefab;
    public int buffer = 0;
    [Range(0, 0.05f)]
    public float foodsPerUnit = 0.02f;
    public SpriteRenderer foodPrefab;
    public Sprite[] foodSprites;
    [Range(0, 0.02f)]
    public float shellsPerUnit = 0.01f;
    public SpriteRenderer shellPrefab;
    public Sprite[] shellSprites;

    private Dictionary<Vector2, Collider2D> boxes = new Dictionary<Vector2, Collider2D>();

    private void Start()
    {
        Collider2DManager.instance.AddListener(this);
        Generate(Vector2.zero);
    }

    private void Generate(Vector2 pos)
    {
        for (int x = -1 - buffer; x <= 1 + buffer; x++)
        {
            for (int y = -1 - buffer; y <= 1 + buffer; y++)
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

    private void SpawnRandomObjects(Vector2 topLeft, Vector2 bottomRight)
    {
        Vector2 areaVector = bottomRight - topLeft;
        float area = areaVector.x * areaVector.y;
        int level = PlayerController.instance.Level();
        if (area * foodsPerUnit > Random.value)
        {
            var foodInstance = Util.InstantiateAs<SpriteRenderer>(foodPrefab, topLeft + areaVector / 2f, transform);
            var foodSprite = foodSprites[Mathf.Min(level, foodSprites.Length)];
            foodInstance.sprite = foodSprite;
        }
        if (area * shellsPerUnit > Random.value)
        {
            var shellInstance = Util.InstantiateAs<SpriteRenderer>(shellPrefab, topLeft + areaVector / 2f, transform);
            // level+1 because we're spawning shells of the next level up.
            var shellSprite = shellSprites[Mathf.Min(level + 1, shellSprites.Length)];
            shellInstance.sprite = shellSprite;
        }
    }
}
