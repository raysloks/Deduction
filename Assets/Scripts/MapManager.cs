using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    public GameController game;

    public void CheckForMapChange()
    {
        if (game.settings.map != SceneManager.GetActiveScene().buildIndex)
        {
            game.doorManager.doors.Clear();
            game.taskManager.minigameInitiators.Clear();
            SceneManager.LoadScene(game.settings.map);
        }
    }
}
