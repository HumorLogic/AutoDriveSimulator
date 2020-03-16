using UnityEngine;

namespace Simulator
{
    public class CameraFollow : MonoBehaviour
    {
        Vector3 smoothPosVelocity;  // Velocity of position smoothing
        Vector3 smoothRotValocity;  // Velocity of Rotation Smoothing

        private void FixedUpdate()
        {
            Car bestCar = transform.GetChild(0).GetComponent<Car>();    // The best car in the bunch is the first one

            for (int i = 0; i < transform.childCount; i++)  // Loop over all the cars
            {
                Car currentCar = transform.GetChild(i).GetComponent<Car>(); // Get the component of the current car

                if (currentCar.Fitness > bestCar.Fitness)   // If the current car is better than the best car
                {
                    bestCar = currentCar;   //Then , the best car is the current car
                }
            }

            Transform bestCarCamPos = bestCar.transform.GetChild(0);    // The target position of the camera relative to the best car
            Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position,
                                                                                                       bestCarCamPos.position,
                                                                                                       ref smoothPosVelocity,
                                                                                                       0.7f);   // Smoothly set the position
            Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation,
                                                                                            Quaternion.LookRotation(bestCar.transform.position - Camera.main.transform.position),
                                                                                            0.1f);  // Smoothly set the rotation
        }
    }

}
