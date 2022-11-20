using UnityEngine;

public class EntityManager : MonoBehaviour
{
    [SerializeField] private DpsEntity _assassinEntity;
    [SerializeField] private TankEntity _spyEntity;
    [SerializeField] private SupportEntity _utilityEntity;
    
    private void Start()
    {
        _assassinEntity = new DpsEntity(_assassinEntity.EntityType, initialPosition: Vector3.zero, name: "Yor");
        _spyEntity = new TankEntity(_spyEntity.EntityType, initialPosition: Vector3.zero, name: "Loid");
        _utilityEntity = new SupportEntity(_utilityEntity.EntityType, initialPosition: Vector3.zero, name: "Anya");
    }

    private void Update()
    {
        //Debugging
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _spyEntity.Apply(ApplyType.PrimaryDamage, _assassinEntity);
            // _assassinEntity.Apply(ApplyType.Heal, _utilityEntity);
        }
    }
}