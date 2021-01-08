using UnityEngine;
using System.Collections;

public class Interactable : MonoBehaviour
{
    public SpriteRenderer outline;

    public virtual bool CanInteract(GameController game)
    {
        return true;
    }

    public virtual void Interact(GameController game)
    {

    }

    public void Highlight(bool highlighted)
    {
        if (outline != null)
        {
            if (highlighted)
                StartCoroutine(FadeInOutline(0.2f));
            else
                StartCoroutine(FadeOutOutline(0.2f));
        }
    }

    IEnumerator FadeInOutline(float seconds)
    {
        float counter = 0;

        while (counter < seconds)
        {
            Color color = outline.color;
            counter = Mathf.Min(counter + Time.deltaTime, seconds);
            color.a = counter / seconds;
            outline.color = color;
            yield return null;
        }
    }

    IEnumerator FadeOutOutline(float seconds)
    {
        float counter = seconds;

        while (counter > 0f)
        {
            Color color = outline.color;
            counter = Mathf.Max(counter - Time.deltaTime, 0f);
            color.a = counter / seconds;
            outline.color = color;
            yield return null;
        }
    }
}