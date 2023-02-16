using UnityEngine;
using FacundoColomboMethods;
using System.Collections.Generic;
using System.Linq;


public class FireteamManager : MonoBehaviour
{

    public HealingZones[] FireteamHealZones;

    public List<Agent> fireteamMembers = new List<Agent>();
    public List<Agent> hostileAgents = new List<Agent>();

  

    public Agent fireTeamLeader;

    public Color _leaderColor, _member;
    public Camera cam;
   
    
   
    [SerializeField]
    public KeyCode orderButton;

    Vector3 posToGo;
    public Vector3 WaypointToGo => posToGo;

    public void SubscribeToFireteam(Agent member)=> fireteamMembers.Add(member);

    public void UnSubscribeOfFireteam(Agent member) => fireteamMembers.Remove(member);

   
    private void Awake()
    {

        Debug.LogError("El " + this.gameObject.name + " se mueve con la tecla " + orderButton.ToString()
            +" y segun donde tengas puesto el mouse en el momento, es recomendable activar los gizmos in game");
        if (cam==null)
        {
            cam = Camera.main;
        }
           
    }
     
    public void InitializeFireteam()
    {
      
        fireteamMembers = ColomboMethods.GetChildrenComponents<Agent>(this.transform).ToList();
        fireTeamLeader = DeclareLeader();

        posToGo = fireTeamLeader.transform.position;

        for (int i = 0; i < fireteamMembers.Count; i++)
        {
            if (fireteamMembers[i] != fireTeamLeader)
            {
                fireteamMembers[i].name = "agent " + (i + 1).ToString();
                fireteamMembers[i].isLeader = false;
                fireteamMembers[i].InitializeAgent(this, _member);
                fireteamMembers[i].gameObject.layer = gameObject.layer;
            }
        }
    }

    Agent DeclareLeader()
    {
        Agent leader = fireteamMembers[Random.Range(0, fireteamMembers.Count)];
        leader.isLeader = true;
        leader.name = leader.name + " Fireteam Leader";
        leader.InitializeAgent(this, _leaderColor);
        leader.gameObject.layer=gameObject.layer;
        return leader;
    }

    private void Update()
    {
        if (Input.GetKey(orderButton))
            SetNewCordinates();

        if (Input.GetKeyDown(KeyCode.K))
        {

            Health health = fireteamMembers[Random.Range(0, fireteamMembers.Count)].Agent_Health;
         

            health.SubstractLife(health.actualHealth/2);
           

        }
    }

    void SetNewCordinates()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
           //el 11 en y es pq las cosas estan 11 unidades mas arriba del 0,0,0
            CallAgentsToRoam(new Vector3(hit.point.x,11,hit.point.z));
          
            return;
        }
        Debug.Log("no choque con nada");

    }

    void CallAgentsToRoam(Vector3 NewPos)
    {
        posToGo = NewPos;       
        foreach (Agent member in fireteamMembers)
        {
            member.NewPosOrders();
        }
    }

    public void SetHostiles(List<Agent> hostiles) => hostileAgents = hostiles;

    private void OnDrawGizmos()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(new Vector3(hit.point.x,11,hit.point.z), 2f);
           
        }
        
        Gizmos.color = _leaderColor;
        Gizmos.DrawWireSphere(posToGo, 2f);
    }







}
