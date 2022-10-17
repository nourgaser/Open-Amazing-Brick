using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource jump;
    [SerializeField]
    private AudioSource passLevel;
    [SerializeField]
    private AudioSource collision;


    // Start is called before the first frame update
    void Start()
    {

        PlayerController.playerJumped += () =>
        {
            jump.Play();
        };

        GameManager.levelPassed += () =>
        {
            passLevel.Play();
        };

        PlayerController.playerCollided += () =>
        {
            collision.Play();
        };
    }
}
