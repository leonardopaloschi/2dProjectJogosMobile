using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomTeleport : MonoBehaviour
{

    [SerializeField] private int direction;
    private Vector3 posititionToTeleport;

    void Awake()
    {
        Random.InitState((int) Time.time);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            StartCoroutine(LoadScene());

            collision.transform.position = posititionToTeleport;

            // GameObject[] zombieTrails = GameObject.FindGameObjectsWithTag("ZombieTrail");
            // foreach (GameObject obj in zombieTrails)
            // {
            //     obj.SetActive(false);
            // }
        }
    }

    private IEnumerator LoadScene() {
        GameObject roomManager = GameObject.FindGameObjectWithTag("RoomManager");
        RoomManager rm = roomManager.GetComponent<RoomManager>();
        
        string roomToLoad;
        string currentRoom = SceneManager.GetSceneAt(1).name;
        if (rm.rooms.ContainsKey(currentRoom)) {
            if (rm.rooms[currentRoom].ContainsKey(direction)) {
                roomToLoad = rm.rooms[currentRoom][direction];
            } else {
                roomToLoad = rm.roomsList[Random.Range(0, rm.roomsList.Count)];
                rm.AddRoom(currentRoom, direction, roomToLoad);
                rm.roomsList.Remove(roomToLoad);
            }
        } else {
            roomToLoad = rm.roomsList[Random.Range(0, rm.roomsList.Count)];
            rm.AddRoom(currentRoom, direction, roomToLoad);
            rm.roomsList.Remove(roomToLoad);
        }

        if (!rm.roomObjects.ContainsKey(roomToLoad)) {
            rm.roomObjects.Add(roomToLoad, new Dictionary<string, bool>());
        }

        if (direction == 0) {
            posititionToTeleport = new Vector3(0f, -3.5f, 0f);
        } else if (direction == 1) {
            posititionToTeleport = new Vector3(-7f, 0f, 0f);
        } else if (direction == 2) {
            posititionToTeleport = new Vector3(0f, 3.5f, 0f);
        } else if (direction == 3) {
            posititionToTeleport = new Vector3(7f, 0f, 0f);
        }

        AsyncOperation unload = SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(1).name);
        AsyncOperation load = SceneManager.LoadSceneAsync(roomToLoad, LoadSceneMode.Additive);
        while (!unload.isDone || !load.isDone) {
            yield return null;
        }
    }
}
