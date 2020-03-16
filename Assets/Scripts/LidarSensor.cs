using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LidarSensor : MonoBehaviour
{
    #region Public Set
    [SerializeField] float MaxRange;
    [SerializeField] LayerMask LidarSensorMask;
    [SerializeField] bool ShowLine;
    #endregion

    /// <summary>
    /// The difference between left and right range
    /// </summary>
    public float CTE
    {
        get;
        private set;
    }



    private Vector3 sensorPos;
    private Vector3 lidarDir;
    private float initAngle;
    private float lidarAngle;
    private float miniL;
    private float miniR;
    private bool dir;
    
    LineRenderer leftLineRender;
    LineRenderer rightLineRender;




    void Start()
    {
        Init();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Lidar();
    }


    /// <summary>
    /// initialize the settings
    /// </summary>
    private void Init()
    {
        leftLineRender = transform.GetChild(0).GetComponent<LineRenderer>();
        rightLineRender = transform.GetChild(1).GetComponent<LineRenderer>();

        leftLineRender.positionCount = 120;
        rightLineRender.positionCount = 120;
    }


    float Lidar()
    {
        
        sensorPos = transform.position;
        lidarDir = transform.forward;
        
        if (lidarDir.z == 0)
        {
            initAngle = Mathf.PI / 2;
        }
        else if (lidarDir.z > 0 && lidarDir.x < 0)
        {
            initAngle = Mathf.Atan(lidarDir.x / lidarDir.z);
        }
        else if (lidarDir.z < 0 && lidarDir.x > 0)
        {
            initAngle = Mathf.PI + Mathf.Atan(lidarDir.x / lidarDir.z);
        }
        else if (lidarDir.z < 0 && lidarDir.x < 0)
        {
            initAngle = -(Mathf.PI - Mathf.Atan(lidarDir.x / lidarDir.z));
        }
        else
        {
            initAngle = Mathf.Atan(lidarDir.x / lidarDir.z);
        }
        
        //左边雷达初始位置从25°开始
        miniL = 25f;
        lidarAngle = initAngle - Mathf.PI / 6;
        dir = true;
        //间隔5°/线，需要24线
        for (int i = 0; i < 60; i++)
        {

            lidarDir = new Vector3(Mathf.Sin(lidarAngle), 0, Mathf.Cos(lidarAngle));
            if (dir)
            {
                // print("L：" + lidarDir);
                dir = false;
            }
            float rDis = CastRay(sensorPos, lidarDir);
           
            leftLineRender.SetPosition(2 * i, sensorPos);
            leftLineRender.SetPosition(2 * i + 1, rDis * lidarDir + sensorPos);
            if (rDis != 0 && rDis < miniL)
                miniL = rDis;
           
            lidarAngle -= Mathf.PI / 90;
           
        }
        
        //右边边雷达初始位置从25°开始
        lidarAngle = initAngle + Mathf.PI / 6;
        miniR = 25f;
        dir = true;
        for (int i = 0; i < 60; i++)
        {

            lidarDir = new Vector3(Mathf.Sin(lidarAngle), 0, Mathf.Cos(lidarAngle));
          
            float rDis = CastRay(sensorPos, lidarDir);
            rightLineRender.SetPosition(2 * i, sensorPos);
            rightLineRender.SetPosition(2 * i + 1, rDis * lidarDir + sensorPos);
            if (rDis != 0 && rDis < miniR)
                miniR = rDis;
          
            lidarAngle += Mathf.PI / 90;
        }
        
        CTE = miniL - miniR;
        return CTE;
    }

    float CastRay(Vector3 rayFrom, Vector3 rayDir)
    {
        float maxLength = 30;   // Maximum length of each ray

        RaycastHit hit;
        if (Physics.Raycast(rayFrom, rayDir, out hit, maxLength, LidarSensorMask))  // Cast a ray
        {
            float dist = Vector3.Distance(sensorPos, hit.point);   // Get the distance of the hit in the line
                                                                   // Set the position of the line
                                                                   //Debug.DrawRay(rayFrom, rayDir, Color.green);
            return dist;
        }
        else
        {
            // Set the distance of the hit in the line to the maximum distance

            return 0;  // Return the maximum distance
        }
    }
}
