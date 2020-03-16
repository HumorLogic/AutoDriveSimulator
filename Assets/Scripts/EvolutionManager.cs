using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Simulator
{
    public class EvolutionManager : MonoBehaviour
    {

        public static EvolutionManager Singleton = null;    // The current EvolutionManager Instance


        [SerializeField] bool UseNodeMutation = true;   // Should we use node mutation?
        [SerializeField] int CarCount = 100;    //  The number of cars per generation
        [SerializeField] GameObject CarPrefab;  // The Prefab of the car to be created for each instance
        [SerializeField] Text GenerationNumberText;  // Some text to write the generation number

        int generationCount = 0;    // The current generation number
        List<Car> cars = new List<Car>();   // This list od cars currently alive

        NeuralNetwork bestNeuralNetwork = null; // The best NeuralNetwork currently avalable
        int bestFitness = -1;   // The Fitness of the best NeuralNetwork ever created

        // On Start
        void Start()
        {
            if (Singleton == null)  // If no other instances were created
                Singleton = this;   //  Make the only instance this one
            else
                gameObject.SetActive(false);    // There is another instance already in place. Make this one inactive.

            bestNeuralNetwork = new NeuralNetwork(Car.NextNetwork); // Set the bestNeuralNetwork to a random new network

            StartGeneration();
        }

        /// <summary>
        /// Starts a whole new genneration
        /// </summary>
        void StartGeneration()
        {
            generationCount++;  // Increment the generation count
            GenerationNumberText.text = "Generation: " + generationCount; // Update generation text

            for (int i = 0; i < CarCount; i++)
            {
                if (i == 0)
                    Car.NextNetwork = bestNeuralNetwork;    // Make sure one car uses the best network
                else
                {
                    // Clone the best neural network and set it to be for the next car
                    Car.NextNetwork = new NeuralNetwork(bestNeuralNetwork);
                    Car.NextNetwork.Mutate();   //Mutate it 
                }

                // Instantiate a new car and add it to the list of cars
                cars.Add(Instantiate(CarPrefab, transform.position, Quaternion.identity, transform).GetComponent<Car>());
            }
        }


        /// <summary>
        /// Gets called by cars when they die
        /// </summary>
        public void CarDead(Car deadCar, int fitness)
        {
            cars.Remove(deadCar);   // Remove the car from the list
            Destroy(deadCar.gameObject);    // Destory the dead car

            if(fitness> bestFitness)
            {
                bestNeuralNetwork = deadCar.TheNetwork; // Make sure it becomes the best car
                bestFitness = fitness;  // And also set the best fitness
            }

            if (cars.Count <= 0)    // If there are no cars left
                StartGeneration();  // Create a new generation
        }
    }

}
