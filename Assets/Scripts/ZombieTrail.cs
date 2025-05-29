using UnityEngine;
using System.Collections.Generic;

public class ZombieTrail : MonoBehaviour
{
    public float duration = 5f;
    public int damage = 2;
    public float damageInterval = 0.5f;

    private Dictionary<GameObject, float> lastDamageTime = new Dictionary<GameObject, float>();

    void Start()
    {
        Destroy(gameObject, duration);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!lastDamageTime.ContainsKey(other.gameObject))
            {
                lastDamageTime[other.gameObject] = 0f;
            }

            float lastTime = lastDamageTime[other.gameObject];
            if (Time.time - lastTime >= damageInterval)
            {
                var health = other.GetComponent<Health>();
                if (health != null)
                {
                    health.TomarDano(damage);
                    lastDamageTime[other.gameObject] = Time.time;
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (lastDamageTime.ContainsKey(other.gameObject))
        {
            lastDamageTime.Remove(other.gameObject);
        }
    }
}
