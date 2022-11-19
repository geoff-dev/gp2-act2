using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

[System.Serializable]
public class Entity :  IHealthStatusHandler
{
    [Header("Entity Data")]
    [SerializeField] private string _name;
    [SerializeField] private EntityType _entityType;
    private float _moveSpeed; 
    private Vector3 _currentPosition;

    public bool IsAlive => CurrentHealth > 0;
     public float ExpToGive { get; private set; }
    private float _health;

    public float CurrentHealth
    {
        get => _health;
        set => _health = Mathf.Clamp(value, 0, MaxHealth + ShieldAmount);
    }
    public float MaxHealth { get; private set; }
    public float ShieldAmount { get; private set; }
    public float BuffTimer { get; private set; }
    public float PrimaryDamage { get; private set; } = 10f;
    public float SkillDamage { get; private set; } = 20f;

    private readonly Vector2 _constantExperience = new Vector2(0.07f, 2f);
    private float _experience;

    public float CurrentExperience
    {
        get => _experience;
        set
        {
            if (!(_experience >= RequireExpToLevel())) return;
            _level++;
            _experience = 0;
        }
    }
    private int _level;

    private float RequireExpToLevel()
    {
        return _level / _constantExperience.x * Mathf.Exp(_constantExperience.y);
    }

    public Entity(EntityType entityType, Vector3 initialPosition, float primaryDamage = 25f, float skillDamage = 50f, float expToGive = 50f, string name = "Unnamed Entity", float maxHealth = 100.0f)
    {
        _entityType = entityType;
        _name = name;
        MaxHealth = maxHealth;
        _health = MaxHealth;
        _currentPosition = initialPosition;
        ExpToGive = expToGive;
        PrimaryDamage = primaryDamage;
        SkillDamage = skillDamage;
    }

    public virtual void UpdateEntity()
    {
        
    }
    
    protected virtual void Navigate(Vector3 destination, float deltaTime)
    {
        var step = _moveSpeed * deltaTime;
        _currentPosition = Vector3.MoveTowards(_currentPosition, destination, step);
    }

    public virtual void Apply(ApplyType type, IHealthStatusHandler agent)
    {
        switch (type)
        {
            case ApplyType.None:
                break;
            case ApplyType.PrimaryDamage:
                CurrentHealth -= agent.PrimaryDamage;
                break;
            case ApplyType.SkillDamage:
                CurrentHealth -= agent.SkillDamage;
                break;
            case ApplyType.Heal:
                CurrentHealth += agent.SkillDamage;
                break;
            case ApplyType.Buff:
                BuffTimer = agent.BuffTimer;
                break;
            case ApplyType.Shield:
                CurrentHealth += ShieldAmount;
                break;
        }
        
        if(!IsAlive)
            OnDie(agent);
    }

    public virtual void OnDie(IHealthStatusHandler agent)
    {
        CurrentExperience += agent.ExpToGive;
        Debug.Log("LEVEL UP");
    }
}

public interface IHealthStatusHandler
{
    bool IsAlive { get; }
    float ExpToGive { get; }
    float CurrentHealth { get; }
    float MaxHealth { get; }
    float ShieldAmount { get; }
    float BuffTimer { get; }
    float PrimaryDamage { get; }
    float SkillDamage { get; }
    void Apply(ApplyType type, IHealthStatusHandler agent);
    void OnDie(IHealthStatusHandler agent);
}

public enum ApplyType
{
    None,
    PrimaryDamage,
    Heal,
    Buff,
    Shield,
    SkillDamage
}

public enum EntityType
{
    Unassigned,
    Dps,
    Tank,
    Support
}