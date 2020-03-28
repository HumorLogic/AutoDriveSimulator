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
                //foreach (var item in nextNodes)
                //{
                //    if (!item.IsMarked && item.NodeType != NodeType.Obsticle)
                //    {
                //        item.IsMarked = true;
                //        await Task.Delay(grid.delayTime);
                //        visual.MarkNode(item);
                //        openList.Add(item);

                //        if (grid.displayCost)
                //            item.DisplayCost();
                //    }
                //}
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
                current.DisplayCost();
                visual.MarkSteppedNode(current);
               
            }

            GetPath(grid.startNode, grid.desNode);
            visual.ColorPath(pathNodes);
        }

        
        

    }

    public class AStar : PathFindAlgorithm
    {
        public AStar(NodeGrid nodeGrid) : base(nodeGrid)
        {
            openList = new List<Node>();
            pathNodes = new List<Node>();
        }


        /// <summary>
        /// A* do step method
        /// </summary>
        public override async void DoStep()
        {
            isDiffCost = true;
            VisualStep visual = new VisualStep(grid);
            Node current = grid.startNode;
            openList.Add(current);
            current.IsMarked = true;
            if (grid.displayCost)
                current.DisplayCost();


            while (!grid.desNode.IsStepped)
            {
                openList.Sort(new NodeAStarCompare());
                current = openList[0];
                openList.RemoveAt(0);

                var nextNodes = GetNeighbors(current);
                foreach (var item in nextNodes)
                {
                    if (!item.IsMarked && item.NodeType != NodeType.Obsticle)
                    {
                        item.IsMarked = true;
                        item.UpdateF();
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

            isDiffCost = false;
        }


        /// <summary>
        /// Get Heuristic value
        /// </summary>
        public void GetHeuristic()
        {
            isHeuristic = true;
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
                item.Value.ResetNoed();
            }

            motionDic.Clear();
            isHeuristic = false;
        }


       
    }
}

