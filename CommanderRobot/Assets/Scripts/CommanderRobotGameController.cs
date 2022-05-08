using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommanderRobotGameController : MonoBehaviour
{
    [SerializeField] private fighterScript player;
    private keyboardInputManager inputManager;
    private AudioSource audioSource;

    private void Start()
    {
        inputManager = GameObject.FindGameObjectWithTag("inputManager").GetComponent<keyboardInputManager>();
        audioSource = GetComponent<AudioSource>();

        PrepareGame();
    }

    private void PrepareGame()
    {
        inputManager.selectPlayer(player.gameObject);

        PrepareLayers();

        Camera.main.GetComponent<cameraBehaviour>().changePlayer(player.gameObject);
    }

    private void PrepareLayers()
    {
        Physics2D.IgnoreLayerCollision(8, 12);
        Physics2D.IgnoreLayerCollision(9, 12);
        Physics2D.IgnoreLayerCollision(8, 13);
        Physics2D.IgnoreLayerCollision(9, 13);
        Physics2D.IgnoreLayerCollision(10, 13);
        Physics2D.IgnoreLayerCollision(11, 13);
        Physics2D.IgnoreLayerCollision(12, 13);
        Physics2D.IgnoreLayerCollision(15, 13);
        Physics2D.IgnoreLayerCollision(0, 13);
        Physics2D.IgnoreLayerCollision(10, 10);
    }

    // win screen actions

    // fail screen actions

    // move to next phase
}
