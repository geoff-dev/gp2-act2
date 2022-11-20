using UnityEngine;

[System.Serializable]
public class DpsEntity : Entity
{
    public EntityType EntityType => EntityType.Dps;
    private int _dashCount = 2;
    private int _maxDashCount = 2;
    private float _dashRefreshTime = 3f;
    private float _dashRefreshReset = 3f;

    public DpsEntity(EntityType entityType, Vector3 initialPosition, float primaryDamage = 25, float skillDamage = 50, float expToGive = 50, string name = "Unnamed Entity", float maxHealth = 100) : base(entityType, initialPosition, primaryDamage, skillDamage, expToGive, name, maxHealth)
    {
    }

    public override void OnDie(IHealthStatusHandler agent, IHealthStatusHandler recipient)
    {
        base.OnDie(agent, recipient);
        OnDieRage();
    }

    private void OnDieRage()
    {
        Debug.Log($"{_name} dying rage activate!");
    }

    protected override void Navigate(Vector3 destination, float deltaTime)
    {
        base.Navigate(destination, deltaTime);

        if (_activateNavigateSkill)
        {
            _dashCount--;
        }
    }
    
    public override void UpdateEntity()
    {
        base.UpdateEntity();

        if (_dashCount < _maxDashCount)
        {
            _dashRefreshTime -= Time.deltaTime;
            if (_dashRefreshTime <= 0)
            {
                _dashCount++;
                _dashRefreshTime = _dashRefreshReset;
            }
        }
    }
}