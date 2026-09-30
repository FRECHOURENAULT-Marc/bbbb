using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class WeaponScript : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] float _damage;
    [SerializeField] float _timeBeforeDamage;
    [SerializeField] float _recoveryTime;
    [SerializeField] UnityEvent _onContact;

    float _recoveryTimer;
    float _timerBeforeDamage;
    List<LifeScript> _damagedEntities;
    
    [Header("Dependencies")]
    [SerializeField] ColliderEvent _collider;
    [SerializeField] LifeScript.GO_TEAM _team;
    [SerializeField] Animator _weaponAnimator;
    [SerializeField] string _animatorAttackTrigger;
    [SerializeField] string _animationStateAttack;
    
    [Header("Debug")] // Debug
    [SerializeField] InputActionReference _attackInput;

    void Reset()
    {
        _damage = 1.0f;
        _timeBeforeDamage = 0.25f;
        _recoveryTime = 1.0f;
        
        _animatorAttackTrigger = "Attack";
        _animationStateAttack = "Attack";
    }
    
    void Start()
    {
        // Test if dependencies are filled
        if(_collider == null || _weaponAnimator == null)
        {
            Debug.LogError("No collider or no _weaponAnimator defined");
            Destroy(this);
            return;
        }
        
        _damagedEntities = new List<LifeScript>();
        _collider.onTriggerStay += InflictDamage;
        
        //Debug
        if (_attackInput)
            _attackInput.action.started += (context) => Attack();
    }

    void InflictDamage(Collider other)
    {
        // Do not apply damage if _timerBeforeDamage is not complete
        if (_timerBeforeDamage < _timeBeforeDamage)
            return;
        
        // Be sure the target is a living entity
        if (other.TryGetComponent(out LifeScript life) == false)
            return;
        // Be sure the target is not damaged more than one time by the same attack
        if (_damagedEntities.Contains(life))
            return;
        // Be sure the target is not in the same team as the weapon
        if (life.GoTeam == _team)
            return;
        
        _onContact?.Invoke();
        life.ApplyDamage(_damage);
        _damagedEntities.Add(life);
    }

    public async void Attack()
    {
        if (_recoveryTimer < _recoveryTime)
        {
            Debug.Log("Recovering... (" +  _recoveryTimer/_recoveryTime*100.0f + "%)");
            return;
        }
        // Ready !
        
        //// Reset fields (_recoveryTimer & _damagedEntities)
        _damagedEntities.Clear();
        _recoveryTimer = 0;
        
        //// Handle Animation
        _weaponAnimator.SetTrigger(_animatorAttackTrigger);
        
        float startTime = Time.time;
        // Transition to attack animation : Wait until animation _animationStateAttack started
        while (
            _weaponAnimator.GetCurrentAnimatorStateInfo(0)
                .IsName(_animationStateAttack) == false)
        {
            await Awaitable.NextFrameAsync();
        }
        
        // Play attack animation : Wait until animation _animationStateAttack ended
        while (_weaponAnimator.GetCurrentAnimatorStateInfo(0)
                   .normalizedTime < 1f)
        {
            await Awaitable.NextFrameAsync();
            _timerBeforeDamage = Time.time - startTime;  // increase timer before attack
        }
        
        // End of animation (reset _timerBeforeDamage to let _recoveryTimer updating)
        _timerBeforeDamage = 0;
    }

    void Update()
    {
        // Do not update _recoveryTimer if attack is currently playing
        if (_timerBeforeDamage > 0.0f)
            return;
        
        _recoveryTimer += Time.deltaTime;
    }
}
