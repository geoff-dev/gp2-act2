using UnityEngine;

[System.Serializable]
public class TankEntity : Entity
{
    public EntityType EntityType => EntityType.Tank;

    private int _crowdControlEffect = 0;
    private int _maxCrowdControl = 3;
    private float _crowdControlCooldown = 3f;
    private float _crowdControlReset = 3f;
    
    public TankEntity(EntityType entityType, Vector3 initialPosition, float primaryDamage = 25, float skillDamage = 50, float expToGive = 50, string name = "Unnamed Entity", float maxHealth = 100) : base(entityType, initialPosition, primaryDamage, skillDamage, expToGive, name, maxHealth)
    {
    }

    public override void OnDie(IHealthStatusHandler agent, IHealthStatusHandler recipient)
    {
        base.OnDie(agent, recipient);
        OnDieExplode();
    }

    private void OnDieExplode()
    {
        Debug.Log($"{_name} explodes!");
    }

    protected override void Navigate(Vector3 destination, float deltaTime)
    {
        base.Navigate(destination, deltaTime);

        if (_activateNavigateSkill)
        {
            _crowdControlEffect++;
        }
    }

    public override void UpdateEntity()
    {
        base.UpdateEntity();

        if (_crowdControlEffect >= _maxCrowdControl)
        {
            _crowdControlCooldown -= Time.deltaTime;
            if (_crowdControlCooldown <= 0)
            {
                _crowdControlEffect = 0;
                _crowdControlCooldown = _crowdControlReset;
            }
        }
    }
}