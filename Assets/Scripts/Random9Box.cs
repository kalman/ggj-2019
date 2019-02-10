using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Random9Box : MonoBehaviour, Collider2DManager.Listener
{
    public static Random9Box instance;
    Random9Box() { instance = this; }

    public Collider2D target;
    public Collider2D boxPrefab;
    public GameObject boxContainer;
    public int buffer = 0;
    [Range(0, 0.05f)]
    public float foodsPerUnit = 0.02f;
    public Collider2D foodPrefab;
    public FoodRenderer[] foodRenderers;
    public GameObject foodContainer;
    [Range(0, 0.02f)]
    public float shellsPerUnit = 0.01f;
    public SpriteRenderer shellPrefab;
    public Sprite[] shellSprites;
    public GameObject shellContainer;

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
        GameObject bcObj = GameObject.Instantiate(boxPrefab.gameObject, boxContainer.transform);
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

        // Food at the current level.
        if (area * foodsPerUnit > Random.value)
        {
            var foodInstance = Util.InstantiateAs<Collider2D>(
                   foodPrefab,
                   topLeft + areaVector / 2f,
                   foodContainer.transform);
            var foodRenderer = foodRenderers[Mathf.Min(level, foodRenderers.Length)];
            GameObject.Instantiate(foodRenderer, foodInstance.transform);
        }

        // Food and shells at the next level. Scaled 2x in every dimension (area
        // 4x), but only every 4th quadrant so that they're not spawned on top
        // of each other.
        if (topLeft.x % 2 == 0 && topLeft.y % 2 == 0)
        {
            Vector2 scale2x = new Vector2(2f, 2f);
            if (area * foodsPerUnit > Random.value)
            {
                var foodInstance = Util.InstantiateAs<Collider2D>(
                    foodPrefab,
                    topLeft + areaVector / 2f,
                    scale2x,
                    transform);
                // level+1 because we're spawning foods of the next level up.
                var foodRenderer = foodRenderers[Mathf.Min(level + 1, foodRenderers.Length)];
                GameObject.Instantiate(foodRenderer, foodInstance.transform);
            }
            if (area * shellsPerUnit > Random.value)
            {
                var shellInstance = Util.InstantiateAs<SpriteRenderer>(
                    shellPrefab,
                    topLeft + areaVector / 2f,
                    scale2x,
                    shellContainer.transform);
                // level+1 because we're spawning shells of the next level up.
                var shellSprite = shellSprites[Mathf.Min(level + 1, shellSprites.Length)];
                shellInstance.sprite = shellSprite;
            }
        }
    }
}
