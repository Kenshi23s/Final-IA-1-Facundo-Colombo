using System.Collections.Generic;
using UnityEngine;

public enum EnumStates
{
    EHealState,
    EFreeRoam,
    EPursuitState
}
public class StateMachine : MonoBehaviour
{
    IState _currentState;

    Dictionary<EnumStates, IState> _statesList = new Dictionary<EnumStates, IState>();

    EnumStates _actualState=EnumStates.EFreeRoam;
    public EnumStates actualStateLecture=>_actualState;




    public void CreateState(EnumStates name, IState state)
    {
        // si en mi diccionario no tengo esa llave 
        if (!_statesList.ContainsKey(name))
            //la creo
            _statesList.Add(name, state);
    }

    public void Execute()
    {
        _currentState.OnUpdate();
    }

    public void ChangeState(EnumStates name)
    {
        if (_statesList.ContainsKey(name))
        {
            _actualState = name;
            if (_currentState != null)
            {
                _currentState.OnExit();
            }

            _currentState = _statesList[name];
            _currentState.OnEnter();
            
        }

    }
    public void StateGizmos()
    {
        _currentState.GizmoState();
    }
}

