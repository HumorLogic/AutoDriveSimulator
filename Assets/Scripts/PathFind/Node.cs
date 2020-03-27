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
        public NodeGrid Grid { get; private set; }
        public int Row { get; private set; }
        public int Col { get; private set; }
        public Vector2 Pos { get; private set; }
        public bool IsStepped { get; set; }
        public bool IsMarked { get; set; }
        public string Name { get; private set; }
        public float dirAngle { get;  set; }  //Node direction angle
        public Vector3 State { get; set;}
        public int gValue { get; set; }

        public int hValue { get; set; }
        public int fValue { get; set; }
        public GameObject nodeObj { get; private set; }

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
            Grid = grid;
            Row = row;
            Col = col;
            NodeType = type;
            Pos = new Vector2(Row, Col);
            Name = $"Node({Row},{Col})";
            IsStepped = false;
            IsMarked = false;
            dirAngle = Mathf.PI / 2;

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
            nodeObj.transform.position = new Vector3(Col, -Row, 0);
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
            nodeObj.GetComponentInChildren<Text>().text = gValue.ToString();
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
