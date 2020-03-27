/// Author : Humor Logic 雍
/// URL : http://www.humorlogic.com
/// Github : https://github.com/HumorLogic

#region Includes

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
 

#endregion

namespace AutoDriveSimulator
{
    
    /// <summary>
    /// NodeGrid class
    /// </summary>
    public class NodeGrid : MonoBehaviour
    {
        #region Members

        [Header("Path Fimd Set")]
        public int delayTime = 10;
        public bool displayCost = false;

        [Header("Grid Size")]
        public int rows = 15;
        public int cols = 18;
        public GameObject nodePrefab;

        [Header("Grid Color")]
        public Color nodeColor = new Color(0.15f, 0.26f, 0.61f);
        public Color obsticleColor = new Color(0, 0, 0);
        public Color startColor = new Color(1, 0.95f, 0.13f);
        public Color destinationColor = new Color(1f, 0.1f, 0.1f);
        public Color pathColor = new Color(0f, 0.89f, 0.99f);
        public Color markColor = new Color(0, 1, 0);
        public Color steppedColor = new Color(0.6f, 0.6f, 0.6f);

        [Header("Node Position")]
        public Vector2[] obsticles;
        public Vector2 startPosition;
        public Vector2 destination;

       
        public Node startNode { get; private set; }
        public Node desNode { get; private set; }
        public  Dictionary<float, Node> nodeDic = new Dictionary<float, Node>();
        private bool playedSearch;

        private List<Vector2> obsList = new List<Vector2>();
        private bool isSetObs;
        private SpriteRenderer sr;


        #endregion

        #region Methods
        void Start()
        {
           
            DrawGrid();

            GameObject.Find("Toggle").GetComponent<Toggle>().onValueChanged.AddListener(isOn => OnObsticleSetToggleClosed());
        }

        void Update()
        {
            if (Input.GetMouseButtonUp(0)&&isSetObs)
            {
               
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit rayhit;
                if (Physics.Raycast(ray, out rayhit))
                {
                    AddObsToList(rayhit.collider.gameObject);
                    print(rayhit.collider.gameObject.name);
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                displayCost = false;
                if (playedSearch)
                {
                    InitialData();
                    InitialNodes();
                }
                playedSearch = true;
                PathFinder.DoSearch();
                print("1 Key pressed");
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                displayCost = true;
                if (playedSearch)
                {
                    InitialData();
                    InitialNodes();
                }
                playedSearch = true;
                PathFinder.DoSearch();
                print("2 Key pressed");
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                if (playedSearch)
                {
                    InitialData();
                    InitialNodes();
                }
                playedSearch = true;
                
                PathFinder.AStarSearch();
                print("3 Key pressed");
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
                // GameObject.Find("Node(5,5)").GetComponent<SpriteRenderer>().color = Color.black;
                // print(-Mathf.Sin(Mathf.PI / 2));
               

                foreach (var item in nodeDic)
                {
                    print(item.Key);
                }
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                GameObject.Find("Node(5,5)").GetComponent<SpriteRenderer>().color = Color.white;
            }
        }

        #region Grid set related methods
        private void InitObsList()
        {
            obsList.Clear();
            foreach (var item in obsticles)
            {
                obsList.Add(item);
            }
        }

        /// <summary>
        /// Add node's position to obsList
        /// </summary>
        /// <param name="gameObject">node gameobject</param>
        private void AddObsToList(GameObject gameObject)
        {
            sr = gameObject.GetComponent<SpriteRenderer>();
            Vector2 v = new Vector2( -gameObject.transform.position.y, gameObject.transform.position.x);
            if (obsList.Contains(v))
            {
                print("deleted");
                //sr = gameObject.GetComponent<SpriteRenderer>();
                //gameObject.GetComponent<SpriteRenderer>().color = nodeColor;
                sr.color = nodeColor;
                obsList.Remove(v);
                
                return;
            }
            else
            {
                obsList.Add(v);
                //gameObject.GetComponent<SpriteRenderer>().color = Color.black;
                sr.color = Color.black;
                print("added");
            }
           
        }

        private void UpdateObsticleArray()
        {
            obsticles = new Vector2[obsList.Count];
            for (int i = 0; i < obsList.Count; i++)
            {
                obsticles[i] = obsList[i];
            }
        }
       
        private void OnObsticleSetToggleClosed()
        {
            isSetObs = !isSetObs;
            
            if (!isSetObs)
            {

                UpdateObsticleArray();
                SaveGridMap();
                DrawGrid();
            }
            
        }


        private void SaveGridMap()
        {
            
            int[,] map = new int[rows,cols];
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    map[r,c] = 0;
                }
            }

            foreach (var item in obsticles)
            {
                map[(int)item.x,(int)item.y] = 1;
            }

