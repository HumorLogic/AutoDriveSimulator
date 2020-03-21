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
    }

    /// <summary>
    /// Search_PFA class
    /// </summary>
    public class Search_PFA : PathFindAlgorithm
    {
        
        public Search_PFA(NodeGrid nodeGrid) : base(nodeGrid)
        {
            nodeList = new List<Node>();
            pathNodes = new List<Node>();
            //pathDic = new Dictionary<Node, Vector3>();
        }

        
        public override async void DoStep()
        {
            VisualStep visual = new VisualStep(Grid);
            Node current = Grid.startNode;
            nodeList.Add(current);
            current.IsMarked = true;
            if (Grid.displayCost)
                current.DisplayCost();

            while (!Grid.desNode.IsMarked)
            {
                current = ExtractMini();
                var nextNodes = GetNeighbors(current);
                foreach (var item in nextNodes)
                {
                    if (!item.IsMarked && item.NodeType != NodeType.Obsticle)
                    {
                        item.IsMarked = true;
                        await Task.Delay(Grid.delayTime);
                        visual.MarkNode(item);
                        nodeList.Add(item);

                        if (Grid.displayCost)
                            item.DisplayCost();
                    }
                }
                current.IsStepped = true;
                visual.MarkNodeStepped(current);
               
            }

            GetPath(Grid.startNode, Grid.desNode);
            visual.ColorPath(pathNodes);
        }

        public override void GetPath(Node start,Node des)
        {
            //pathNodes = new List<Node>();
            Vector3 state = des.State;
            Node node;
            for (int i = 0; i < des.gValue; i++)
            {
                node = GetNode(state);
                state -= motionDic[node];
                pathNodes.Add(node);
            }
        }
    }
}

