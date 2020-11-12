using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventCallbacks;
using System.IO;

public class ScreenshotHandler : MonoBehaviour
{
    private static ScreenshotHandler instance;

    private Camera myCamera;
    private Player player;
    private bool takeScreenshotOnNextFrame = false;
    private List<byte[]> byteList = new List<byte[]>();
    private List<List<Vector3>> vec3List = new List<List<Vector3>>();
  //  private List<Vector3> vec3 = new List<Vector3>();
    public GameController game;
    public GameObject lights;
    private byte[] LastPictureTaken;

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
            StartCoroutine(player.CameraFlash(0.25f, false, Vector3.zero));
        }
    }

    WaitForEndOfFrame frameEnd = new WaitForEndOfFrame();

    public IEnumerator TakeScreenshot(int width, int height, bool meeting, Vector3 pos)
    {
        Vector3 orgPos = myCamera.transform.position;
        Vector3 orgPos2 = lights.transform.position;
        if (meeting)
        {

           
            myCamera.transform.position = pos;
            lights.transform.position = pos + new Vector3(0f, 2f, 0f);
            Debug.Log("StartScreenshot " + lights.transform.position + " " + myCamera.transform.position);
        }
        myCamera.targetTexture = RenderTexture.GetTemporary(width, height, 0);

        yield return frameEnd;

        RenderTexture renderTexture = myCamera.targetTexture;

        Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
        Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
        renderResult.ReadPixels(rect, 0, 0);

        byte[] byteArray = renderResult.EncodeToPNG();
        if (!meeting)
        {
            // Dictionary<ulong, Mob> mobs = new Dictionary<ulong, Mob>();
            List<Vector3> vec3 = new List<Vector3>();
            foreach (KeyValuePair<ulong, Mob> m in game.handler.mobs)
            {
                vec3.Add(m.Value.transform.position);
            }
            vec3List.Add(vec3);

            // File.WriteAllBytes(Application.dataPath + "/../SavedScreen.png", byteArray);
            byteList.Add(byteArray);
        }

        Debug.Log("This Many Pictures in List: " + byteList.Count);
        RenderTexture.ReleaseTemporary(renderTexture);
        myCamera.targetTexture = null;
        myCamera.transform.position = orgPos;
        lights.transform.position = orgPos2;
        LastPictureTaken = byteArray;
        if (meeting) {
            Debug.Log("EndScreenshot " + lights.transform.position + " " + myCamera.transform.position);

            SendEvidenceEvent sendEvidenceEvent = new SendEvidenceEvent();
            sendEvidenceEvent.byteArray = LastPictureTaken;
            EventSystem.Current.FireEvent(EVENT_TYPE.SEND_EVIDENCE2, sendEvidenceEvent);
        }

    }

    public static void TakeScreenshot_Static(int width, int height, bool meeting, Vector3 pos)
    {
        instance.StartCoroutine(instance.TakeScreenshot(width, height, meeting, pos));
    }

    public static void StartCameraFlash(float time, bool meeting, Vector3 pos)
    {
        instance.StartCoroutine(instance.player.CameraFlash(time, meeting, pos));
    }

    public static List<byte[]> GetListOfPicturesTaken()
    {
        return instance.byteList;
    }
    public static List<List<Vector3>> GetListOfPicturesPositions()
    {
        return instance.vec3List;
    }

    public static void ClearListOfPicturesTaken()
    {
        instance.byteList.Clear();
    }
    public static byte[] GetLastPicture()
    {
       return instance.LastPictureTaken;
    }
}
