using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenshotHandler : MonoBehaviour
{
    private static ScreenshotHandler instance;

    private Camera myCamera;
    private bool takeScreenshotOnNextFrame = false;
    private List<byte[]> byteList = new List<byte[]>();

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        myCamera = gameObject.GetComponent<Camera>();       
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine(TakeScreenshot(Screen.width, Screen.height));
        }
    }

    WaitForEndOfFrame frameEnd = new WaitForEndOfFrame();

    public IEnumerator TakeScreenshot(int width, int height)
    {
        Debug.Log("StartScreenshot");
        myCamera.targetTexture = RenderTexture.GetTemporary(width, height);

        yield return frameEnd;

        RenderTexture renderTexture = myCamera.targetTexture;

        Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
        Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
        renderResult.ReadPixels(rect, 0, 0);

        byte[] byteArray = renderResult.EncodeToPNG();
           
        byteList.Add(byteArray);
        Debug.Log("This Many Pictures in List: " + byteList.Count);
        RenderTexture.ReleaseTemporary(renderTexture);
        myCamera.targetTexture = null;

        

    }

    public static void TakeScreenshot_Static(int width, int height)
    {
        instance.StartCoroutine(instance.TakeScreenshot(width, height));
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
