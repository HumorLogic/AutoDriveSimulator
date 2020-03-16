using UnityEngine;

namespace Simulator
{
    public class Wall : MonoBehaviour
    {
        [SerializeField] string LayerHitName = "CarCollider";   // The name of the layer set on each car

        private void OnCollisionEnter(Collision collision)  // Once anythong hits the wall
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("LayerHitName"))
            {
                //collision.transform.GetComponent<Car>().WallHit();  // If it is a car, tell it that it just hit a wall
            }
        }
    }
}

