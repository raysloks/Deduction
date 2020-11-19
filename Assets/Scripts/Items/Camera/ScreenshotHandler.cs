using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventCallbacks;
using System.IO;
using TMPro;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering;

[RequireComponent(typeof(Camera))]
public class ScreenshotHandler : MonoBehaviour
{
    public GameController game;
    public Light2D visionLight;

    public GameObject dummyPrefab;
    public Transform dummyContainer;

    private Camera myCamera;

    private RenderTexture renderTexture;

    private List<Mob> dummies = new List<Mob>();

    private Photo photo;

    private float visionLightInnerRadius;
    private float visionLightOuterRadius;

    private Vector3 offset;

    public Dictionary<ulong, Photo> photos = new Dictionary<ulong, Photo>();

    private void Awake()
    {
        myCamera = GetComponent<Camera>();
        myCamera.enabled = false;

        offset = transform.position;
    }

    private void Update()
    {
        foreach (var n in photos)
        {
            if (n.Value.texture == null)
            {
                RecreateScreenshot(600, 450, n.Value);
                break;
            }
        }
    }

    private void OnEnable()
    {
        RenderPipelineManager.endCameraRendering += OnRenderEnd;
    }

    private void OnDisable()
    {
        RenderPipelineManager.endCameraRendering -= OnRenderEnd;
    }

    private void OnRenderEnd(ScriptableRenderContext context, Camera camera)
    {
        if (camera == myCamera)
        {
            OnPostRender();
        }
    }

    public void SetDummyPositions()
    {
        for (int i = dummies.Count; i < photo.poses.Count; ++i)
        {
            var dummy = Instantiate(dummyPrefab, dummyContainer);
            dummies.Add(dummy.GetComponent<Mob>());
        }

        for (int i = 0; i < photo.poses.Count; ++i)
        {
            PhotoPose pose = photo.poses[i];
            dummies[i].gameObject.SetActive(true);
            Transform transform = dummies[i].transform;
            transform.position = pose.position;
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
            dummies[i].characterTransform.localScale = new Vector3(pose.flipped ? -1f : 1f, 1f, 1f);
            Mob originalMob = game.handler.mobs[pose.index];
            dummies[i].SetSprite(originalMob.spriteIndex);
            dummies[i].SetName(originalMob.name);
        }

        for (int i = photo.poses.Count; i < dummies.Count; ++i)
        {
            dummies[i].gameObject.SetActive(false);
        }
    }

    public void DisableDummies()
    {
        foreach (var dummy in dummies)
            dummy.gameObject.SetActive(false);
    }

    public void Prepare()
    {
        SetDummyPositions();

        game.mobContainer.gameObject.SetActive(false);

        transform.position = dummies[photo.photographer].transform.position + offset;

        visionLight.transform.SetParent(dummies[photo.photographer].transform, false);
        visionLightInnerRadius = visionLight.pointLightInnerRadius;
        visionLightOuterRadius = visionLight.pointLightOuterRadius;

        // TEMP
        visionLight.pointLightInnerRadius = 5f;
        visionLight.pointLightOuterRadius = 10f;
    }

    public void Restore()
    {
        visionLight.pointLightOuterRadius = visionLightOuterRadius;
        visionLight.pointLightInnerRadius = visionLightInnerRadius;
        visionLight.transform.SetParent(game.player.transform, false);

        game.mobContainer.gameObject.SetActive(true);

        DisableDummies();
    }

    public Photo RecordPhoto()
    {
        Photo photo = new Photo();
        photo.photographer = -1;
        foreach (var n in game.handler.mobs)
        {
            ulong index = n.Key;
            Mob mob = n.Value;
            if (mob.gameObject.activeSelf)
            {
                if (index == game.handler.playerMobId)
                    photo.photographer = photo.poses.Count;

                PhotoPose pose = new PhotoPose();
                pose.index = index;
                pose.position = mob.transform.position;
                pose.flipped = mob.characterTransform.localScale.x < 0f;
                photo.poses.Add(pose);
            }
        }
        if (photo.photographer == -1)
            return null;
        return photo;
    }

    public void RecreateScreenshot(int width, int height, Photo photo)
    {
        if (this.photo == null)
        {
            this.photo = photo;

            renderTexture = RenderTexture.GetTemporary(width, height, 0);
            myCamera.targetTexture = renderTexture;

            photo.texture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);

            Prepare();

            myCamera.Render();
        }
    }

    private void OnPostRender()
    {
        Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
        photo.texture.ReadPixels(rect, 0, 0);
        photo.texture.Apply();
        photo = null;

        RenderTexture.ReleaseTemporary(renderTexture);
        renderTexture = null;
        myCamera.targetTexture = null;

        Restore();
    }
}
