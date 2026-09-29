using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class AsyncTest : MonoBehaviour
{
    [SerializeField] float _waitTime;
    [SerializeField] InputActionReference _stopInput;
    CancellationTokenSource _cts;

    void Reset()
    {
        _waitTime = 2.0f;
    }

    void Start()
    {
        _cts = new CancellationTokenSource();
        _stopInput.action.started += StopAwaitable;
        PrintLog(_cts);

        
    }

    void OnDestroy()
    {
        _cts.Cancel();
    }

    void StopAwaitable(InputAction.CallbackContext context)
    {
        _cts.Cancel();
        Debug.Log("Requested: "+ _cts.Token.IsCancellationRequested);
    }
    
    // same as Awaitable<void>
    async void PrintLog(CancellationTokenSource cts)
    {
        while (cts.IsCancellationRequested == false)
        {
            //Compute string
            string str = await PrintLogStr(cts);
            Debug.Log(str);
        }
    }
    async Awaitable<string> PrintLogStr(CancellationTokenSource cts)
    {
        float startTime = Time.time;
        await Awaitable.WaitForSecondsAsync(_waitTime, cts.Token);
        float endTime = Time.time;
        float waitTime = endTime - startTime;
        return "Time waited: " + waitTime + " (start: " + startTime + " end: " + endTime + ")";
        
    }
}
