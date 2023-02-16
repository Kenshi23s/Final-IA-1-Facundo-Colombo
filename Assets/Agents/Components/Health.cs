using System;
using UnityEngine;

public class Health
{


    float _health = 10;
    public float actualHealth => _health;
    float _maxHealth = 20;
    float _healSpeed=3;
    Action _dieAction;
    Action _needHeal;
   

    public Health(float _health, float _maxHealth, float _healSpeed, Action _dieAction, Action _needHeal)
    {
        this._health = _health;
        this._maxHealth = _maxHealth;
        this._healSpeed = _healSpeed;
        this._dieAction = _dieAction;
        this._needHeal = _needHeal;      
        this._health = this._maxHealth;
    }
    public bool IsMaxHealth()
    {
        if (actualHealth>=_maxHealth)
        {
            return true;

        }

        return false;
    }

    public void RegenHealth()
    {
       _health += Time.deltaTime * _healSpeed;
       _health  = Mathf.Clamp(_health, 0, _maxHealth);
        Debug.Log("tengo " + _health + " de vida, me faltan " + (_maxHealth - _health) + " de vida");

    }

    public void SubstractLife(float value)
    {
         
        _health -= value;
     
        Debug.Log(_maxHealth);
        _health = Mathf.Clamp(_health, 0, _maxHealth);
        Debug.Log(_health);
        if (_health <= _maxHealth/2)
        {
            Debug.Log("vida baja, requiero curaRME");
            _needHeal.Invoke();
            if (_health == 0)
            {
                _dieAction.Invoke();
            }
        }

        
    }
    internal void SetMaxHealth() => _health = _maxHealth;
  
}
