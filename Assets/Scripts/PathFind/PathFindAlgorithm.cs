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
    /// <summary>
    /// IPathFindAlgorithm Interface
    /// </summary>
    public interface IPathFindAlgotithm
    {
        void DoStep();
        IReadOnlyList<Node> GetNeighbors(Node node);
    }

    /// <summary>
    /// PathFindAlgorithm abstract class
    /// </summary>
    public abstract class PathFindAlgorithm : IPathFindAlgotithm
    {
        #region Members

        protected NodeGrid Grid;
        protected List<Node> nodeList;
        private Vector3[] motions = new Vector3[4] {new Vector3 (-1, 0, 1), new Vector3(1, 0, 1), new Vector3(0, -1, 1), new Vector3(0, 1, 1) };

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="nodeGrid">NodeGrid class object</param>
        protected PathFindAlgorithm(NodeGrid nodeGrid)
        {
            Grid = nodeGrid;
        }

        /// <summary>
        /// abstract method do step
        /// </summary>
        public abstract void DoStep();


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
                if (next.x >= 0 && next.x < Grid.rows && next.y >= 0 && next.y < Grid.cols)
                {
                    nextNode= GetNode(next);
                    list.Add(nextNode);
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
            nodeList.Sort(new NodeCompare());
            Node node = nodeList[0];
            nodeList.RemoveAt(0);
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
            Node node = Grid.nodeDic[key];
            node.State = v;
            node.gValue = (int)v.z;
            return node;
        } 
        
    }

    public interface IVisualStep
    {
        void MarkNode(Node node);
        void ColorPath(List<Node> nodes);
    }

    public  class VisualStep : IVisualStep
    {
        private NodeGrid nodeGrid;
        public VisualStep(NodeGrid grid)
        {
            nodeGrid = grid;
        }
        public  void MarkNode(Node node)
        {
            if (node.NodeType == NodeType.Normal)
            {
                if (node.IsMarked)
                    node.SetColor(nodeGrid.markColor);
                if (node.IsStepped)
                    node.SetColor(nodeGrid.steppedColor);
            }
           else return ;
        }
        public  void ColorPath(List<Node> nodes)
        {
            
        }

        
    }


}