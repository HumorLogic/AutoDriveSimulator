using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Vehicles.Car
{
    [RequireComponent(typeof (CarController))]
    public class CarUserControl : MonoBehaviour
    {
        private CarController m_Car; // the car controller we want to use
       // private GameObject sensor;
        private Vector3 sensorPos;
        //private GameObject sensor;
        private float tempCTE;
        private float d_cte;
        private float int_cte;

        // Sensor Members Variablies
        public GameObject lidarSensor;
       
     


      




        private void Awake()
        {
            // get the car controller
            m_Car = GetComponent<CarController>();
            if (lidarSensor == null)
            {
                lidarSensor = GameObject.Find("LidarSensor");
                
            }
           
            int_cte = 0.0f;
            
        }


        private void FixedUpdate()
        {
            // pass the input to the car!
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            float v = CrossPlatformInputManager.GetAxis("Vertical");
#if !MOBILE_INPUT
            float handbrake = CrossPlatformInputManager.GetAxis("Jump");
            //P:0.56
            h = PID_Cotroller(0.50f, 8f, 0.0001f);
            v = 0.45f;
            //pid:0.5,5,0.0001 v:0.35

            m_Car.Move(h, v, v, handbrake);

            
            
            
            //Debug.DrawRay(transform.position,Vector3.forward, Color.green);
            
#else
            m_Car.Move(h, v, v, 0f);
#endif
        }


        private float PID_Cotroller(float Kp, float Kd,float Ki)
        {
            float steer = 0;
            
            //get the distance from two walls
          
            float cte = lidarSensor.GetComponent<LidarSensor>().CTE;
            int_cte += cte;
            d_cte = cte - tempCTE;
            tempCTE = cte;
          
            //print("CTE: "+cte);
            //print("Int CTE: "+ int_cte);
            //±ÈÀý¿ØÖÆ£º
            steer = -Kp * cte-d_cte*Kd-Ki*int_cte;
           // print("steer: "+steer);
           // print("-----");
            return steer;
        }



    }
}
