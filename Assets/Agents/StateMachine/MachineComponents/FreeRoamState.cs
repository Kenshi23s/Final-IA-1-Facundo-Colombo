using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FreeRoamState : IState
{
    Movement _movement;
    FireteamManager _myFireteam;
    StateMachine _fsm;
  
    Func<bool> _enemyOnSigth;
    PathFiniding _pathFinding;
  
    

    Action actualMethod;
    
    bool finished;

    public FreeRoamState(StateMachine fsm, Movement movement, PathFiniding pathFinding, FireteamManager myFireteam,Func<bool> enemyOnSigth)
    {
        _movement = movement;
        _myFireteam = myFireteam;
        _fsm = fsm;
        _enemyOnSigth = enemyOnSigth;
        _pathFinding = pathFinding;
    }

    public void OnEnter()
    {
        finished = false;

        if (Pathfinding_Algorithm.InLineOffSight(_movement._transform.position, _myFireteam.WaypointToGo, AgentsManager.instance.wallMask))
        {
            //Debug.Log("el punto ESTA en la mira, voy directo"); 
            actualMethod = GoToPoint;
        }
        else
        {
            //Debug.Log("no veo el punto, calculo el camino");

            _pathFinding.SetPath(_myFireteam.WaypointToGo, GoToPoint, PathFiniding.MovementType.Seek);
            _pathFinding.SetLastPoint(_myFireteam.WaypointToGo);

            actualMethod = _pathFinding.PlayPath;        
          
        }
    }

    public void OnUpdate()
    {
        if(finished != true)
        actualMethod?.Invoke();
        MakeDecision();

    }

    public void OnExit() => _pathFinding.Clear();
    

    void GoToPoint()
    {
       
        float distance = Vector3.Distance(_myFireteam.WaypointToGo, _movement._transform.position);
        if (distance > AgentsManager.instance.nodeInteractradius)
        {
            Vector3 actualForce = Vector3.zero;

            actualForce += _movement.Seek(_myFireteam.WaypointToGo);
            _movement.AddForce(actualForce);
        }
        else
        {
            finished = true;
        }
        
    
    }



    public void MakeDecision()
    {
        if (_enemyOnSigth.Invoke())
        {
            Debug.Log("cambio a pursuit, estaba en free roam");
            _fsm.ChangeState(EnumStates.EPursuitState);
        }
           
    }

    public void GizmoState()
    {
      
     
        
    }

}
