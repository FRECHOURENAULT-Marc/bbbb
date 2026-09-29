using UnityEngine;

public class GameObjectDestroyer : MonoBehaviour
{
    [SerializeField] float destroyTime;
    [SerializeField] bool destroyOnExit;
    
    float timer;

    void Reset()
    {
        destroyTime = 3f;
        destroyOnExit = true;
    }
    
    void Start()
    {
        timer = 0.0f;
    }
    
    void Update()
    {
        timer += Time.deltaTime;
        if (timer < destroyTime)
            return;

        Destroy(this);
        if (destroyOnExit == false) 
            return;
        Destroy(gameObject);
    }
}
