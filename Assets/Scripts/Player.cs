using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;
using TMPro;

public class Player : Mob
{
    public long killCooldown;
    public long sabotageCooldown;

    [HideInInspector] public bool canMove = true;
    [HideInInspector] public int emergencyButtonsLeft = 0;

    public GameController controller;

    private Light2D visionLight;

    [Header("Camera Flash Stuff")]
    public GameObject canvasButtons;
    public GameObject targetMarker;
    public TextMeshProUGUI text;
    public GameObject MeetingCanvas;
    public GameObject arrowParent;
    public GameObject stayClose;
    private bool cameraFlashing = false;

    private new void Awake()
    {
        base.Awake();
        visionLight = GetComponentInChildren<Light2D>();
    }

    private void Update()
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

        animator.SetFloat("Speed", move.magnitude);

        if(!cameraFlashing)
        {
            if(inCamo)
                visionLight.pointLightOuterRadius = GetVision() * 0.8f;
            else
                visionLight.pointLightOuterRadius = GetVision();
            
        }
        visionLight.pointLightInnerRadius = Mathf.Min(1f, visionLight.pointLightOuterRadius * 0.5f);
        visionLight.shadowIntensity = IsAlive ? 1.0f : 0.0f;

        if (type == 0)
        {
            for (int i = 0; i < 2; ++i)
            {
                float radius = 0.45f;
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, 1 << 10);
                foreach (var collider in colliders)
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

        if (Input.GetKeyDown(KeyCode.U))
        {
            EnterCamo();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            ExitCamo();
        }
    }

    public float GetVision()
    {
        if (!IsAlive)
            return 20f;
        if (role == 0)
            return Mathf.Max(1.0f, controller.settings.crewmateVision * controller.lightCurrent);
        else
            return controller.settings.impostorVision;
    }

    WaitForEndOfFrame frameEnd = new WaitForEndOfFrame();

    public IEnumerator CameraFlash(float Sec, bool meeting, Vector3 pos)
    {
        cameraFlashing = true;
        
        bool cB = canvasButtons.activeSelf;
        bool mC = MeetingCanvas.activeSelf;
        bool aP = arrowParent.activeSelf;
        bool sC = stayClose.activeSelf;
        canvasButtons.GetComponent<Canvas>().enabled = false;
        MeetingCanvas.GetComponent<Canvas>().enabled = false;
        arrowParent.SetActive(false);
        stayClose.SetActive(false);

        targetMarker.GetComponent<SpriteRenderer>().enabled = false;
        visionLight.pointLightOuterRadius = controller.settings.impostorVision;
        text.color = Color.white;
        float counter = Sec;

        while (counter > 0)
        {
            counter -= Time.deltaTime;
            yield return null;
        }

        ScreenshotHandler.TakeScreenshot_Static(Screen.width, Screen.height, meeting, pos);

        yield return frameEnd;

        if (role == 0)
        {
            text.color = Color.white;
        }
        else
        {
            text.color = Color.red;
        }
        targetMarker.GetComponent<SpriteRenderer>().enabled = true;
       
        canvasButtons.GetComponent<Canvas>().enabled = true;
        MeetingCanvas.GetComponent<Canvas>().enabled = true;
        arrowParent.SetActive(aP);
        stayClose.SetActive(sC);
        

        visionLight.pointLightOuterRadius = GetVision();
    }

    public override void EnterCamo()
    {
        inCamo = true;
        controller.UpdateHidden();
    }

    public override void ExitCamo()
    {
        inCamo = false;
        controller.UpdateHidden();
    }

}
