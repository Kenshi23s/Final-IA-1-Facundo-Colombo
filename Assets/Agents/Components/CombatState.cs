using UnityEngine;

public class CombatState : IState
{
    StateMachine _fsm;
    Movement _move;
    CombatComponent combat;
    FOVAgent fov;
    FireteamManager myFireteam;
    float atkCD;
    float time;
    bool canAttack;
    Vector3 lastPos;

    public CombatState(StateMachine fsm, Movement move, CombatComponent combat, FOVAgent fov, FireteamManager myFireteam, float atkCD)
    {
        _fsm = fsm;
        _move = move;
        this.combat = combat;
        this.fov = fov;
        this.myFireteam = myFireteam;
        this.atkCD = atkCD;
        time = atkCD;
        canAttack = true;
        
        lastPos= new Vector3(0,0,0);
    }

    public void OnEnter()
    {
      
        if (!fov.SomeoneInFov(myFireteam.hostileAgents.ToArray()))
        {
            _fsm.ChangeState(EnumStates.EFreeRoam);
        }
    }

    public void OnUpdate()
    {
        OnCombat();
        MakeDecision();
        if (!canAttack)
        {
            CanAttackCd();
        }
       

    }

    void CanAttackCd() 
    {

        time -= Time.deltaTime;
        time = Mathf.Clamp(time, 0, atkCD);
       
        if (time == 0)
        {       
           canAttack = true;
           time= atkCD;
        }
        
    }

    void OnCombat()
    {

        Agent agentTarget = fov.NearestInFOV(myFireteam.hostileAgents.ToArray());
        if (agentTarget != null && canAttack)
        {
            combat.AttackTargetRange(agentTarget.transform);
            lastPos = agentTarget.transform.position;

            canAttack = false;
            time = atkCD;
        }
    }


    public void OnExit()
    {
        canAttack = true;
        time = atkCD;
    }

    public void MakeDecision()
    {
        if (fov.inFOV(lastPos) && Vector3.Distance(_move._transform.position, lastPos) > 10f)
        {
            _move.AddForce(_move.Seek(lastPos));
        }
        else
        {
            OnEnter();
        }
    }

    public void GizmoState()
    {
      
    }

}
