using UnityEngine;

[System.Serializable]
public class SupportEntity : Entity
{
    public EntityType EntityType => EntityType.Support;
    private float _moveSpeedBoost = 3f;
    private float _boostTime = 2f;
    private float _boostReset = 2f;


    public SupportEntity(EntityType entityType, Vector3 initialPosition, float primaryDamage = 25,
        float skillDamage = 50, float expToGive = 50, string name = "Unnamed Entity", float maxHealth = 100) : base(
        entityType, initialPosition, primaryDamage, skillDamage, expToGive, name, maxHealth)
    {
    }

    public override void OnDie(IHealthStatusHandler agent, IHealthStatusHandler recipient)
    {
        base.OnDie(agent, recipient);
        OnDieApplyCurse();
    }

    private void OnDieApplyCurse()
    {
        Debug.Log($"{_name} applied curse on last breath!");
    }

    protected override void Navigate(Vector3 destination, float deltaTime)
    {
        base.Navigate(destination, deltaTime);

        if (_activateNavigateSkill)
        {
            _moveSpeed *= _moveSpeedBoost;
        }
    }

    public override void UpdateEntity()
    {
        base.UpdateEntity();

        if (!_activateNavigateSkill) return;
        
        _boostTime -= Time.deltaTime;
        if (_boostTime <= 0)
        {
            _activateNavigateSkill = false;
            _boostTime = _boostReset;
        }
    }
}