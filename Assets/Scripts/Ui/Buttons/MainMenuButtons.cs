using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButtons : MonoBehaviour
{
    public GameObject MainMenuCanvas;
    public Player player;
    public GameObject StartScreen;
    public GameObject TutorialScreen;
    public GameObject OptionScreen;
    public GameObject VideoTutorial;
    public GameObject EvidenceTutorial;
    public GameObject ItemTutorial;
    private Vector3 startPos;

    void Start()
    {
        player.canMove = false;
        startPos = player.transform.position;
    }

    public void clickQuit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void playClick()
    {
        player.transform.position = startPos;
        player.canMove = !player.canMove;
        MainMenuCanvas.SetActive(!MainMenuCanvas.activeSelf);
    }

    public void tutorialClick()
    {
        TutorialScreen.SetActive(!TutorialScreen.activeSelf);
    }
    public void optionClick()
    {
        OptionScreen.SetActive(!OptionScreen.activeSelf);
    }
    public void StartScreenBackButton()
    {
        TutorialScreen.SetActive(false);
        OptionScreen.SetActive(false);
        StartScreen.SetActive(true);
    }

    public void videoOverviewButton()
    {
        TutorialScreen.SetActive(false);
        VideoTutorial.SetActive(true);
    }
    public void videoOverviewButtonBack()
    {
        Debug.Log("BackOverview");
        TutorialScreen.SetActive(true);
        VideoTutorial.SetActive(false);
    }
    public void EvidenceGeneralButton()
    {
        Debug.Log("Back Evidence");
        TutorialScreen.SetActive(!TutorialScreen.activeSelf);
        EvidenceTutorial.SetActive(!EvidenceTutorial.activeSelf);
    }
    public void ItemGeneralButton()
    {
        Debug.Log("Back Item");
        TutorialScreen.SetActive(!TutorialScreen.activeSelf);
        ItemTutorial.SetActive(!ItemTutorial.activeSelf);
    }
}
