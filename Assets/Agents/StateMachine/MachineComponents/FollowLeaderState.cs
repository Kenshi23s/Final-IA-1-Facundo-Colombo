using System;
using System.Collections.Generic;
using UnityEngine;

public class FollowLeaderState : IState
{
   
    Movement _movement;
    StateMachine _fsm;
    Agent leader;

    Action actualMethod;

    PathFiniding _pathFinding;
    Func<bool> enemyOnSigth;

    public FollowLeaderState(StateMachine _fsm,Movement _movement, PathFiniding _pathFinding, Agent _leader, Func<bool> enemyOnSigth)
    {
        this._movement = _movement;
        this._fsm = _fsm;
        this.leader = _leader;
        this._pathFinding = _pathFinding;
        this.enemyOnSigth = enemyOnSigth;
    }

    public void OnEnter()
    {
        _pathFinding.Clear();

        if  (!LeaderOnSigth())
        {
            Debug.Log("calculo el camino y voy hacia donde esta el lider");

            _pathFinding.SetPath(leader.transform.position, FollowLeader,PathFiniding.MovementType.Arrive);
            actualMethod = _pathFinding.PlayPath;
        }
        else
        {
            Debug.Log("sigo al lider");
            actualMethod = FollowLeader;
        }
      
    }

   

    public void OnUpdate()
    {
        actualMethod?.Invoke();
        MakeDecision();
    }
   
    

    void FollowLeader()
    {
        Vector3 actualForce = Vector3.zero;      
        if (LeaderOnSigth())
        {
            if (Vector3.Distance(_movement._transform.position, leader.transform.position) > AgentsManager.instance.viewRadius)
            {
                actualForce += _movement.Arrive(leader.transform.position);
            
                _movement.AddForce(actualForce);

            }
            else
            {
                _movement.FlockingUpdate();
            }
        }
        

    }

    

    bool LeaderOnSigth()=> Pathfinding_Algorithm.InLineOffSight(_movement._transform.position, leader.transform.position, AgentsManager.instance.wallMask);

    public void OnExit() => _pathFinding.Clear();


    public void GizmoState()
    {
     
        Gizmos.color= Color.cyan;
        Gizmos.DrawWireSphere(_movement._transform.position, AgentsManager.instance.viewRadius);
    }

    public void MakeDecision()
    {
        if (enemyOnSigth.Invoke())
            _fsm.ChangeState(EnumStates.EPursuitState);

        if (actualMethod != FollowLeader && LeaderOnSigth())
        {
            actualMethod = FollowLeader;
        }
        else if (actualMethod == FollowLeader && !LeaderOnSigth())
        {
            _pathFinding.SetPath(leader.transform.position, FollowLeader, PathFiniding.MovementType.Arrive);
            actualMethod = _pathFinding.PlayPath;

        }


    }
}
