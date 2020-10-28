using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class MinigameInitiator : Interactable
{
    public GameObject minigame;

    public int minigame_index;

    private MinigamePopupScript popup;
    private GameController game;
    public Material outline;
    private Material og;
    private bool interactable = false;

    private void Awake()
    {
        popup = FindObjectOfType<MinigamePopupScript>();
        og = GetComponent<SpriteRenderer>().material;
    }

    public override bool CanInteract(GameController game)
    {
        interactable = game.taskManager.tasks.Find(x => x.minigame_index == minigame_index && !x.completed) != null;
        return interactable;
    }

    public override void Interact(GameController game)
    {
        this.game = game;
        popup.ActivatePopup(minigame, this);
    }

    public void Solved()
    {
        GetComponent<SpriteRenderer>().material = og;
        int task_index = game.taskManager.tasks.FindIndex(x => x.minigame_index == minigame_index);
        game.taskManager.tasks[task_index].completed = true;
        game.handler.link.Send(new TaskUpdate { task = (ushort)task_index });
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            if(interactable == true)
            {
                GetComponent<SpriteRenderer>().material = outline;
                outline.SetFloat("_FishEyeUvAmount", 0f);
                outline.SetFloat("_HitEffectBlend", 0f);
                outline.SetFloat("_ColorChangeLuminosity", 0f);
                StartCoroutine(FadeInOutline(1));
                Debug.Log("Player Entered trig");
            }
        }

    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            StartCoroutine(FadeOutOutline(1));

            Debug.Log("Player exited trig");
        }

    }

    IEnumerator FadeInOutline(float Sec)
    {
        float counter = 0;


        while (counter < Sec)
        {

            outline.SetFloat("_FishEyeUvAmount", 0.235f * (counter / Sec));
            outline.SetFloat("_HitEffectBlend", 0.1f * (counter / Sec));
            outline.SetFloat("_ColorChangeLuminosity", 1f * (counter / Sec));
            counter += Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator FadeOutOutline(float Sec)
    {
        float counter = Sec;


        while (counter > 0)
        {
            outline.SetFloat("_FishEyeUvAmount", 0.235f * (counter / Sec));
            outline.SetFloat("_HitEffectBlend", 0.1f * (counter / Sec));
            outline.SetFloat("_ColorChangeLuminosity", 1f * (counter / Sec));
            counter -= Time.deltaTime;
            yield return null;
        }
        GetComponent<SpriteRenderer>().material = og;

    }
}
