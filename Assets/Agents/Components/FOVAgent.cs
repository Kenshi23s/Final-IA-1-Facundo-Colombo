using UnityEngine;
using FacundoColomboMethods;
using System.Collections.Generic;

public class FOVAgent
{
    public FOVAgent(Transform _myTransform,LayerMask mask)
    {
        this._myTransform = _myTransform;
        this.mask=mask;
    }
    Transform _myTransform;
    public float viewRadius => AgentsManager.instance.fovRadius;
    public float viewAngle => AgentsManager.instance.fovAngle;
    LayerMask mask;


    public bool inFOV(Vector3 obj)
    {
        Vector3 dir = obj - _myTransform.position;

        if (dir.magnitude <= viewRadius)
        {
            if (Vector3.Angle(_myTransform.forward, dir) <= viewAngle / 2)
            {
                return ColomboMethods.InLineOffSight(_myTransform.position, obj, mask);
            }
        }

        return false;
    }

    public bool SomeoneInFov<T>(T[] obj) where T: MonoBehaviour
    {
        foreach (var item in obj)
        {
            Vector3 dir = item.transform.position - _myTransform.position;

            if (dir.magnitude <= viewRadius)
            {
                if (Vector3.Angle(_myTransform.forward, dir) <= viewAngle / 2)
                {
                    return ColomboMethods.InLineOffSight(_myTransform.position, item.transform.position, mask);
                }
            }
        }
       

        return false;
    }

    public T NearestInFOV<T>(T[] items) where T : MonoBehaviour 
    {
        List<T> Seeing = new List<T>(); 
        foreach (T item in items)
        {
            if (inFOV(item.transform.position))
            {
                Seeing.Add(item);
            }

         
        }
        return ColomboMethods.GetNearest<T>(items, _myTransform.position);
        
    }

    public void FovDrawGizmos()
    {

        if (AgentsManager.instance.GizmosFov)
        {
            Gizmos.color = Color.white;

            Gizmos.DrawWireSphere(_myTransform.position, viewRadius);

            Vector3 lineA = GetVectorFromAngle(viewAngle / 2 + _myTransform.eulerAngles.y);
            Vector3 lineB = GetVectorFromAngle(-viewAngle / 2 + _myTransform.eulerAngles.y);

            Gizmos.DrawLine(_myTransform.position, _myTransform.position + lineA * viewRadius);
            Gizmos.DrawLine(_myTransform.position, _myTransform.position + lineB * viewRadius);
            Gizmos.DrawLine(_myTransform.position, _myTransform.position + lineB * viewRadius);
        }
       
    }

    Vector3 GetVectorFromAngle(float angle) => new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    

}
