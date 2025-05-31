using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Collectable : MonoBehaviour
{
    private PowerUp pu;
    public List<string> possibleCollectables = new List<string> {
        "isAutomaticShotActive",
        "isKnockbackActive",
        "+0.5Speed",
        "+1Speed",
        "+1.5Speed",
    };
    public int coletavelIndex = 0;

    private GameObject player;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        pu = player.GetComponent<PowerUp>();
    }

    public void Collect()
    {
        pu.powerUpsAtivos[possibleCollectables[coletavelIndex]] = true;

        if (possibleCollectables[coletavelIndex].Equals("isKnockbackActive"))
        {
            pu.ApplyKnockback();
        }
        else if (possibleCollectables[coletavelIndex].Equals("+0.5Speed"))
        {
            PlayerMovement pm = player.GetComponent<PlayerMovement>();
            pm.maxSpeed += 0.5f;
            pm.speed = pm.maxSpeed;
        }
        else if (possibleCollectables[coletavelIndex].Equals("+1Speed"))
        {
            PlayerMovement pm = player.GetComponent<PlayerMovement>();
            pm.maxSpeed += 1;
            pm.speed = pm.maxSpeed;
        }
        else if (possibleCollectables[coletavelIndex].Equals("+1.5Speed"))
        {
            PlayerMovement pm = player.GetComponent<PlayerMovement>();
            pm.maxSpeed += 1.5f;
            pm.speed = pm.maxSpeed;
        }

        Destroy(gameObject);
    }
}
