using UnityEngine;

[RequireComponent(typeof(CircleCollider2D), typeof(LineRenderer))]
public class CircleColliderVisualizer : MonoBehaviour
{
    public int segments = 50;
    public Color lineColor = new Color(0f, 1f, 0f, 0.5f);
    public float lineWidth = 0.05f;

    private LineRenderer lineRenderer;
    private CircleCollider2D circleCollider;

    void Awake()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.positionCount = segments + 1;
        lineRenderer.loop = true;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;
    }

    void Update()
    {
        DrawCircle();
    }

    void DrawCircle()
    {
        float radius = circleCollider.radius * Mathf.Max(transform.localScale.x, transform.localScale.y);
        // ローカルスケールを考慮

        Vector3 center = transform.position + (Vector3)circleCollider.offset;
        float angle = 0f;

        for (int i = 0; i <= segments; i++)
        {
            float x = Mathf.Cos(angle) * radius + center.x;
            float y = Mathf.Sin(angle) * radius + center.y;
            lineRenderer.SetPosition(i, new Vector3(x, y, 0));
            angle += 2 * Mathf.PI / segments;
        }
    }
}
