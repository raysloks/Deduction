using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    public GameController game;
    public GameObject sabotageMenus;

    public void CheckForMapChange()
    {
        if (game.settings.map != SceneManager.GetActiveScene().buildIndex)
        {
            game.doorManager.doors.Clear();
            game.taskManager.minigameInitiators.Clear();
            game.GetComponent<SabotageButtonManager>().map = game.settings.map;
            game.GetComponent<SabotageButtonManager>().UpdateButtons();

            SceneManager.LoadScene(game.GetComponent<SabotageButtonManager>().map);
        }
        else
        {
            game.GetComponent<SabotageButtonManager>().map = game.settings.map;
            game.GetComponent<SabotageButtonManager>().UpdateButtons();
        }
    }
}