using UnityEngine;
using FacundoColomboMethods;
using System.Collections;

public class NodeManager : MonoBehaviour
{
    public static NodeManager instance;

    public LayerMask wallMask;
    public Node[] Nodes;
 

    [SerializeField]
    float sphereCasstRadius;
    public float radiusRead => sphereCasstRadius;

    void Awake()
    {
        instance = this;
        InitializePaths();
    }
    void InitializePaths()
    {
        Application.targetFrameRate = 144;

        Nodes = ColomboMethods.GetChildrenComponents<Node>(this.transform);

        for (int i = 0; i < Nodes.Length; i++)
        {
            Nodes[i].name = "Node" + i;
            Nodes[i].IntializeNode();

           
        }
    }
  
    public Node NearestNode(Vector3 pos) => ColomboMethods.GetNearest(Nodes, pos);

    private void OnDrawGizmos()
    {
       
    }

}
