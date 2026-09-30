using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Parade : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] float _recoveryTime;
    [SerializeField] float _paradeTime;
    
    float _recoveryTimer;
    float _paradeTimer;
    
    [Header("Dependencies")]
    [SerializeField] LifeScript _life;
    
    [Header("Debug")] // Debug
    [SerializeField] InputActionReference _paradeInput;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Reset()
    {
        _recoveryTime = 1.0f;
    }

    public async void StopAttack()
    {
        if (_recoveryTimer < _recoveryTime)
        {
            Debug.Log("Recovering... (" +  _recoveryTimer/_recoveryTime*100.0f + "%)");
            return;
        }
        // Ready !

        _recoveryTimer = 0;

        _life.IsInvincible = true;
        
        float startTime = Time.time;
        while (_paradeTimer < _paradeTime)
        {
            await Awaitable.NextFrameAsync();
            _paradeTimer = Time.time - startTime;
        }
        
        _life.IsInvincible = false;
    }
    
    void Start()
    {
        //Debug
        if (_paradeInput)
            _paradeInput.action.started += (context) => StopAttack();  
    }
    
    // Update is called once per frame
    void Update()
    {
        if ( _life.IsInvincible == false)
            _recoveryTimer += Time.deltaTime;
    }
}
