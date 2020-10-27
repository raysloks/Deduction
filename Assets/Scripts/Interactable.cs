using UnityEngine;
using System.Collections;

public class Interactable : MonoBehaviour
{
    public virtual bool CanInteract(GameController game)
    {
        return true;
    }

    public virtual void Interact(GameController game)
    {

    }
}
