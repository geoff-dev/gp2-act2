using UnityEngine;

[System.Serializable]
public class DpsEntity : Entity
{
    public DpsEntity(EntityType entityType, Vector3 initialPosition, float primaryDamage = 25, float skillDamage = 50, float expToGive = 50, string name = "Unnamed Entity", float maxHealth = 100) : base(entityType, initialPosition, primaryDamage, skillDamage, expToGive, name, maxHealth)
    {
    }
}