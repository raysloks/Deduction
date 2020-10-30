using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;

public class Player : Mob
{
    public long killCooldown;
    public long sabotageCooldown;

    [HideInInspector] public bool canMove = true;
    [HideInInspector] public int emergencyButtonsLeft = 0;

    public GameController controller;

    private Light2D visionLight;

    private new void Awake()
    {
        base.Awake();
        visionLight = GetComponent<Light2D>();
    }

    private void Update()
    {
        Vector3 move = new Vector2();
        if (canMove && (!EventSystem.current.currentSelectedGameObject || !EventSystem.current.currentSelectedGameObject.GetComponent<InputField>()))
        {
            move.x += Input.GetAxis("Horizontal");
            move.y += Input.GetAxis("Vertical");

            move = Vector3.ClampMagnitude(move, 1f);
            transform.position += move * Time.deltaTime * controller.settings.playerSpeed;
        }

        if (move.x > 0f)
            sprite.flipX = true;
        if (move.x < 0f)
            sprite.flipX = false;

        visionLight.pointLightOuterRadius = GetVision();
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
}
