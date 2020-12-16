using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LockerScript : Interactable
{
    public int locker_index;
    public bool occupied;
    public GameObject occupant;
    public SpriteRenderer outline;
    public SpriteRenderer highlight;
    public Vector3 exitDirection;

    void Start()
    {
        if (locker_index >= 0)
            FindObjectOfType<LockerManager>().Lockers[locker_index] = this;
        occupied = false;
        GetComponent<ShadowCaster2D>().castsShadows = false;
    }
    

    public override bool CanInteract(GameController game)
    {
        return game.player.IsAlive;
    }

    public override void Interact(GameController game)
    {
        //this.game = game;
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
            outline.enabled = false;
            GetComponent<ShadowCaster2D>().castsShadows = true;
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
        occupant.transform.position = this.transform.position + exitDirection;
        var script = occupant.GetComponent<Player>();
        if (script != null)
        {
            occupant.GetComponent<Player>().Reveal();
            occupant.GetComponent<Player>().inLocker = false;
            occupant.GetComponent<Player>().canMove = true;
            outline.enabled = true;
            GetComponent<ShadowCaster2D>().castsShadows = false;
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