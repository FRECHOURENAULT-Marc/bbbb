using System;
using NaughtyAttributes;
using UnityEngine;

public enum LIVING_TEAM
{
    Player,
    Enemies,
    Other
}

public class LivingScript : MonoBehaviour
{
    [SerializeField] private LIVING_TEAM _team;
    [SerializeField] private float _maxLife;
    [ShowNonSerializedField] private float _life;
    
    // parameter = void
    public event Action OnDamaged;
    public event Action OnDeath;
    // <parameters ...>
    public event Action<string> OnDamagedAndDisplayMessage;
    // <parameters, ... , returnType>
    public Func<int, int, double> MultiplierFunction;
    
    // Properties
    public LIVING_TEAM LivingTeam
    {
        get => _team;
        private set => _team = value;
    }
    public float MaxLife
    {
        private set => _maxLife = value;
        get => _maxLife;
    }
    
    public float Life
    {
        set => _life = value;
        get => _life;
    }

    public bool IsAlive
    {
        get => Life > 0;
    }

    // Methods
    void Reset()
    {
        LivingTeam = LIVING_TEAM.Other;
        MaxLife = 5f;
        Life = MaxLife;
    }

    void Start()
    {
        Life = MaxLife;
    }
    
    public void DealDamage(float damage)
    {
        if(IsAlive == false) return;
        
        Life -= damage;
        OnDamaged?.Invoke();
        Debug.Log($"{name} taken {damage} damage");

        if(IsAlive) return;
        
        OnDeath?.Invoke();
    }

    public void Respawn()
    {
        Life = MaxLife;
    }
    
    
}
