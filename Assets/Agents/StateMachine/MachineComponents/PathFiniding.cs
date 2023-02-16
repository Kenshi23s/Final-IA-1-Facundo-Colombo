using System;
using System.Collections.Generic;
using UnityEngine;

public class PathFiniding
{
    public enum MovementType
    {
        Seek,
        Arrive
    }
    public MovementType actualType;
    Func<Vector3,Vector3> movementType;
    Movement move;
    List<Vector3> actualPath = new List<Vector3>();
    Action onPathFinish;

    public PathFiniding(Movement move, List<Vector3> actualPath)
    {
        this.move = move;
        this.actualPath = actualPath;
    }

    public void SetPath(Vector3 endPos,Action onPathFinish,MovementType typeSelected = MovementType.Arrive)
    {
        this.onPathFinish = onPathFinish;

        movementType = SetMovementType(typeSelected);

        Node start = NodeManager.instance.NearestNode(move._transform.position);

        Node end = NodeManager.instance.NearestNode(endPos);

        actualPath= Pathfinding_Algorithm.CalculateThetaStar(start, end, NodeManager.instance.wallMask);
       

        //return Pathfinding_Algorithm.CalculateAStar(start, end);

    }
      
    public void SetLastPoint(Vector3 point) => actualPath.Add(point);   
    
       
    
    void FollowPath(Vector3 nextPoint)
    {
        Vector3 actualForce = Vector3.zero;
        actualForce += movementType.Invoke(nextPoint);
        move.AddForce(actualForce);
    }

    public void PlayPath()
    {

        if (actualPath.Count >= 1)
        {
            //Debug.Log("reproduzco el camino, me quedan  " + actualPath.Count + " nodos");
            FollowPath(actualPath[0]);
            float distance = Vector3.Distance(actualPath[0], move._transform.position);
            if (distance < AgentsManager.instance.nodeInteractradius)
            {
                actualPath.RemoveAt(0);
            }
         
        }
        else
        {
            actualPath.Clear();
            onPathFinish?.Invoke();
            onPathFinish = default;
        }
       


    }

    Func<Vector3, Vector3> SetMovementType(MovementType typeSelected)
    {
        switch (typeSelected)
        {
            case MovementType.Seek:
                return move.Seek;

            case MovementType.Arrive:
                return move.Arrive;

        }

        return move.Seek;

    }


    public void Clear()
    {
        actualPath.Clear();
        onPathFinish = default;
    }
}
