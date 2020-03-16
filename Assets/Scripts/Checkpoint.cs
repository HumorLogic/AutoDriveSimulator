using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Simulator
{
    public class Checkpoint : MonoBehaviour
    {

        [SerializeField] string LayerHitName = "CarCollider";   // The name of the layer set on each car
        List<string> AllGuids = new List<string>(); // The list of Guids of all the cars increased

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.layer == LayerMask.NameToLayer(LayerHitName))   // If this object is a car
            {
                Car carComponent = other.transform.parent.GetComponent<Car>();  //  Get the compoent of the car
                string carGuid = carComponent.TheGuid;  // Get the Unique ID of the car
                if (!AllGuids.Contains(carGuid))    // If we didn't increase the car before
                {
                    AllGuids.Add(carGuid);  // Make sure we don't increase it again
                    carComponent.CheckpointHit();   // Increase the car's fitness
                }
            }
        }

    }
}

