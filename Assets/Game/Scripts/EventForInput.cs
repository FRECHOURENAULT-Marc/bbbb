using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class EventForInput : MonoBehaviour
{
    [SerializeField] InputActionReference _input;
    [SerializeField] UnityEvent _onStart;
    [SerializeField] UnityEvent _onPerformed;
    [SerializeField] UnityEvent _onCanceled;

    void Start()
    {
        if (_input == null)
        {
            Debug.LogError("EventForInput has no InputActionReference set");
            Destroy(this);
            return;
        }
        
        _input.action.started += (context) => _onStart?.Invoke();
        _input.action.performed += (context) => _onPerformed?.Invoke();
        _input.action.canceled += (context) => _onCanceled?.Invoke();
    }
}
