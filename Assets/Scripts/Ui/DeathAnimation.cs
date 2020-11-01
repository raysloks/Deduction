using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DeathAnimation : MonoBehaviour
{
    public Animator animator;
    public Image victimImage;
    public Image killerImage;

    public void Play(Mob victim, Mob killer)
    {
        gameObject.SetActive(true);
        victimImage.color = victim.sprite.color;
        killerImage.color = killer.sprite.color;
        animator.SetTrigger("Start");
    }
}
