using System.Collections.Generic;
using UnityEngine;
using FacundoColomboMethods;
using System.Linq;

public class Node : MonoBehaviour
{
    
   
    public List<Node> Neighbors=new List<Node>();
    public int cost=0;
    [SerializeField]
  
  
    
    public void IntializeNode()
    {
        //argumentos
        Neighbors = ColomboMethods.GetWhichAreOnSight(NodeManager.instance.Nodes.ToList(),
        transform.position,
        RaycastType.Sphere,NodeManager.instance.wallMask,
        NodeManager.instance.radiusRead);
      
        foreach (Node item in Neighbors)
        {
            if (item == this)
            {
                Neighbors.Remove(item);
                return;
            }
        }
    }

    private void OnDrawGizmos()
    {
       foreach (Node node in Neighbors)
       {
          foreach (Node neighbor in node.Neighbors)
          {
              if (neighbor == this)
              {
                  Gizmos.color = Color.blue;
                  Gizmos.DrawLine(node.transform.position, transform.position);
              }
       
              else
              {
                  Gizmos.color = Color.cyan;
              }
       
          }
       }
        
        
        
        
        //Gizmos.DrawWireSphere(transform.position, AgentsManager.instance.nodeInteractradius);
    }
  
}
