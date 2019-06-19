using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class main : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            var player = GameObject.Find("Player");

            var start = GameObject.Find("StartPoint").transform;

            player.transform.position = new Vector3(start.position.x, start.position.y, -10);

            Debug.Log(new Vector3(start.position.x, start.position.y, -10) + "  -  " + player.transform.position);

        }
    }
}
