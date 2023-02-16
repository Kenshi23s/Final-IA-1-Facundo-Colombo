using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement 
{
    FireteamManager myFireteam;
    public Vector3 _velocity;
    public Transform _transform;
    float _maxSpeed;
    float _maxForce;
    bool _isMoving;

    public Movement(Vector3 velocity, float maxSpeed, Transform transform, float maxForce, FireteamManager myFireteam)
    {
        _velocity = velocity;
        _maxSpeed = maxSpeed;
        _transform = transform;
        _maxForce = maxForce;
        this.myFireteam = myFireteam;
    }

    #region Movement Updates
    public void FlockingUpdate()
    {
        if (_isMoving==false)
        {
            Vector3 actualforce = Vector3.zero;
            Debug.Log("Flocking");

            actualforce += Alignment() * AgentsManager.instance.AlignmentForce;
            actualforce += Cohesion() * AgentsManager.instance.CohesionForce;
            actualforce += Separation() * AgentsManager.instance.SeparationForce;

            AddForce(actualforce);
        }
       

    }

    Vector3 ObstacleAvoidance(Transform transform)
    {
        
        float dist = _velocity.magnitude;
      
        if (Physics.SphereCast(transform.position, 1, transform.forward , out RaycastHit hit, dist, AgentsManager.instance.obstacleMask))
        {
            Vector3 obstacle = hit.transform.position;
            Vector3 dirToObject = obstacle - transform.position;
            float angleInBetween = Vector3.SignedAngle(transform.forward, dirToObject, Vector3.up);

            Vector3 desired = angleInBetween >= 0 ? -transform.right : transform.right;
         
            return CalculateSteering(desired);
        }

        return Vector3.zero;
    }

    public void AddForce(Vector3 force)
    {
        force+= ObstacleAvoidance(_transform);
        _velocity += force;
        _velocity = Vector3.ClampMagnitude(_velocity, _maxSpeed);
        _transform.position += _velocity * Time.deltaTime;
        _transform.forward = _velocity.normalized;


    }
    #endregion

    #region Flocking
    Vector3 Alignment()
    {
        Vector3 desired = Vector3.zero;
        int count = 0;
        foreach (Agent item in myFireteam.fireteamMembers)
        {
            if (item.ActualVelocity.GetHashCode() == this._velocity.GetHashCode())
                continue;

            Vector3 dist = item.transform.position - _transform.position;

            if (dist.magnitude <= AgentsManager.instance.viewRadius)
            {
                desired += item.ActualVelocity;
                count++;
            }
        }

        if (count <= 0)
            return desired;

        desired /= count;

        desired.Normalize();
        desired *= _maxForce;

        return CalculateSteering(desired);
    }

    Vector3 Cohesion()
    {
        Vector3 desired = Vector3.zero;
        int count = 0;

        foreach (var item in myFireteam.fireteamMembers)
        {
            if (item.ActualVelocity.GetHashCode() == this._velocity.GetHashCode())
                continue;

            Vector3 dist = item.transform.position - _transform.position;

            if (dist.magnitude <= AgentsManager.instance.viewRadius)
            {
                desired += item.transform.position;
                count++;
            }
        }

        if (count <= 0)
            return desired;

        desired /= count;
        desired -= _transform.position;

        desired.Normalize();
        desired *= _maxForce;

        return CalculateSteering(desired);
    }

    Vector3 Separation()
    {
        Vector3 desired = Vector3.zero;

        foreach (var item in myFireteam.fireteamMembers)
        {
            if (item.ActualVelocity.GetHashCode() == this._velocity.GetHashCode())
                            continue;
            
            Vector3 dist = item.transform.position - _transform.position;

            if (dist.magnitude <= AgentsManager.instance.viewRadius)
                desired += dist;
        }

        if (desired == Vector3.zero)
            return desired;

        desired = -desired;

        desired.Normalize();
        desired *= _maxForce;

        return CalculateSteering(desired);
    }
    #endregion

    #region MovementTypes
    public Vector3 Seek(Vector3 targetSeek)
    {
        _isMoving=true;
        Vector3 desired = targetSeek - _transform.position;
        desired.Normalize();
        desired *= _maxForce;

        Vector3 steering = desired - _velocity;

        steering = Vector3.ClampMagnitude(steering, _maxSpeed);
        _isMoving = false;
        return steering;
    }
    /// <summary>
    /// Este metodo se usa para Evadir x objetivo en base a una posicion que tendra(el objetivo) en el futuro.
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    //girar hacia
    //Vector3 SteerTo(Vector3 desiredPos)
    //{
    //    desiredPos.Normalize();
    //    desiredPos *= _maxSpeed;
    //    Vector3 steering = desiredPos - _velocity;
    //    return steering;
    //}

    public Vector3 Arrive(Vector3 actualTarget)
    {
        _isMoving = true;
        Debug.Log("Arrive");
        Vector3 desired = actualTarget - _transform.position;
        float dist = desired.magnitude;
        desired.Normalize();
        if (dist <= AgentsManager.instance.ArriveRadius)
            desired *= _maxSpeed * (dist / AgentsManager.instance.ArriveRadius);
        else
            desired *= _maxSpeed;

        //Steering
        Vector3 steering = desired - _velocity;
        _isMoving=false;
        return steering;
    }

    #endregion

    Vector3 CalculateSteering(Vector3 desired) => Vector3.ClampMagnitude(desired - _velocity, _maxSpeed);

    public void MovementGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(_transform.position, _transform.position + _velocity);

    }

    public void MovementDebug(string message)
    {
       

    }
}
