using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class AgentsManager : MonoBehaviour
{
   
    public static AgentsManager instance;
    public bool GizmosFov = false;
    [Header("Agent Stats")]
    [SerializeField] float _nodeInteractRadius = 5f;

    #region Fov
    [Header("Fov")]
    [SerializeField, Range(0, 200)] float _FovRadius = 5f;
    [SerializeField, Range(0, 90)] float _FovAngle = 5f;
    #endregion

    [SerializeField] float _viewRadius = 5f;

    #region Flocking
    [Header("Flocking")]

    [Header("Alignment")]

    [SerializeField, Range(0, 30)] float _AlignmentForce = 5f;
    [Header("Separation")]

    [SerializeField, Range(0, 30)] float _SeparationForce = 5f;
    [SerializeField] float _radiusSeparation = 5f;

    [Header("Cohesion")]

    [SerializeField, Range(0, 30)] float _CohesionForce = 5f;
    #endregion


    [Header("Arrive")]
    [SerializeField] float _arriveRadius = 5f;

    #region VariableGetters

    public float nodeInteractradius => _nodeInteractRadius;
    public float radiusSeparation => _radiusSeparation;
    public float viewRadius => _viewRadius;

    public float fovRadius => _FovRadius;
    public float fovAngle => _FovAngle;

    public float SeparationForce => _SeparationForce;
    public float CohesionForce => _CohesionForce;
    public float AlignmentForce => _AlignmentForce;
    public float ArriveRadius => _arriveRadius;

    #endregion

    public List<Agent> activeAgents = new List<Agent>();
    public List<FireteamManager> ActiveFireteams = new List<FireteamManager>();

    public LayerMask wallMask;

    public LayerMask obstacleMask;

    private void Awake()
    {
        instance = this;
        foreach (var _fireteam in ActiveFireteams)
        {
            _fireteam.InitializeFireteam();
        }
        //despues de q todos los fireteam se inicializaron, les seteo los enemigos
        foreach (var _fireteam in ActiveFireteams)
        {

            _fireteam.SetHostiles(GetEnemyAgents(_fireteam));
        }

    }



    public void SubscribeToList(Agent a) => activeAgents.Add(a);

    //private void OnDrawGizmos()
    //{
    //    foreach (var agent in activeAgents)
    //    {
    //        Gizmos.color = Color.cyan;
    //        Gizmos.DrawWireSphere(agent.transform.position, _viewRadius);
    //        Gizmos.color = Color.green;
    //        Gizmos.DrawWireSphere(agent.transform.position, SeparationForce);

    //    }
    //}

    public List<Agent> GetEnemyAgents(FireteamManager AllyFireteam)
    {
        List<FireteamManager> enemyFireteams = new List<FireteamManager>();
        List<Agent> enemyAgents = new List<Agent>();
        foreach (FireteamManager item in AgentsManager.instance.ActiveFireteams)
        {
            if (item != AllyFireteam)
            {
                enemyFireteams.Add(item);
            }

        }
        if (enemyFireteams.Count>=1)
        {
            foreach (FireteamManager teams in enemyFireteams)
            {
                foreach (var enemy in teams.fireteamMembers)
                {
                    enemyAgents.Add(enemy);
                }
            }
            return enemyAgents;
            
        }
        return new List<Agent>();

       
    }

}
