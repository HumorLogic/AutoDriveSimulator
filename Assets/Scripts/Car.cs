using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using System;

namespace Simulator
{
    public class Car : MonoBehaviour
    {

        #region My Set Members
        [Header("Car Set Option")]
        [SerializeField] bool UseUserInput = false; // Define wether the car uses a NeuralNetwork or user input
        [SerializeField] LayerMask SensorMask;    // Define the layer of the walls('Wall')
        [SerializeField] float FitnessUnchangedDie = 5;  //   The number of seconds to wait before checking if the fitness didn't increase

        //public NeuralNetwork that refers to the next neural network to be set to the next instantiated car
        public static NeuralNetwork NextNetwork = new NeuralNetwork(new uint[] { 6, 4, 3, 2 }, null);
        public string TheGuid { get; private set; } // The Unique ID of the current car
        public int Fitness { get; private set; }    //The fitness/score of the current car. Represents the number of checkpoints that his car hit.
        public NeuralNetwork TheNetwork { get; private set; }   // The NeuralNetwork of the current car
        Rigidbody TheRigidbody; // The Rigidbody of the current car
        LineRenderer TheLineRenderer;   // The LineRenderer of the current car

        #endregion

        #region Added Method
        private void Awake()
        {

            //stratPosiont = transform.position;
            //startRotation = transform.eulerAngles;
            TheGuid = Guid.NewGuid().ToString();    // Assigns a new Unique ID for the current car

            TheNetwork = NextNetwork;   // Sets the current network to the Next Network

            //Make sure the Next Network is reassigned to avoid having another car use the same network
            NextNetwork = new NeuralNetwork(NextNetwork.Topology, null);

            TheRigidbody = GetComponent<Rigidbody>();   // Assign Rigidbody
            TheLineRenderer = GetComponent<LineRenderer>(); // Assign LineRenderer

            StartCoroutine(IsNotImproving());   // Start checking if the score stayed the same for a lot of time

            TheLineRenderer.positionCount = 17; //Make sure the line is long enough

        }

        // Checks each few seconds if the car didn't make any improvement
        IEnumerator IsNotImproving()
        {
            while (true)
            {
                int OldFitness = Fitness;   //Save the initial fitness
                yield return new WaitForSeconds(FitnessUnchangedDie);   // Wait for some time
                if (OldFitness == Fitness)  // Check if the fitness didn't change yet
                    WallHit(); // Kill this car
            }
        }

        /// <summary>
        /// The main function that moves the car 
        /// </summary>
        /// <param name="v">The speed of the car</param>
        /// <param name="h">The steering of the car </param>
        public void Move(float v, float h)
        {
            TheRigidbody.velocity = transform.right * v * 4;
            TheRigidbody.angularVelocity = transform.up * h * 3;
        }

        /// <summary>
        /// Cast a ray and makes it visible through line renderer
        /// </summary>
        /// <param name="rayDir">The direction of the ray</param>
        /// <param name="lineDir">The direction of the Renderer Line</param>
        /// <param name="linePosIndex">The Renderer Line position index</param>
        /// <returns></returns>
        double CastRay(Vector3 rayDir, Vector3 lineDir, int linePosIndex)
        {
            float length = 4;   // Maximum length of each ray

            RaycastHit hit;
            if (Physics.Raycast(transform.position, rayDir, out hit, length, SensorMask))  // Cast a ray
            {
                float dist = Vector3.Distance(hit.point, transform.position);   // Get the distance of the hit in the line
                TheLineRenderer.SetPosition(linePosIndex, dist * lineDir);  // Set the position of the line
                return dist;
            }
            else
            {
                // Set the distance of the hit in the line to the maximum distance
                TheLineRenderer.SetPosition(linePosIndex, lineDir * length);
                return length;  // Return the maximum distance
            }
        }

        void GetNeuralInputAxis(out float vertical, out float horizontal)
        {
            double[] NeuralInput = new double[NextNetwork.Topology[0]];

            // Cast forward, back, right and left 
            NeuralInput[0] = CastRay(transform.forward, Vector3.forward, 1) / 4;
            NeuralInput[1] = CastRay(-transform.forward, -Vector3.forward, 3) / 4;
            NeuralInput[2] = CastRay(transform.right, Vector3.right, 5) / 4;
            NeuralInput[3] = CastRay(-transform.right, -Vector3.right, 7) / 4;

            // Cast forward-right and forward-left
            float sqrtHalf = Mathf.Sqrt(0.5f);
            NeuralInput[4] = CastRay(transform.right * sqrtHalf + transform.forward * sqrtHalf,
                                                    Vector3.right * sqrtHalf + Vector3.forward * sqrtHalf,
                                                    9) / 4;
            NeuralInput[5] = CastRay(transform.right * sqrtHalf + -transform.forward * sqrtHalf,
                                                    Vector3.right * sqrtHalf + -Vector3.forward * sqrtHalf,
                                                    13) / 4;

            //  Feed through the network
            double[] NeuralOutput = TheNetwork.FeedForward(NeuralInput);

            // Get vertical value
            if (NeuralOutput[0] <= 0.25f)
                vertical = -1;
            else if (NeuralOutput[0] >= 0.75f)
                vertical = 1;
            else
                vertical = 0;

            // Get horizontal value
            if (NeuralOutput[1] <= 0.25f)
                horizontal = -1;
            else if (NeuralOutput[1] >= 0.75f)
                horizontal = 1;
            else
                horizontal = 0;

            // If the output is just standing still , the move the car forward
            if (vertical == 0 && horizontal == 0)
                vertical = 1;

        }


        private void FixedUpdate()
        {
            if (UseUserInput)   // If we're gonna use user input
            {
                // Move the car according to the input
                Move(CrossPlatformInputManager.GetAxis("Vertical"), CrossPlatformInputManager.GetAxis("Horizontal"));
            }
            else
            {
                float vertiacal;
                float horizontal;

                GetNeuralInputAxis(out vertiacal, out horizontal);
                Move(vertiacal, horizontal);
            }
        }

        /// <summary>
        /// This method is called through all the checkpoint when the car hits any
        /// </summary>
        public void CheckpointHit()
        {
            Fitness++;  // Increase Fitness/Score
        }


        /// <summary>
        /// Called by walls when hit the car
        /// </summary>
        public void WallHit()
        {
           //EvolutionManager.Singleton.CarDead(this, Fitness);  //  Telll the Evolution Manager that the car is dead
            //gameObject.SetActive(false);    // Make sure the car is inactive
        }


        #endregion
    }

}
