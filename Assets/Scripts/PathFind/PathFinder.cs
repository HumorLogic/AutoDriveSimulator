using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace AutoDriveSimulator
{
    public class PathFinder
    {

    }

    public class Search_PFA : PathFindAlgorithm
    {

        public Search_PFA(NodeGrid nodeGrid) : base(nodeGrid)
        {

        }
        public override void DoStep()
        {
            Vector3 currentState=new Vector3(Grid.startPosition.x,Grid.startPosition.y,0);
            Node current = GetNode(currentState);

            var nodes = GetNeighbors(current);
            


        }
    }
}

