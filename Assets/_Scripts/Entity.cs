using UnityEngine;

[System.Serializable]
public class Entity :  IHealthStatusHandler
{
    [Header("Entity Data")] 
    [SerializeField] protected string _name;
    [SerializeField] protected EntityType _entityType;
    protected float _moveSpeed; 
    private Vector3 _currentPosition;
    protected bool _activateNavigateSkill = false;

    public bool IsAlive => CurrentHealth > 0;
    public float ExpToGive { get; private set; }
    private float _health;

    public float CurrentHealth
    {
        get => _health;
        private set => _health = Mathf.Clamp(value, 0, MaxHealth + ShieldAmount);
    }
    public float MaxHealth { get; private set; }
    public float ShieldAmount { get; private set; }
    public float BuffTime { get; private set; }
    public float PrimaryDamage { get; private set; }
    public float SkillDamage { get; private set; }

    private readonly Vector2 _constantExperience = new Vector2(0.07f, 2f);
    private float _experience;
    public float CurrentExperience
    {
        get => _experience;
        private set
        {
            if (!(_experience >= RequireExpToLevel()))
            {
                _experience = value;
                return;
            }
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
        if (!IsAlive) return;
        
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
                BuffTime = agent.BuffTime;
                break;
            case ApplyType.Shield:
                CurrentHealth += ShieldAmount;
                break;
        }
        
        if(!IsAlive)
            OnDie(this,agent);
    }

    public virtual void OnDie(IHealthStatusHandler agent, IHealthStatusHandler recipient)
    {
        recipient.AccumulateExperience(agent);
    }

    public virtual void AccumulateExperience(IHealthStatusHandler agent)
    {
        CurrentExperience += agent.ExpToGive;
    }
}

public interface IHealthStatusHandler
{
    bool IsAlive { get; }
    float CurrentExperience { get; }
    float ExpToGive { get; }
    float CurrentHealth { get; }
    float MaxHealth { get; }
    float ShieldAmount { get; }
    float BuffTime { get; }
    float PrimaryDamage { get; }
    float SkillDamage { get; }
    void Apply(ApplyType type, IHealthStatusHandler agent);
    void OnDie(IHealthStatusHandler agent, IHealthStatusHandler recipient);
    void AccumulateExperience(IHealthStatusHandler agent);
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