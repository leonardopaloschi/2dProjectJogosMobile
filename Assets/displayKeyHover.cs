using UnityEngine;

public class displayKeyHover : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject eKeyHover;

    public Collider2D interactionRange;
    void Start()
    {
        interactionRange = GetComponent<Collider2D>();
        interactionRange.isTrigger = true;


        // Find the eKeyHover game object in the scene
        eKeyHover = GameObject.Find("eKeyHover");
        if (eKeyHover == null)
        {
            Debug.LogError("eKeyHover game object not found in the scene.");
        }
        // Disable the eKeyHover game object at the start
        if (eKeyHover != null)
        {
            eKeyHover.SetActive(false);
        }


    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Enable the "eKeyHover" game object
            if (eKeyHover != null)
            {
                eKeyHover.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Disable the "eKeyHover" game object
            if (eKeyHover != null)
            {
                eKeyHover.SetActive(false);
            }
        }
    }
}
