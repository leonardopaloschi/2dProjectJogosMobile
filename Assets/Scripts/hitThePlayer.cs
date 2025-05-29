using UnityEngine;

public class hitThePlayer : MonoBehaviour
{
    public int meleeDamage = 10;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Collider2D collider = GetComponent<Collider2D>();
        if (collider == null)
        {
            Debug.LogError("Collider2D component is missing on the GameObject.");
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Health playerHealth = collision.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TomarDano(meleeDamage);
            }
            else
            {
                Debug.LogError("Health component is missing on the Player GameObject.");
                
            }
        }
    }
}
