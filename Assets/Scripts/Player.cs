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

    private new void Awake()
    {
        base.Awake();
        visionLight = GetComponentInChildren<Light2D>();
    }

    private new void Update()
    {
        Vector3 move = new Vector2();
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

        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);

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

    private new void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if (other.CompareTag("Area"))
        {
            controller.areaText.text = other.name;
            controller.areaText.GetComponent<Animator>().SetTrigger("EnterArea");
        }
    }

    private new void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
        if (other.CompareTag("Area") && controller.areaText.text == other.name)
        {
            controller.areaText.GetComponent<Animator>().SetTrigger("ExitArea");
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
    
 }
