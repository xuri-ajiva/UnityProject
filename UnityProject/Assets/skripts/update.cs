using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class update : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        var ra = new System.Random();
        gameObject.transform.position += new Vector3(ra.Next(0, 10), ra.Next(0, 10), ra.Next(0, 10));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
