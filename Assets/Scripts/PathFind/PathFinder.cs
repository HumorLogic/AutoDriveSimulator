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
    public static class PathFinder
    {
        public static void DoSearch()
        {
            NodeGrid nodeGrid = GameObject.Find("Grid").GetComponent<NodeGrid>();
            Search_PFA search = new Search_PFA(nodeGrid);
            search.DoStep();
        }
    }

    public class Search_PFA : PathFindAlgorithm
    {

        public Search_PFA(NodeGrid nodeGrid) : base(nodeGrid)
        {
            nodeList = new List<Node>();
        }

        
        public override void DoStep()
        {
            VisualStep visual = new VisualStep(Grid);
            Node current = Grid.startNode;
            nodeList.Add(current);
            current.IsMarked = true;


            while (!Grid.desNode.IsMarked)
            {
                current = ExtractMini();
                var nextNodes = GetNeighbors(current);
                foreach (var item in nextNodes)
                {
                    if (!item.IsMarked && item.NodeType != NodeType.Obsticle)
                    {
                        
                        item.IsMarked = true;
                        visual.MarkNode(item);
                        nodeList.Add(item);
                    }
                }
                current.IsStepped = true;
                visual.MarkNode(current) ;
               
            }
                
                
            
            
                
            


        }
    }
}

