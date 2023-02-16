
using System.Collections.Generic;
using UnityEngine;
using FacundoColomboMethods;


public class Agent : MonoBehaviour
{


    FireteamManager myFireteam;
    StateMachine my_fsm;
    FOVAgent my_Fov;

    public bool isLeader;
    
        //set
        //{
        //    if (myFireteam != null)
        //    {              
        //        foreach (Agent item in myFireteam.fireteamMembers)
        //        {
        //            if (item.isLeader == true)
        //            {
        //                isLeader = false;
        //                return;
        //            }
        //        }
        //        isLeader = value;
        //    }
        //}
        //get
        //{
        //    return isLeader;
        //}
    



    #region Pathfind
    PathFiniding my_Pathfind;

    List<Vector3> path = new List<Vector3>();
    #endregion

    #region CombatVariables
    CombatComponent my_Combat;
    [Header("Movement")]
    public Transform shootingPivot;
    public int maxBulletSpeed=16;
    public Bullet _bullet;
    public float attackCd;
    #endregion

    #region MovementVariables
    Movement my_Movement;
    [Header("Movement")]

    public float maxMovementSpeed;
    public float maxMovementForce; 
    public Vector3 ActualVelocity => my_Movement._velocity;
    #endregion

    #region HealthVariables
    public Health Agent_Health;
    [Header("Health")]

    public float maxHealth;
    float health;
    public float actualHealth => Agent_Health.actualHealth;
    public float healingSpeed;
   
    #endregion

    [Header("Skin")]

    public Material mat;

    public EnumStates actual;


    //se llama a initialize en el awake
    public void InitializeAgent(FireteamManager myFiretam,Color color)
    {
        mat = GetComponent<Renderer>().material;
        mat.SetColor("_Color", color);

        AgentsManager.instance.SubscribeToList(this);

        this.myFireteam = myFiretam;

        CreateComponents();

        CreateStates();
        
    }

    void CreateComponents()
    {
        my_Movement = new Movement(new Vector3(0, 0, 0), maxMovementSpeed, this.transform, maxMovementForce, myFireteam);

        Agent_Health = new Health(health, maxHealth, healingSpeed, Respawn, ChangeToHeal);

        my_Pathfind = new PathFiniding(my_Movement, path);

        my_Combat = new CombatComponent(_bullet, shootingPivot, maxBulletSpeed,this.gameObject.layer);

        my_Fov = new FOVAgent(transform, AgentsManager.instance.wallMask);
    }

    void CreateStates()
    {
        my_fsm = new StateMachine();

        my_fsm.CreateState(EnumStates.EFreeRoam, CheckIfLeader());

        my_fsm.CreateState(EnumStates.EHealState, new LowLifeState(my_fsm, myFireteam, my_Pathfind, my_Movement, Agent_Health.IsMaxHealth));

        my_fsm.CreateState(EnumStates.EPursuitState, new CombatState(my_fsm,my_Movement,my_Combat,my_Fov,myFireteam, attackCd));

        my_fsm.ChangeState(EnumStates.EFreeRoam);
    }

    IState CheckIfLeader()
    {   
        if (isLeader)
        {            
            return new FreeRoamState(my_fsm, my_Movement,my_Pathfind, myFireteam, EnemiesOnFov);
        }
        else
        {          
            return new FollowLeaderState(my_fsm, my_Movement,my_Pathfind ,myFireteam.fireTeamLeader, EnemiesOnFov);
        }

        //return isLeader? new FreeRoamState(my_StateMachine, my_Movement, myFireteam, path, FollowPathSeek, CalculatePath, EnemiesOnFov)

        //: new FollowLeaderState(my_StateMachine, myFireteam, my_Movement, path, FollowPathArrive, CalculatePath, EnemiesOnFov);
    }


    bool EnemiesOnFov()
    {
        foreach (Agent hostile in myFireteam.hostileAgents)
        {
            if (my_Fov.inFOV(hostile.transform.position))
            {
                Debug.Log("devuelvo true");
                return true;
            }
                                 
        }
        Debug.Log("devuelvo false");
        return false;
    }

    void Update() => my_fsm?.Execute();

    #region StateChanges

    void ChangeToHeal() => my_fsm.ChangeState(EnumStates.EHealState);

    public void NewPosOrders()
    {

        if (my_fsm.actualStateLecture != EnumStates.EHealState)
        {
            //Debug.Log("cambia a freeRoam");
            my_fsm.ChangeState(EnumStates.EFreeRoam);
        }

    }

    #endregion

    #region HealthMethods
    public void TakeDamage(int dmg) => Agent_Health.SubstractLife(dmg);

    public void HealthRegeneration() => Agent_Health.RegenHealth();

    //void FullHealth() => my_StateMachine.ChangeState(EnumStates.EFreeRoam);

    void Respawn()
    {
        Agent_Health.SetMaxHealth();
        transform.position = myFireteam.FireteamHealZones[Random.Range(0,2)].transform.position;
        my_fsm.ChangeState(EnumStates.EFreeRoam);
    }
    #endregion

    private void OnDrawGizmos()
    {

       
        if (my_fsm!=null)
        {
            my_fsm?.StateGizmos();
        }

        if (my_Movement != null)
        {
            my_Movement.MovementGizmos();
        }
        if (my_Fov!=null)
        {
            my_Fov.FovDrawGizmos();
        }
       

    }

    void PathGizmos(List<Vector3> path)
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, AgentsManager.instance.nodeInteractradius);
        if (path != null)
        {
            for (int i = 1; i < path.Count; i++)
            {
                Gizmos.color = Color.grey;
                Gizmos.DrawLine(path[i - 1], path[i]);
                Gizmos.DrawWireSphere(path[i], AgentsManager.instance.nodeInteractradius);
            }
           
        }
    }

    private void LateUpdate()
    {
        actual = my_fsm.actualStateLecture;
    }


    //void PathFinding(Vector3 endpos) => actualPath = CalculatePath(endpos);
}
