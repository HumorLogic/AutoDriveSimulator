/// Author : Humor Logic 雍
/// URL : http://www.humorlogic.com
/// Github : https://github.com/HumorLogic

#region Includes
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#endregion

namespace AutoDriveSimulator
{

    /// <summary>
    /// Node Class Type
    /// </summary>
    public enum NodeType
    {
        Normal,
        Obsticle,
        Start,
        Destination
    }


    /// <summary>
    /// Node Class
    /// </summary>
    public class Node
    {

        #region Members
        public NodeType NodeType { get; set; }
       // public NodeGrid Grid { get; private set; }
        //public int Row { get; private set; }
        //public int Col { get; private set; }
        private int row;
        private int col;

   
        public Vector2 Pos { get; private set; }
        public bool IsStepped { get; set; }
        public bool IsMarked { get; set; }
        public string Name { get; private set; }
        public Vector3 State { get; set; }
        public int gValue { get; set; }
        public int hValue { get; set; }
        public int fValue { get; set; }
        public GameObject nodeObj { get; private set; }
        public double DirAngle
        {
            get { return dir; }
            set
            {
                if (value <= -Mathf.PI) { dir = value + 2 * Mathf.PI; }
                else if (value > Mathf.PI) { dir = value - 2 * Mathf.PI; }
                else { dir = value; }
            }
        }

        private double dir; //Node direction angle

        private string[] dirSymbol = { "^", "v", "<", ">" };

        private int[] angleArr = { 90, -90, 180, 0 };
        

        #endregion

        #region Constructor

        /// <summary>
        /// Node class constructor
        /// </summary>
        /// <param name="grid">NodeGrid class object</param>
        /// <param name="row">the node‘s row</param>
        /// <param name="col">the node's col</param>
        /// <param name="type">Node type</param>
        public Node(NodeGrid grid, int row, int col, NodeType type)
        {
            //  Grid = grid;
            //Row = row;
            //Col = col;
            this.row = row;
            this.col = col;
            NodeType = type;
            Pos = new Vector2(row, col);
            Name = $"Node({row},{col})";
            IsStepped = false;
            IsMarked = false;
            dir = Mathf.PI / 2;

        }

        #endregion

        #region Methods

        /// <summary>
        /// Initial Node object method
        /// </summary>
        /// <param name="parent">node's parent transform in Unity</param>
        /// <param name="prefab">node's prefab</param>
        public void InitNode(Transform parent, GameObject prefab)
        {
            nodeObj = GameObject.Instantiate(prefab);
            nodeObj.name = Name;
            nodeObj.transform.parent = parent;
            nodeObj.transform.position = new Vector3(col, -row, 0);
        }


        /// <summary>
        /// Set node's color
        /// </summary>
        /// <param name="color">color</param>
        public void SetColor(Color color)
        {
            nodeObj.GetComponent<SpriteRenderer>().color = color;
        }


        /// <summary>
        /// Display the cost of node
        /// </summary>
        public void DisplayCost()
        {
            string dir = GetDirSymbol();
            string content = gValue.ToString() + dir;

            nodeObj.GetComponentInChildren<Text>().text = content;
            Debug.Log(Pos + " 角度:" + this.dir + " Angle:" + dir + " gValue:" + gValue);

        }

       
        private string GetDirSymbol()
        {
            int angle = (int)(dir / Mathf.PI * 180);
            Debug.Log(angle);
            string d = " ";

            for (int i = 0; i < angleArr.Length; i++)
            {
                if (angle == angleArr[i])
                    return dirSymbol[i];
            }

            //switch (angle)
            //{
            //    case (90):
            //        d=  "^";
            //        break;
            //    case (180):
            //        d = "<";
            //        break;
            //    case (-90):
            //        d = "v";
            //        break;
            //    case (0):
            //        d = ">";
            //        break;
            //    case (-180):
            //        d = "<";
            //        break;

            //}

            //switch (dir)
            //{
            //    case (Mathf.PI / 2):
            //        d = "^";
            //        break;
            //    case (Mathf.PI):
            //        d = "<";
            //        break;
            //    case (-Mathf.PI / 2):
            //        d = "v";
            //        break;
            //    case (0):
            //        d = ">";
            //        break;
            //    case (-Mathf.PI):
            //        d = "<";
            //        break;

            //}
            return d;

        }

        public void DisplayHeuristic()
        {
            nodeObj.GetComponentInChildren<Text>().text = hValue.ToString();
        }

        public void UpdateF()
        {
            fValue = hValue + gValue;
        }

        /// <summary>
        /// Reset node properties
        /// </summary>
        public void ResetNoed()
        {
            IsStepped = false;
            IsMarked = false;
            nodeObj.GetComponentInChildren<Text>().text = null;
            State = new Vector3(State.x, State.y, 0);
            gValue = (int)State.z;
            dir = Mathf.PI / 2;
        }

        #endregion

    }


    /// <summary>
    /// NodeCompare class
    /// </summary>
    public class NodeCompare : IComparer<Node>
    {
        public int Compare(Node x, Node y)
        {
            var compare = x.gValue - y.gValue;
            return compare < 0 ? -1 : compare > 0 ? 1 : 0;
        }
    }

    /// <summary>
    /// NodeAStarCompare class
    /// </summary>
    public class NodeAStarCompare : IComparer<Node>
    {
        public int Compare(Node x, Node y)
        {
            var compare = x.fValue - y.fValue;
            return compare < 0 ? -1 : compare > 0 ? 1 : 0;
        }
    }

}