            string mapstr = " ";
            for (int i = 0; i < 15; i++)
            {
                string str = " ";
                for (int j = 0; j < 18; j++)
                {
                    str += map[i, j];
                    str += ",";
                }
                str += "\n";
                mapstr += str;

            }

            string path = Application.dataPath + "/Test";
            string fileName = "map.txt";

            if (Directory.Exists(path))
            {
                Debug.Log("Paht Exist");

            }
            else
            {
                Debug.Log("Paht does not Exist");
            }

            StreamWriter sw;
            FileInfo fi = new FileInfo(path + "/" + fileName);
            if (!fi.Exists)
            {
                sw = fi.CreateText();
            }
            else
            {
                fi.Delete();
                sw = fi.CreateText();
            }

            sw.Write(mapstr);
            sw.Close();
            sw.Dispose();
            

        }

        /// <summary>
        /// Read map from txt file
        /// </summary>
        private void ReadMap()
        {
            string path = Application.dataPath + "/Test";
            string fileName = "map.txt";

            string[] mapLine = new string[cols];
            string[][] map = new string[rows][];
            string line;
            int i = 0;

            if (Directory.Exists(path))
            {
                StreamReader sr;
                FileInfo fi = new FileInfo(path + "/" + fileName);
                if (fi.Exists)
                {
                    sr = fi.OpenText();
                    while (!sr.EndOfStream&&i<rows)
                    {
                        line = sr.ReadLine();
                        mapLine = line.Split(',');
                        //print(mapLine[3]);
                        map[i] = mapLine;
                        i++;
                    }

                    sr.Close();
                    sr.Dispose();
                }
                else
                {
                    Debug.Log("The file dosen't exist");
                }
            }
            else
            {
                Debug.Log("The path doesn't exist");
            }

            int count = 0;
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    if (map[r][c] == "1")
                        count++;
                }
            }

            obsticles = new Vector2[count];
            count = 0;
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    if (map[r][c] == "1")
                    {
                        obsticles[count] = new Vector2(r, c);
                        count++;
                    }

                }
            }
        }



        #endregion


        /// <summary>
        /// Draw the Grid
        /// </summary>
        private void DrawGrid()
        {
            ReadMap();
            InitObsList();

            ArrangeGrid(rows, cols);
            ResetNodeType();
            InitialNodes();
            InitialData();
        }

        /// <summary>
        /// Arrange nodes to be a grid method 
        /// </summary>
        /// <param name="rows">the grid's rows</param>
        /// <param name="cols">the grid's cols</param>
        private void ArrangeGrid(int rows,int cols)
        {
            nodeDic.Clear();
            int key;
            for (int r = 0; r < rows; r++)
            {
                for (int c= 0; c < cols; c++)
                {
                    Node node = new Node(this, r, c, NodeType.Normal);
                    node.InitNode(transform, nodePrefab);
                     key = cols * r + c;
                    nodeDic.Add(key, node);
                }
            }

        }


        /// <summary>
        /// Reset node's special type
        /// </summary>
        private void ResetNodeType()
        {
            float key;
            foreach (Vector2 obs in obsticles)
            {
               key  = cols * obs.x + obs.y;
                nodeDic[key].NodeType = NodeType.Obsticle;
            }
            key = cols* startPosition.x + startPosition.y;
            nodeDic[key].NodeType = NodeType.Start;

            key = cols * destination.x + destination.y;
            nodeDic[key].NodeType = NodeType.Destination;
        }


        /// <summary>
        ///  Initial every node's color and properties
        /// </summary>
        private void InitialNodes()
        {
            foreach (var item in nodeDic)
            {
                Node node = item.Value;
                node.ResetNoed();
                switch (node.NodeType)
                {
                    case NodeType.Normal:
                        node.SetColor(nodeColor);
                        break;
                    case NodeType.Obsticle:
                        node.SetColor(obsticleColor);
                        break;
                    case NodeType.Start:
                        node.SetColor(startColor);
                        break;
                    case NodeType.Destination:
                        node.SetColor(destinationColor);
                        break;
                }
            }

        }


        /// <summary>
        /// Initial GridNode data
        /// </summary>
        private void InitialData()
        {
            //set NodeGrid's startNode
           float key = cols * startPosition.x + startPosition.y;

            startNode = nodeDic[key];
            startNode.State = new Vector3(startPosition.x, startPosition.y, 0);
            nodeDic[key] = startNode;

            //set NodeGerid's desNode
            key = cols* destination.x +destination.y;
            desNode = nodeDic[key];
            desNode.State = new Vector3(destination.x, destination.y, 0);
            nodeDic[key] = desNode;

        }


        //private void ResetGrid()
        //{
        //    nodeDic.Clear();
        //    DrawGrid();
        //    InitialData();
        //}
       

        #endregion

    }
}

