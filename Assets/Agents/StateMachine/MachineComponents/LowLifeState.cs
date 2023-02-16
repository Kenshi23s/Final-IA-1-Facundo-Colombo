using System;
using UnityEngine;
using FacundoColomboMethods;

public class LowLifeState : IState
{
    
    FireteamManager myFireteam;
    PathFiniding pathFind;
    Movement move;
    StateMachine fsm;
  

    Vector3 nearestHealthZone;

    Func<bool> _isMaxHealth;
    Action method;

    public LowLifeState(StateMachine fsm, FireteamManager myFireteam, PathFiniding pathFind, Movement move,Func<bool> func)
    {
        this.myFireteam = myFireteam;
        this.pathFind = pathFind;
        this.move = move;
        this.fsm = fsm;
        _isMaxHealth = func;
    }

    public void OnEnter()
    {
        nearestHealthZone = ColomboMethods.GetNearest<HealingZones>(myFireteam.FireteamHealZones, move._transform.position)
                                          .transform.position;

        if (ColomboMethods.InLineOffSight(move._transform.position, nearestHealthZone,AgentsManager.instance.wallMask))
        {
            method = GoToHeal;
        }
        else
        {
            pathFind.SetPath(nearestHealthZone, move.FlockingUpdate, PathFiniding.MovementType.Arrive);
            method = pathFind.PlayPath;
        }
    }



    void GoToHeal()
    {
        Vector3 actualForce = Vector3.zero;
        if (Vector3.Distance(move._transform.position, nearestHealthZone)>2f)
        {
            actualForce += move.Arrive(nearestHealthZone);
            move.AddForce(actualForce);
        }
        else
        {
            move.FlockingUpdate();
        }
        
        
    }
    public void OnUpdate()
    {
        method?.Invoke();
        MakeDecision();
    }
    public void MakeDecision()
    {
        if (_isMaxHealth.Invoke())
        {
            Debug.Log("TENGO TODA LA VIDA, paso a free roam");
            fsm.ChangeState(EnumStates.EFreeRoam);
        }
        else
        {
            Debug.Log("no estoy full vida");
        }
        
    }


    public void OnExit()
    {
        pathFind.Clear();
        method = default;
    }
    
    public void GizmoState()
    {
        throw new System.NotImplementedException();
    }
}
