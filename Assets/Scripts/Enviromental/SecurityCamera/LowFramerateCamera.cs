using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class LowFramerateCamera : MonoBehaviour
{
    public float fps = 30;

    public static LowFramerateCameraObserver observer = null; // very dirty temp hack

    private Camera camera;

    private float accumulator = 0f;

    private static LowFramerateCamera current = null;

    private void Awake()
    {
        camera = GetComponent<Camera>();
        camera.enabled = false;
    }

    private void Update()
    {
        if (current == this)
            current = null;
        accumulator += Time.deltaTime;
        if (accumulator > 1f / fps && current == null && observer != null)
        {
            current = this;
            accumulator = Mathf.Repeat(accumulator, 1f / fps);
            camera.Render();
        }
    }
}
