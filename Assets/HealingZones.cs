using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingZones : MonoBehaviour
{
    [SerializeField]
    List<Agent> _injuredAgents = new List<Agent>();

    private void OnTriggerEnter(Collider other)
    {
        var agent =  other.GetComponent<Agent>();
        string message = "";
        if (agent!=null)
        {
            message += " el agente no es null ";
         
            if (!_injuredAgents.Contains(agent))
            {
                message += " y no lo tenia asi que lo agrego";
                _injuredAgents.Add(agent);
            }
            else
            {
                message += " ya lo tenia asi q no lo agrego";
            }

        }
        else
        {
            message += "el agente es null";
        }
        
        Debug.Log(message);
    }

    private void OnTriggerStay(Collider other)
    {
        if (_injuredAgents.Count>=1)
        {
            foreach (var item in _injuredAgents)
            {
                item.HealthRegeneration();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var agent = other.GetComponent<Agent>();

        if (agent != null)
        {
            if (_injuredAgents.Contains(agent))
            {
                _injuredAgents.Remove(agent);
            }

        }
    }




}
