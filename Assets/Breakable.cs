using UnityEngine;
using UnityEngine.SceneManagement;

public class Breakable : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public int hitsToBreak = 2;
    public Animator animator;

    public Collider2D collider;

    public Rigidbody2D rb;

    public AudioClip breakSound;
    public AudioClip hitSound;
    public AudioSource audioSource;

    public
    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component is missing on the GameObject.");
        }

        collider = GetComponent<Collider2D>();
        if (collider == null)
        {
            Debug.LogError("Collider component is missing on the GameObject.");
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing on the GameObject.");
        }

        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component is missing on the GameObject.");
        }

    }


    public void TakeDamage()
    {
        hitsToBreak--;
        if (hitsToBreak <= 0)
        {
            Break();
        }
        else
        {
            if (hitSound != null)
            {
                audioSource.PlayOneShot(hitSound);
            }
            else
            {
                Debug.LogError("Hit sound not assigned.");
            }
            animator.SetTrigger("hit");
        }
    }

    private void Break()
    {
        if (breakSound != null)
        {
            audioSource.PlayOneShot(breakSound);
        }
        else
        {
            Debug.LogError("Break sound not assigned.");
        }
        animator.SetTrigger("break");
        collider.enabled = false;
        // Disable the collider to prevent further interactions

        StartCoroutine(MarkAsRemovedLater());
    }
    
    private System.Collections.IEnumerator MarkAsRemovedLater()
    {
        yield return new WaitForSeconds(0.84f); // Espera a animação terminar

        GameObject roomManager = GameObject.FindGameObjectWithTag("RoomManager");
        if (roomManager != null)
        {
            RoomManager rm = roomManager.GetComponent<RoomManager>();
            string currentRoom = SceneManager.GetSceneAt(1).name;
            rm.roomObjects[currentRoom].Add(gameObject.name, false);
        }
    }
}
