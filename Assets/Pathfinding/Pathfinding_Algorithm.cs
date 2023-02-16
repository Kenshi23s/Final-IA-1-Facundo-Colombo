using System.Collections.Generic;
using UnityEngine;
//todos los algoritmos visto en la materia de IA 1
//los nodos deben tener de nombre de clase "Node" y las siguientes variables con los siguientes nombres:

// int cost = 1;
// public List<Node> Neighbors = new List<Node>();

//clase creada por Facundo Colombo
public static class Pathfinding_Algorithm
{
    public static List<Vector3> CalculateBFS(Node startingNode, Node goalNode)
    {
        List<Vector3> result = new List<Vector3>();

        Queue<Node> frontier = new Queue<Node>();
        frontier.Enqueue(startingNode);

        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        cameFrom.Add(startingNode, null);

        while (frontier.Count > 0)
        {
            Node current = frontier.Dequeue();

            if (current == goalNode)
            {


                while (current != startingNode)
                {
                    result.Add(current.transform.position);
                    current = cameFrom[current];
                }

                result.Reverse();
                return result;
            }

            foreach (var next in current.Neighbors)
            {
                if (!cameFrom.ContainsKey(next))
                {
                    frontier.Enqueue(next);
                    cameFrom.Add(next, current);
                }
            }
        }

        return new List<Vector3>();
    }

    public static List<Vector3> CalculateDijkstra(Node startingNode, Node goalNode)
    {
        PriorityQueue<Node> frontier = new PriorityQueue<Node>();
        frontier.Enqueue(startingNode, 0);

        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        cameFrom.Add(startingNode, null);

        Dictionary<Node, int> costSoFar = new Dictionary<Node, int>();
        costSoFar.Add(startingNode, 0);

        while (frontier.Count > 0)
        {
            Node current = frontier.Dequeue();

            if (current == goalNode)
            {
                List<Vector3> path = new List<Vector3>();

                while (current != startingNode)
                {
                    path.Add(current.transform.position);
                    current = cameFrom[current];
                }

                path.Reverse();
                return path;
            }

            foreach (var next in current.Neighbors)
            {
                

                int newCost = costSoFar[current] + next.cost;

                if (!costSoFar.ContainsKey(next))
                {
                    frontier.Enqueue(next, newCost);
                    cameFrom.Add(next, current);
                    costSoFar.Add(next, newCost);
                }
                else if (costSoFar[next] > newCost)
                {
                    frontier.Enqueue(next, newCost);
                    cameFrom[next] = current;
                    costSoFar[next] = newCost;
                }

            }
        }

        return new List<Vector3>();
    }

    public static List<Vector3> CalculateGreedyBFS(Node startingNode, Node goalNode)
    {
        List<Vector3> result = new List<Vector3>();
        PriorityQueue<Node> frontier = new PriorityQueue<Node>();
        frontier.Enqueue(startingNode, 0);

        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        cameFrom.Add(startingNode, null);

        while (frontier.Count > 0)
        {
            Node current = frontier.Dequeue();

            if (current == goalNode)
            {
                while (current != startingNode)
                {
                    result.Add(current.transform.position);
                    current = cameFrom[current];
                }
                return result;

              
            }

            foreach (var next in current.Neighbors)
            {
                if (!cameFrom.ContainsKey(next))
                {
                    float priority = Vector3.Distance(next.transform.position, goalNode.transform.position);
                    frontier.Enqueue(next, priority);
                    cameFrom.Add(next, current);
                }
            }
        }
        return new List<Vector3>();

    }

    public static List<Vector3> CalculateAStar(Node startingNode, Node goalNode)
    {
        //el vecino del nodo

        PriorityQueue<Node> frontier = new PriorityQueue<Node>();
        frontier.Enqueue(startingNode, 0f);

        //de donde vino, mi key es el nodo siguiente y el value es el nodo actual

        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        cameFrom.Add(startingNode, null);
        //el costo de cada nodo y cuanto costo lleva acumulando el camino
        Dictionary<Node, float> costSoFar = new Dictionary<Node, float>();
        costSoFar.Add(startingNode, 0f);

        while (frontier.Count > 0)
        {
            Node current = frontier.Dequeue();

            if (current == goalNode)
            {
                List<Vector3> path = new List<Vector3>();


                while (current != startingNode)
                {
                    path.Add(current.transform.position);
                    current = cameFrom[current];
                }

                path.Reverse();
                path.Add(goalNode.transform.position);
                path.Insert(0, startingNode.transform.position);
                return path;
            }

            foreach (Node next in current.Neighbors)
            {

                float newCost = costSoFar[current] + next.cost;
                float priority = newCost + Vector3.Distance(next.transform.position, goalNode.transform.position);

                if (!costSoFar.ContainsKey(next))
                {
                    frontier.Enqueue(next, priority);
                    cameFrom.Add(next, current);
                    costSoFar.Add(next, newCost);
                }

                else if (costSoFar[next] > newCost)
                {
                    frontier.Enqueue(next, priority);
                    cameFrom[next] = current;
                    costSoFar[next] = newCost;
                }

            }
        }

        return new List<Vector3>();
    }

    public static List<Vector3> CalculateThetaStar(Node startingNode, Node goalNode,LayerMask WallMask,Vector3 endpos=default)
    {
        List<Vector3> PathList = CalculateAStar(startingNode, goalNode);
        if (endpos!=Vector3.zero)
        {
            PathList.Add(endpos);
        }
        int current = 0;

        while (current + 2 < PathList.Count)
        {      
            if (InLineOffSight(PathList[current], PathList[current + 2], WallMask))
            {              
                PathList.RemoveAt(current + 1);
            }
            else
                current++;
        }
        
        return PathList;


    }

   public static bool InLineOffSight(Vector3 InitialPos, Vector3 finalPos, LayerMask maskWall)
   {
        Vector3 dir = finalPos - InitialPos;
        return !Physics.Raycast(InitialPos, dir, dir.magnitude, maskWall);
   }
}


public class PriorityQueue<T>
{
    Dictionary<T, float> ElementsDictionary = new Dictionary<T, float>();

    public int Count => ElementsDictionary.Count;
     

   

    // Update is called once per frame
    public void Enqueue(T elem, float Value)
    {
        if (!ElementsDictionary.ContainsKey(elem)&&elem!=null)
        {
            ElementsDictionary.Add(elem, Value);

        }
    }


    public T Dequeue()
    {
        float lowestValue = float.MaxValue;

        T elem = default(T);
        foreach (var item in ElementsDictionary)
        {
            if (lowestValue > item.Value)
            {
                elem = item.Key;
                lowestValue = item.Value;
            }

        }
        ElementsDictionary.Remove(elem);

        return elem;
    }
}
