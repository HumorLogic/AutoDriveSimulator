using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineTest : MonoBehaviour
{
    private LineRenderer lr;


    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 10;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position,Vector3.right,out hit))
        {
            float d = Vector3.Distance(transform.position, hit.point);
            print(d);
            lr.SetPosition(0, d * Vector3.right);
        }
    }
}
