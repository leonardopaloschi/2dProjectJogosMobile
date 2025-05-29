using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifetime = 5f;
    public int damage = 1;

    private void Start()
    {
        // Destroi o projetil após um tempo caso não colida com nada
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Health playerHealth = collision.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TomarDano(damage);
            }
            Destroy(gameObject); // Destroi a bolinha após atingir o jogador
        }
        else if (!collision.isTrigger)
        {
            // Colidiu com parede ou objeto físico — destrói o projetil
            Destroy(gameObject);
        }
    }
}
