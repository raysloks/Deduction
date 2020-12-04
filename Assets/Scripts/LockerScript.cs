using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockerScript : Interactable
{
    public int locker_index;
    public bool occupied;
    public GameObject occupant;
    private GameController game;
    public SpriteRenderer outline;
    // Start is called before the first frame update
    void Start()
    {
        if (locker_index > 0)
            FindObjectOfType<LockerManager>().Lockers[locker_index] = this;
        occupied = false;
    }

    private void Awake()
    {

    }

    public override bool CanInteract(GameController game)
    {
        return game.player.IsAlive;
    }

    public override void Interact(GameController game)
    {
        this.game = game;
        game.handler.link.Send(new HideAttempted { index = locker_index, user = game.handler.playerMobId });

    }

    public override void Highlight(bool highlighted)
    {
        if (outline != null)
        {
            if (highlighted)
                StartCoroutine(FadeInOutline(0.2f));
            else
                StartCoroutine(FadeOutOutline(0.2f));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AttemptToHide(GameObject body)
    {
        if (!occupied)
            HidePerson(body);
        else
            RemovePerson();
    }

    public void HidePerson(GameObject body)
    {
        occupant = body;
        var script = body.GetComponent<Player>();
        if (script != null)
        {
            occupant.GetComponent<Player>().Hide();
            occupant.GetComponent<Player>().inLocker = true;
        }
        else
        {
            occupant.GetComponent<NetworkMob>().Hide();
            occupant.GetComponent<NetworkMob>().inLocker = true;
        }
        occupant.transform.position = this.transform.position;

    }

    public void RemovePerson()
    {
        occupant.transform.position = this.transform.position + new Vector3(0f, -1f, 0f);
        var script = occupant.GetComponent<Player>();
        if (script != null)
        {
            occupant.GetComponent<Player>().Reveal();
            occupant.GetComponent<Player>().inLocker = false;
            occupant.GetComponent<Player>().canMove = true;
        }
        else
        {
            occupant.GetComponent<NetworkMob>().Reveal();
            occupant.GetComponent<NetworkMob>().inLocker = false;
        }
        occupant = null;
    }

    IEnumerator FadeInOutline(float seconds)
    {
        float counter = 0;

        while (counter < seconds)
        {
            Color color = outline.color;
            color.a = counter / seconds;
            outline.color = color;
            counter += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator FadeOutOutline(float seconds)
    {
        float counter = seconds;

        while (counter > 0f)
        {
            Color color = outline.color;
            color.a = counter / seconds;
            outline.color = color;
            counter -= Time.deltaTime;
            yield return null;
        }
    }


}
