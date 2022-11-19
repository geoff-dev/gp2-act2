using System;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    //[SerializeField] private Entity[] _entities;
    [SerializeField] private DpsEntity _assassinEntity;
    private void Start()
    {
        _assassinEntity = new DpsEntity(entityType: EntityType.Dps, initialPosition: Vector3.zero, name: "Mei");
    }

    private void Update()
    {
        //debugging
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _assassinEntity.Apply(ApplyType.PrimaryDamage, _assassinEntity);
            Debug.Log(_assassinEntity.CurrentHealth);
            Debug.Log($"Entity Life Status: {_assassinEntity.IsAlive}");
            Debug.Log($"Entity Exp: {_assassinEntity.CurrentExperience}");
        }
    }
}