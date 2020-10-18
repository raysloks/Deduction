using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;

public class Player : Mob
{
    public long killCooldown;

    [HideInInspector] public bool cantMove = false;
    [HideInInspector] public bool nearEmergencyButton = false;

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
        if (!cantMove && (!EventSystem.current.currentSelectedGameObject || !EventSystem.current.currentSelectedGameObject.GetComponent<InputField>()))
        {
            move.x += Input.GetAxis("Horizontal");
            move.y += Input.GetAxis("Vertical");
        }
        move = Vector3.ClampMagnitude(move, 1f);

        if (move.x > 0f)
            sprite.flipX = true;
        if (move.x < 0f)
            sprite.flipX = false;

        transform.position += move * Time.deltaTime * float.Parse(controller.settings.GetSetting("Player Speed").Get());

        visionLight.pointLightOuterRadius = GetVision();

        if (type == 0)
        {
            for (int i = 0; i < 2; ++i)
            {
                float radius = 0.5f;
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
        return role == 0 ?
            float.Parse(controller.settings.GetSetting("Crewmate Vision").Get()) :
            float.Parse(controller.settings.GetSetting("Impostor Vision").Get());
    }
}
