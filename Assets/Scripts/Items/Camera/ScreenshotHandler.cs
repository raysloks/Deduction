using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;

public class ScreenshotHandler : MonoBehaviour
{
    private static ScreenshotHandler instance;

    private Camera myCamera;
    private Player player;
    private bool takeScreenshotOnNextFrame = false;
    private List<byte[]> byteList = new List<byte[]>();

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        myCamera = gameObject.GetComponent<Camera>();
        player = transform.parent.GetComponent<Player>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine(player.CameraFlash(0.25f));
        }
    }

    WaitForEndOfFrame frameEnd = new WaitForEndOfFrame();

    public IEnumerator TakeScreenshot(int width, int height)
    {
        Debug.Log("StartScreenshot");
        myCamera.targetTexture = RenderTexture.GetTemporary(width, height, 0);

        yield return frameEnd;

        RenderTexture renderTexture = myCamera.targetTexture;

        Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
        renderResult.ReadPixels(rect, 0, 0);

        byte[] byteArray = renderResult.EncodeToPNG();

        // File.WriteAllBytes(Application.dataPath + "/../SavedScreen.png", byteArray);
        byteList.Add(byteArray);
        Debug.Log("This Many Pictures in List: " + byteList.Count);
        RenderTexture.ReleaseTemporary(renderTexture);
        myCamera.targetTexture = null;        

    }

    public static void TakeScreenshot_Static(int width, int height)
    {
        instance.StartCoroutine(instance.TakeScreenshot(width, height));
    }

    public static void StartCameraFlash(float time)
    {
        instance.StartCoroutine(instance.player.CameraFlash(0.25f));
    }

    public static List<byte[]> GetListOfPicturesTaken()
    {
        return instance.byteList;
    }

    public static void ClearListOfPicturesTaken()
    {
        instance.byteList.Clear();
    }
}
