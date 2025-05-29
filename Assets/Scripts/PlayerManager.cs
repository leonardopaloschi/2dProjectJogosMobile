using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        GameObject gm = GameObject.FindGameObjectWithTag("GameManager");
        GameObject playerPrefab = gm.GetComponent<GameManager>().playerPrefabSelected;
        Instantiate(playerPrefab, transform.position, Quaternion.identity);
    }
}
