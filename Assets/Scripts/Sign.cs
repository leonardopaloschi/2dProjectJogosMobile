using UnityEngine;

public class Sign : MonoBehaviour
{
    [SerializeField] private GameObject text;

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            text.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            text.SetActive(false);
        }
    }
}
