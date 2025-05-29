using UnityEngine;

public class PlayerDeathAnimator : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        Animator anim = GetComponent<Animator>();
        anim.runtimeAnimatorController = gm.playerDeathAnimatorSelected;
    }
}
