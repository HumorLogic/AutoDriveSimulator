﻿/// Author : Humor Logic 雍
/// URL : http://www.humorlogic.com
/// Github : https://github.com/HumorLogic

#region Includes

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#endregion

namespace AutoDriveSimulator
{
    #region IPathFindAlgorithm interface

    /// <summary>
    /// IPathFindAlgorithm Interface
    /// </summary>
    public interface IPathFindAlgotithm
    {
        void DoStep();
        void GetPath(Node start,Node des);
        IReadOnlyList<Node> GetNeighbors(Node node);
    }

    #endregion


    #region PathFindAlgorithm abstract class

    /// <summary>
    /// PathFindAlgorithm abstract class
    /// </summary>
    public abstract class PathFindAlgorithm : IPathFindAlgotithm
    {
        #region Members

        protected NodeGrid grid;
        protected List<Node> openList;
        protected Dictionary<Node, Vector3> motionDic;
        private Vector3[] motions = new Vector3[4] {new Vector3 (-1, 0, 1), new Vector3(1, 0, 1), new Vector3(0, -1, 1), new Vector3(0, 1, 1) };
        protected List<Node> pathNodes;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="nodeGrid">NodeGrid class object</param>
        protected PathFindAlgorithm(NodeGrid nodeGrid)
        {
            grid = nodeGrid;
            motionDic = new Dictionary<Node, Vector3>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// abstract method do step
        /// </summary>
        public abstract void DoStep();


        /// <summary>
        /// Get path node list
        /// </summary>
        /// <param name="start">start node</param>
        /// <param name="des">destination node</param>
        public void GetPath(Node start,Node des)
        {
            Vector3 state = des.State;
            Node node = des;

            for (int i = 0; i < des.gValue; i++)
            {
                node = GetNode(state);
                state -= motionDic[node];
                pathNodes.Add(node);
            }
        }


        /// <summary>
        /// Get node's neighbors
        /// </summary>
        /// <param name="node">Node object</param>
        /// <returns></returns>
        public IReadOnlyList<Node> GetNeighbors(Node node)
        {
            var list = new List<Node>();
            if (node.NodeType == NodeType.Obsticle)
            {
                return list;
            }

            Vector3 next;
            Node nextNode;
            for (int i = 0; i < motions.Length; i++)
            {
                next = node.State + motions[i];
                if (next.x >= 0 && next.x < grid.rows && next.y >= 0 && next.y < grid.cols)
                {
                    nextNode= GetNode(next);
                    list.Add(nextNode);
                    if (!motionDic.ContainsKey(nextNode))
                        motionDic.Add(nextNode, motions[i]);
                    
                }

            }

            return list;
        }


        /// <summary>
        /// Extract mini node in nodeList
        /// </summary>
        /// <returns></returns>
        protected Node ExtractMini()
        {
            openList.Sort(new NodeCompare());
            Node node = openList[0];
            openList.RemoveAt(0);
            return node;
        }


        /// <summary>
        /// Get node from dictionary
        /// </summary>
        /// <param name="v">node state</param>
        /// <returns></returns>
        protected Node GetNode(Vector3 v)
        {
            Vector2 key = new Vector2(v.x, v.y);
            Node node = grid.nodeDic[key];
            node.State = v;
            node.gValue = (int)v.z;
            return node;
        }

        #endregion

    }

    #endregion


    #region IVisualStep interface

    /// <summary>
    /// IVisualStep interface
    /// </summary>
    public interface IVisualStep
    {
        void MarkNode(Node node);
        void MarkSteppedNode(Node node);
        void ColorPath(List<Node> nodes);
    }

    #endregion


    #region VisualStep class

    /// <summary>
    /// VisualStep class
    /// </summary>
    public class VisualStep : IVisualStep
    {
        private NodeGrid nodeGrid;
        public VisualStep(NodeGrid grid)
        {
            nodeGrid = grid;
        }

        public  void MarkNode(Node node)
        {
            if (node.NodeType == NodeType.Normal&& node.IsMarked)
                node.SetColor(nodeGrid.markColor);
        }

        public void MarkSteppedNode(Node node)
        {
            if (node.IsStepped&&node.NodeType==NodeType.Normal)
                node.SetColor(nodeGrid.steppedColor);
        }
        
        public  void ColorPath(List<Node> nodes)
        {
            foreach (var item in nodes)
            {
                if(item.NodeType==NodeType.Normal)
                    item.SetColor(nodeGrid.pathColor);
            }
        }

        
    }

    #endregion

}
