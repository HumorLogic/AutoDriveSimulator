using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AutoDriveSimulator
{
    public class Search : MonoBehaviour
    {

        private Vector2[] motion = new Vector2[4];
        private List<Vector3> open = new List<Vector3>();
       // private Vector3[] open;
        private int x;
        private int y;
        private int g;
        private TileGrid tileGrid;
        // Start is called before the first frame update
        void Start()
        {
            motion[0] = new Vector2(-1, 0);
            motion[1] = new Vector2(1, 0);
            motion[2] = new Vector2(0, -1);
            motion[3] = new Vector2(0, 1);

            tileGrid = GameObject.Find("Grid").GetComponent<TileGrid>();
            x = (int)tileGrid.initialPosition.x;
            y = (int)tileGrid.initialPosition.y;
            g = 0;

            open.Add(new Vector3(x, y, g));
           
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SearchMethod()
        {

        }
    }

}
