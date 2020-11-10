using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TaskManager : MonoBehaviour
{
    public GameController game;

    public GameObject indicatorPrefab;
    public GameObject indicatorParent;

    public RectTransform taskWindow;

    public List<Task> tasks = new List<Task>();

    public List<GameObject> indicators = new List<GameObject>();

    public Dictionary<int, MinigameInitiator> minigameInitiators = new Dictionary<int, MinigameInitiator>();

    public List<SabotageTask> sabotageTasks = new List<SabotageTask>();

    public List<GameObject> sabotageIndicators = new List<GameObject>();

    private float accumulator = 0.0f;
    private bool blink = false;

    private void Update()
    {
        for (int i = 0; i < tasks.Count; ++i)
        {

        }

        for (int i = indicators.Count; i < tasks.Count; ++i)
        {
            var indicator = Instantiate(indicatorPrefab);
            indicator.transform.SetParent(indicatorParent.transform);
            DontDestroyOnLoad(indicator);
            indicators.Add(indicator);
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


        accumulator += Time.deltaTime;
        if (accumulator > 1f)
        {
            accumulator -= 1f;
            blink = !blink;
        }


        for (int i = sabotageIndicators.Count; i < sabotageTasks.Count; ++i)
        {
            var indicator = Instantiate(indicatorPrefab);
            DontDestroyOnLoad(indicator);
            sabotageIndicators.Add(indicator);
        }

        for (int i = 0; i < sabotageTasks.Count; ++i)
        {
            if (minigameInitiators.ContainsKey(sabotageTasks[i].minigame_index))
            {
                sabotageIndicators[i].SetActive(true);
                Transform transform = sabotageIndicators[i].transform;
                Transform target = minigameInitiators[sabotageTasks[i].minigame_index].transform;
                Vector2 diff = target.position - game.player.transform.position;
                float distance = diff.magnitude;
                distance = Mathf.Min(distance, Mathf.Sqrt(distance) * 2f) * 0.5f;
                Vector3 position = Vector2.MoveTowards(game.player.transform.position, target.position, distance);
                position.y += 0.125f;
                position.z = position.y - 0.25f;
                transform.position = position;
                transform.up = diff;
                sabotageIndicators[i].GetComponent<SpriteRenderer>().color = blink ? Color.yellow : Color.red;
            }
        }

        for (int i = sabotageTasks.Count; i < sabotageIndicators.Count; ++i)
        {
            sabotageIndicators[i].SetActive(false);
        }
    }
}
