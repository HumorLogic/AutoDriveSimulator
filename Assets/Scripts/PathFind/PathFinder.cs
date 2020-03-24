/// Author : Humor Logic 雍
/// URL : http://www.humorlogic.com
/// Github : https://github.com/HumorLogic

#region Includes

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

#endregion


namespace AutoDriveSimulator
{
    public static class PathFinder
    {
        public static void DoSearch()
        {
            NodeGrid nodeGrid = GameObject.Find("Grid").GetComponent<NodeGrid>();
            Search_PFA search = new Search_PFA(nodeGrid);
            search.DoStep();

        }

        public static void AStarSearch()
        {
            NodeGrid nodeGrid = GameObject.Find("Grid").GetComponent<NodeGrid>();
            AStar search = new AStar(nodeGrid);
            search.GetHeuristic();
            search.DoStep();
            

        }
    }

    /// <summary>
    /// Search_PFA class
    /// </summary>
    public class Search_PFA : PathFindAlgorithm
    {
        
        public Search_PFA(NodeGrid nodeGrid) : base(nodeGrid)
        {
            openList = new List<Node>();
            pathNodes = new List<Node>();
        }

        /// <summary>
        /// Search Algorithm do step method
        /// </summary>
        public override async void DoStep()
        {
            VisualStep visual = new VisualStep(grid);
            Node current = grid.startNode;
            openList.Add(current);
            current.IsMarked = true;
            if (grid.displayCost)
                current.DisplayCost();

            while (!grid.desNode.IsMarked)
            {
                current = ExtractMini();
                var nextNodes = GetNeighbors(current);
                foreach (var item in nextNodes)
                {
                    if (!item.IsMarked && item.NodeType != NodeType.Obsticle)
                    {
                        item.IsMarked = true;
                        await Task.Delay(grid.delayTime);
                        visual.MarkNode(item);
                        openList.Add(item);

                        if (grid.displayCost)
                            item.DisplayCost();
                    }
                }
                current.IsStepped = true;
                visual.MarkSteppedNode(current);
               
            }

            GetPath(grid.startNode, grid.desNode);
            visual.ColorPath(pathNodes);
        }

        /// <summary>
        /// Get path node list
        /// </summary>
        /// <param name="start">start node</param>
        /// <param name="des">destination node</param>
        public override void GetPath(Node start,Node des)
        {
            Vector3 state = des.State;
            Node node = des;
           // int length = des.gValue - start.gValue;
            
            
            for (int i = 0; i < des.gValue; i++)
            {
                node = GetNode(state);
                state -= motionDic[node];
                pathNodes.Add(node);
            }
            //while (node !=start)
            //{
            //    node = GetNode(state);
            //    state -= motionDic[node];
            //    pathNodes.Add(node);
            //}
        }

    }

    public class AStar : PathFindAlgorithm
    {
        public AStar(NodeGrid nodeGrid) : base(nodeGrid)
        {
            openList = new List<Node>();
            pathNodes = new List<Node>();
        }

        public override void DoStep()
        {
            VisualStep visual = new VisualStep(grid);
            Node current = grid.startNode;
            openList.Add(current);
            current.IsMarked = true;

            while (!grid.desNode.IsStepped)
            {
                openList.Sort(new NodeASatrCompare());
                current = openList[0];
                openList.RemoveAt(0);

                var nextNodes = GetNeighbors(current);
                foreach (var item in nextNodes)
                {
                    if (!item.IsMarked && item.NodeType != NodeType.Obsticle)
                    {
                        item.IsMarked = true;
                        item.UpdateF();

                        visual.MarkNode(item);
                        openList.Add(item);

                        //if (grid.displayCost)
                        //item.DisplayHeuristic();
                    }
                }
                current.IsStepped = true;
                visual.MarkSteppedNode(current);
            }

            GetPath(grid.startNode, grid.desNode);
            visual.ColorPath(pathNodes);

        }

        public void GetHeuristic()
        {
          //  VisualStep visual = new VisualStep(grid);
            Node current = grid.desNode;
            openList.Add(current);
            current.IsMarked = true;

            while (openList.Count!=0)
            {
                current = ExtractMini();
                var nextNodes = GetNeighbors(current);
                foreach (var item in nextNodes)
                {
                    if (!item.IsMarked && item.NodeType != NodeType.Obsticle)
                    {
                        item.IsMarked = true;
                        item.hValue = item.gValue;
                       
                        //visual.MarkNode(item);
                        openList.Add(item);

                        //if (grid.displayCost)
                            //item.DisplayHeuristic();
                    }
                }
                current.IsStepped = true;
            }

            foreach (var item in grid.nodeDic)
            {
                item.Value.IsMarked = false;
                item.Value.IsStepped = false;
                item.Value.gValue = 0;
                item.Value.State = new Vector3(item.Value.State.x, item.Value.State.y, 0);
            }

        }



        public override void GetPath(Node start, Node des)
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
    }
}

