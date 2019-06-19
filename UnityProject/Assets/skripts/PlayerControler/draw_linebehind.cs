using System.Collections.Generic;
using UnityEngine;

public class draw_linebehind : MonoBehaviour
{
    private LineRenderer LineRenderer;

    private List<Vector3> points = new List<Vector3>();

    private void Start()
    {
        LineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            points.Clear();
        if (DistanceToLastPoint(transform.position) > 0.2F)
        {
            points.Add(transform.position);

            LineRenderer.positionCount = points.Count;
            LineRenderer.SetPositions(points.ToArray());
        }
    }

    private float DistanceToLastPoint(Vector3 point)
    {
        if (points.Count < 1)
            return Mathf.Infinity;
        return Vector3.Distance(points[points.Count - 1], point);
    }
}
