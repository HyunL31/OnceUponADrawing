using System.Collections.Generic;
using UnityEngine;

public class PathDrawer : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public GrandmaFollow grandma;

    private List<Vector3> points = new List<Vector3>();
    private bool drawing = false;

    public static PathDrawer Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
    }

    public void ClearLine()
    {
        lineRenderer.positionCount = 0;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            drawing = true;
            points.Clear();
            lineRenderer.positionCount = 0;
            grandma.ResetPath();
        }

        if (Input.GetMouseButton(0) && drawing)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;

            if (points.Count == 0 || Vector3.Distance(points[points.Count - 1], mousePos) > 0.1f)
            {
                points.Add(mousePos);
                lineRenderer.positionCount = points.Count;
                lineRenderer.SetPosition(points.Count - 1, mousePos);

                grandma.AddPathPoint(mousePos);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            grandma.ReturnToStart();
            lineRenderer.positionCount = 0;
            drawing = false;
        }
    }
}