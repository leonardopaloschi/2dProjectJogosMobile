using UnityEngine;

public class audioInicial : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        // Ensure that this GameObject is not destroyed when loading a new scene
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void destoySelf()
    {
        Destroy(gameObject);
    }
}
