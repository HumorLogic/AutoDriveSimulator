using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AutoDriveSimulator
{
    public class Search : MonoBehaviour
    {

        private Vector3[] motion = new Vector3[4];
        private List<Vector3> open = new List<Vector3>();
        private Vector3 next;
        private int x;
        private int y;
        private int g;
        private TileGrid tileGrid;
        private Tile tile;
        private int gridWidth;
        private int gridHeight;
        private Vector3 desVector;  //目的地


        // Start is called before the first frame update
        void Start()
        {
            motion[0] = new Vector3(1, -1, 0);    //上
            motion[1] = new Vector3(1, 1, 0);     //下
            motion[2] = new Vector3(1, 0, -1);   //左
            motion[3] = new Vector3(1, 0, 1);    //右

            tileGrid = GameObject.Find("Grid").GetComponent<TileGrid>();
            gridHeight = tileGrid.Rows;
            gridWidth = tileGrid.Cols;
            x = (int)tileGrid.initialPosition.x;
            y = (int)tileGrid.initialPosition.y;
            g = 0;

            desVector = new Vector3(0, (int)tileGrid.destination.x, (int)tileGrid.destination.y);

            next = new Vector3(g, x, y);
            open.Add(next);


        }

        // Update is called once per frame
        void Update()
        {

        }

        public void StartSearch()
        {
            StartCoroutine("SearchMethod");
        }

        public IEnumerator SearchMethod()
        {
            while (IsStep(desVector)==true)
            {
                open.Sort(new SearchVectorComparer());
                next = open[0];
                open.RemoveAt(0);

                //next=open.
                for (int i = 0; i < motion.Length; i++)
                {
                    Vector3 temp;
                    temp = next + motion[i];

                    if (IsStep(temp))
                    {
                        if (temp.y >= 0 && temp.y < gridWidth && temp.z >= 0 && temp.z < gridHeight)
                        {
                            open.Add(temp);
                            //StartCoroutine("MarkTile",temp);
                            MarkTile(temp);

                        }
                    }

                }
                CloseTile(next);
                //yield return new WaitForSeconds(0.2f);
                yield return new WaitForFixedUpdate();

            }

            //for (int j = 0; j < 80; j++)
            //{
               
            //}
            
        }

        public bool IsStep(Vector3 next) {

            string key = $"Tile({next.y},{next.z})";
          //  print(key);
            if (TileGrid.tileDic.ContainsKey(key)){
                tile = TileGrid.tileDic[key];
                if (tile.TileType != TileType.Obsticle)
                    return !TileGrid.tileDic[key].isStepped;
                else return false;
            }
           
            else return false;
        
        }

        public void CloseTile(Vector3 tileVector)
        {
            string key = $"Tile({tileVector.y},{tileVector.z})";
            tile=TileGrid.tileDic[key];
            tile.isStepped = true;
            tile.SetColor(Color.gray);
            
        }

     

        public void MarkTile(Vector3 tileVector)
        {
            
            string key = $"Tile({tileVector.y},{tileVector.z})";
            tile = TileGrid.tileDic[key];
            tile.SetColor(Color.green);

        }

        

        public class SearchVectorComparer : IComparer<Vector3>
        {
            public int Compare(Vector3 x, Vector3 y)
            {
                var compare = x.x - y.x;
                return compare < 0 ? -1 : compare > 0 ? 1 : 0;
                //hrow new System.NotImplementedException();
            }
        }


    }

}
