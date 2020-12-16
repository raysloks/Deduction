using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Experimental.U2D.Animation;

public class Player : Mob
{
    public long killCooldown;
    public long sabotageCooldown;

    [HideInInspector] public bool canMove = true;
    [HideInInspector] public int emergencyButtonsLeft = 0;

    public GameController controller;

    [HideInInspector] public Light2D visionLight;
    [HideInInspector] public bool cameraFlashing = false;
    [HideInInspector] public Vector3 move;
    [HideInInspector] public bool MotionSensorCheck = false;
    public GameObject UpDownArrow;
    public GameObject LeftRightArrow;


    private AudioSource audioSource;
    private int inAreas;
    private string lastArea;

    private new void Awake()
    {
        base.Awake();
        visionLight = GetComponentInChildren<Light2D>();
    }

    private new void Update()
    {
        move = new Vector2();
        if (canMove && (!EventSystem.current.currentSelectedGameObject || controller.phase == GamePhase.Main))
        {
            move.x += Input.GetAxis("Horizontal");
            move.y += Input.GetAxis("Vertical");

            move = Vector3.ClampMagnitude(move, 1f);
            transform.position += move * Time.deltaTime * controller.settings.playerSpeed;
        }

        if (move.x > 0f)
            characterTransform.localScale = new Vector3(-1f, 1f, 1f);
        if (move.x < 0f)
            characterTransform.localScale = new Vector3(1f, 1f, 1f);

        animator.SetFloat("Speed", move.magnitude * controller.settings.playerSpeed);

        if (!cameraFlashing)
        {
            if (inCamo && IsAlive)
                visionLight.pointLightOuterRadius = GetVision() * 0.85f;
            else
                visionLight.pointLightOuterRadius = GetVision();
        }
        visionLight.pointLightInnerRadius = Mathf.Min(1f, visionLight.pointLightOuterRadius * 0.5f);
        visionLight.shadowIntensity = IsAlive ? 1.0f : 0.0f;
        for (int i = 0; i < 2; ++i)
        {
            float radius = 0.3f;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, 1 << 10);
            foreach (var collider in colliders)
            {
                if (type == 0 || collider.CompareTag("Ghost collision"))
                {
                    Vector2 point = Physics2D.ClosestPoint(transform.position, collider);
                    Vector2 diff = point - (Vector2)transform.position;
                    if (diff.magnitude < radius)
                    {
                        transform.position += (Vector3)(diff.normalized * (diff.magnitude - radius));
                    }
                }
            }
        }

        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);

        if (MotionSensorCheck)
        {
            LeftRightArrow.SetActive(false);
            UpDownArrow.SetActive(false);
            if (move.x == 0f && move.y == 0f)
            {

                Debug.Log("LEFTRIGHT");
                LeftRightArrow.SetActive(true);

            }
            else if (Mathf.Abs(move.y) > Mathf.Abs(move.x))
            {
                Debug.Log("LEFTRIGHT");
                LeftRightArrow.SetActive(true);
            }
            else
            {
                Debug.Log("UPDOWN");
                UpDownArrow.SetActive(true);
            }
        }


        base.Update();
    }

    public float GetVision()
    {
        if (!IsAlive)
            return 50f;
        if (role == 0)
            return Mathf.Max(1.0f, controller.settings.crewmateVision * controller.lightCurrent);
        else
            return controller.settings.impostorVision;
    }

    public void ResetArrows()
    {
        LeftRightArrow.SetActive(false);
        UpDownArrow.SetActive(false);

    }

    private new void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if (other.CompareTag("Area"))
        {
            inAreas += 1;
            if (!inLocker)
            {
                lastArea = controller.areaText.text;

                if (inAreas == 1)
                {
                    controller.areaText.GetComponent<Animator>().SetTrigger("EnterArea");
                    controller.areaText.text = other.name;
                }
                else
                {
                    controller.areaText.GetComponent<Animator>().SetTrigger("ExitArea");
                    StartCoroutine(UpdateAreaName(other.name));
                }
            }
            else
                controller.areaText.GetComponent<Animator>().SetBool("InLocker", true);
        }
    }

    private new void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
        if (other.CompareTag("Area"))
        {
            inAreas -= 1;
            if (inAreas == 0)
            {
                if (controller.areaText.GetComponent<Animator>().GetBool("InLocker") == false)
                    controller.areaText.GetComponent<Animator>().SetTrigger("ExitArea");
                else
                    controller.areaText.GetComponent<Animator>().SetBool("InLocker", false);
            }
            else if (controller.areaText.text == other.name)
                controller.areaText.text = lastArea;
        }
    }

    public override void EnterCamo()
    {
        inCamo = true;

        SpriteRenderer[] spr = GetComponentsInChildren<SpriteRenderer>();

        for (int i = 0; i < spr.Length; i++)
        {
            Color color = spr[i].color;
            color.a = 0.75f;
            spr[i].color = color;
        }
        controller.UpdateHidden();
    }

    public override void ExitCamo()
    {
        inCamo = false;

        SpriteRenderer[] spr = GetComponentsInChildren<SpriteRenderer>();

        for (int i = 0; i < spr.Length; i++)
        {
            Color color = spr[i].color;
            color.a = 1;
            spr[i].color = color;
        }
        controller.UpdateHidden();
    }

    IEnumerator UpdateAreaName(string areaName)
    {
        int step = 0;
        while (step < 2)
        {
            step++;
            if (step == 2)
            {
                Debug.Log("Coroutine: " + areaName);
                controller.areaText.GetComponent<Animator>().SetTrigger("EnterArea");
                controller.areaText.text = areaName;
            }
            else
                yield return new WaitForSeconds(controller.areaText.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        }
    }
}
