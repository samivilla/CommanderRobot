using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommanderRobotGameController : MonoBehaviour
{
    [SerializeField] private fighterScript player;
    private keyboardInputManager inputManager;
    private AudioSource audioSource;

    [SerializeField] private GameObject introText;

    private void Start()
    {
        inputManager = GameObject.FindGameObjectWithTag("inputManager").GetComponent<keyboardInputManager>();
        audioSource = GetComponent<AudioSource>();

        PrepareGame();

        StartCoroutine(IntroTextFade());

        ShowPlayerCharacter();
    }

    private void PrepareGame()
    {
        inputManager.selectPlayer(player.gameObject);
        player.isMainCharacter = true;
        player.underControl = true;

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

    private void ShowPlayerCharacter()
    {
        // instantiate character here

    }

    private IEnumerator IntroTextFade()
    {
        yield return new WaitForSeconds(3f);

        introText.SetActive(false);
    }

    public void ApplyDeath(GameObject dead)
    {
        if (dead.GetComponent<fighterScript>().isMainCharacter)
        {
            // go to fail screen
        }
        else if (dead.layer == 9)
        {
            // adjust the amount of killed enemies, if needed!
        }
    }

    // win screen actions

    // fail screen actions

    // move to next phase
}
