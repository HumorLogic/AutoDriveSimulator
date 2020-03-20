/// Author : Humor Logic 雍
/// URL : http://www.humorlogic.com/
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
        public int rows = 15;
        public int cols = 18;
        public GameObject nodePrefab;
        public Color nodeColor = new Color(0.15f, 0.26f, 0.61f);
        public Color obsticleColor = new Color(0, 0, 0);
        public Color startColor = new Color(1, 0.95f, 0.13f);
        public Color destinationColor = new Color(1f, 0.1f, 0.1f);
        public Color pathColor = new Color(0f, 0.89f, 0.99f);
        public Color steppedColor = new Color(0.6f, 0.6f, 0.6f);
        public Vector2[] obsticles;
        public Vector2 startPosition;
        public Vector2 destination;

        private static int nodeNum;


        public  Dictionary<Vector2, Node> nodeDic = new Dictionary<Vector2, Node>();
        private static Color _pathColor;
        public static Color _steppedColor;


        void Start()
        {
            //InitialData();
            //nodeNum = rows * cols;

            //AeraSet();

            DrawGrid();
        }

      
        /// <summary>
        /// Draw the Grid
        /// </summary>
        private void DrawGrid()
        {
            ArrangeGrid(rows, cols);
            ResetNodeType();
            InitialNodesColor();
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
        ///  Initial every node's color
        /// </summary>
        private void InitialNodesColor()
        {
            foreach (var item in nodeDic)
            {
                Node node = item.Value;
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
        /// 设置贴块类型和颜色等属性
        /// </summary>
        private void AeraSet()
        {
            foreach (var item in obsticles)
            {
                //string nodeName = $"node({item.x},{item.y})";
                //print(nodeName);
                nodeDic[item].NodeType = NodeType.Obsticle;
            }

            //string initialName = $"node({initialPosition.x},{initialPosition.y})";
            

            //string desName = $"node({destination.x},{destination.y})";
            

            foreach (var item in nodeDic)
            {
                Node node = item.Value;
                switch (node.NodeType)
                {
                    case NodeType.Normal:
                        node.SetColor(new Color(0.15f, 0.26f, 0.61f));
                        break;
                    case NodeType.Obsticle:
                        node.SetColor(Color.black);
                        //node.isStepped= true;
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

        public static void SetnodePathColor(Vector2 vector)
        {
            string key = $"node({vector.x},{vector.y})";
           // Node node = nodeDic[key];
         //   node.SetColor(_pathColor);

        }

        private void InitialData()
        {
            _pathColor = pathColor;
            _steppedColor = steppedColor;
        }

        public static void SetnodeClosed(int row, int col)
        {
            //if (nodeDic.Count == nodeNum)
            //{
            //    string key = $"node({row},{col})";
            //    nodeDic[key].isStepped = true;
            //    nodeDic[key].SetColor(Color.gray);
            //}
        }
    }
}

