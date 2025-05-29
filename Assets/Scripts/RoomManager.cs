using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviour
{
    // 0: norte
    // 1: leste
    // 2: sul
    // 3: oeste
    public Dictionary<string, Dictionary<int, string>> rooms = new Dictionary<string, Dictionary<int, string>>();
    public Dictionary<string, Dictionary<string, bool>> roomObjects = new Dictionary<string, Dictionary<string, bool>>() {{"inicial", new Dictionary<string, bool>()}};
    public List<string> roomsList = new List<string>() { "room01", "room2", "room03", "room04", "room05", "room06", "room07", "room08", "room09", "room10",
    "room11", "room12", "room13", "room14", "room15", "room16", "room17", "room18", "room19", "room20"
    };

    public void AddRoom(string currentRoom, int position, string nextRoom) {
        if (rooms.ContainsKey(currentRoom)) {
            rooms[currentRoom].Add(position, nextRoom);
        } else {
            rooms.Add(currentRoom, new Dictionary<int, string>() {{position, nextRoom}});
        }
        
        if (rooms.ContainsKey(nextRoom)) {
            rooms[nextRoom].Add((position + 2) % 4, currentRoom);
        } else {
            rooms.Add(nextRoom, new Dictionary<int, string>() {{(position + 2) % 4, currentRoom}});
        }
    }

    void Update()
    {
        string currentRoom = SceneManager.GetSceneAt(1).name;
        if (!currentRoom.Equals("Lobby")) {
            Dictionary<string, bool> objetos = roomObjects[currentRoom];
            foreach (var (key, value) in objetos) {
                GameObject.Find(key)?.SetActive(value);
            }
        }
    }
}
