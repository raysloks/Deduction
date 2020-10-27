using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TaskManager : MonoBehaviour
{
    public GameController game;

    public GameObject indicatorPrefab;

    public RectTransform taskWindow;

    public List<Task> tasks = new List<Task>();

    public List<GameObject> indicators = new List<GameObject>();

    private void Update()
    {
        for (int i = 0; i < tasks.Count; ++i)
        {

        }

        for (int i = indicators.Count; i < tasks.Count; ++i)
        {
            indicators.Add(Instantiate(indicatorPrefab));
        }

        for (int i = 0; i < tasks.Count; ++i)
        {
            indicators[i].SetActive(!tasks[i].completed);
            Transform transform = indicators[i].transform;
            Transform target = game.MinigameInitiators[tasks[i].index % game.MinigameInitiators.Count].transform;
            Vector2 diff = target.position - game.player.transform.position;
            transform.position = Vector2.MoveTowards(game.player.transform.position, target.position, Mathf.Min(diff.magnitude * 0.5f, 5f));
            transform.up = diff;
        }

        for (int i = tasks.Count; i < indicators.Count; ++i)
        {
            indicators[i].SetActive(false);
        }
    }
}
