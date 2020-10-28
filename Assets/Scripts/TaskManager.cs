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

    public Dictionary<int, MinigameInitiator> minigameInitiators = new Dictionary<int, MinigameInitiator>();

    private void Awake()
    {
        foreach (MinigameInitiator minigameInitiator in FindObjectsOfType<MinigameInitiator>())
            minigameInitiators.Add(minigameInitiator.minigame_index, minigameInitiator);
    }

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
            if (minigameInitiators.ContainsKey(tasks[i].minigame_index))
            {
                indicators[i].SetActive(!tasks[i].completed);
                Transform transform = indicators[i].transform;
                Transform target = minigameInitiators[tasks[i].minigame_index].transform;
                Vector2 diff = target.position - game.player.transform.position;
                float distance = diff.magnitude;
                distance = Mathf.Min(distance, Mathf.Sqrt(distance) * 2f) * 0.5f;
                Vector3 position = Vector2.MoveTowards(game.player.transform.position, target.position, distance);
                position.y += 0.125f;
                position.z = position.y - 0.25f;
                transform.position = position;
                transform.up = diff;
            }
        }

        for (int i = tasks.Count; i < indicators.Count; ++i)
        {
            indicators[i].SetActive(false);
        }
    }
}
