/// Author : Humor Logic 雍
/// URL : http://www.humorlogic.com
/// Github : https://github.com/HumorLogic

#region Includes

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        public  Dictionary<Vector2, Node> nodeDic = new Dictionary<Vector2, Node>();
        private bool playedSearch;


        #endregion

        #region Methods
        void Start()
        {
            DrawGrid();
            InitialData();
        }

        void Update()
        {
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
                //foreach (var item in nodeDic)
                //{
                //    print(item.Value.State);
                //}
            }

        }


        /// <summary>
        /// Draw the Grid
        /// </summary>
        private void DrawGrid()
        {
            ArrangeGrid(rows, cols);
            ResetNodeType();
            InitialNodes();
        }

        /// <summary>
        /// Arrange nodes to be a grid method 
        /// </summary>
        /// <param name="rows">the grid's rows</param>
        /// <param name="cols">the grid's cols</param>
        private void ArrangeGrid(int rows,int cols)
        {

            for (int r = 0; r < rows; r++)
            {
                for (int c= 0; c < cols; c++)
                {
                    Node node = new Node(this, r, c, NodeType.Normal);
                    node.InitNode(transform, nodePrefab);
                    nodeDic.Add(node.Pos, node);
                }
            }

        }


        /// <summary>
        /// Reset node's special type
        /// </summary>
        private void ResetNodeType()
        {
            foreach (Vector2 obs in obsticles)
            {
                nodeDic[obs].NodeType = NodeType.Obsticle;
            }
            nodeDic[startPosition].NodeType = NodeType.Start;
            nodeDic[destination].NodeType = NodeType.Destination;
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
            startNode = nodeDic[startPosition];
            startNode.State = new Vector3(startPosition.x, startPosition.y, 0);
            nodeDic[startPosition] = startNode;
           
            //set NodeGerid's desNode
            desNode = nodeDic[destination];
            desNode.State = new Vector3(destination.x, destination.y, 0);
            nodeDic[destination] = desNode;

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

