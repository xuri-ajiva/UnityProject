using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class draw_point : MonoBehaviour
{

    private LineRenderer LineRenderer;

    private List<Vector3> points = new List<Vector3>();

    public Action<IEnumerable<Vector3>> OnNewPathCreated = delegate { };

    private void Awake()
    {
        LineRenderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        //LineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            points.Clear();
        if (Input.GetKey(KeyCode.E))
        {
            Ray ray = Camera.main.ScreenPointToRay(GameObject.Find("Image").transform.position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (DistanceToLastPoint(hit.point) > 1F)
                {
                    points.Add(hit.point);

                    LineRenderer.positionCount = points.Count;
                    LineRenderer.SetPositions(points.ToArray());
                }
            }
        }
        else if (Input.GetKeyUp(KeyCode.E))
            OnNewPathCreated(points);
    }

    private float DistanceToLastPoint(Vector3 point)
    {
        if (points.Count < 1)
            return Mathf.Infinity;
        return Vector3.Distance(points[points.Count-1], point);
    }
}
