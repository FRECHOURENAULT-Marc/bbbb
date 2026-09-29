using System;
using NaughtyAttributes;
using UnityEngine;

public class LifeScript : MonoBehaviour
{
    public enum GO_TEAM
    {
        Player,
        Enemies,
        Other
    }
    
    [SerializeField] private GO_TEAM _team;
    [SerializeField] private float _maxLife;
    [ShowNonSerializedField] private float _life;
    
    public event Action OnDamaged;
    public event Action OnDeath;
    
    public GO_TEAM GoTeam
    {
        get => _team;
        private set => _team = value;
    }
    public float MaxLife
    {
        get => _maxLife;
        private set => _maxLife = value;
    }
    public float Life
    {
        get => _life;
        private set => _life = value;
    }
    public bool IsAlive
    {
        get => Life > 0;
    }
    
    void Reset()
    {
        GoTeam = GO_TEAM.Other;
        MaxLife = 5f;
        Life = MaxLife;
    }

    void Start()
    {
        Life = MaxLife;
    }
    
    public void ApplyDamage(float damage)
    {
        if(IsAlive == false) return;
        
        Life -= damage;
        OnDamaged?.Invoke();
        
        Debug.Log($"[LifeScript] Dealing {damage} damage");

        if(IsAlive) return;
        
        OnDeath?.Invoke();
    }

    public void Respawn()
    {
        Life = MaxLife;
    }
}
