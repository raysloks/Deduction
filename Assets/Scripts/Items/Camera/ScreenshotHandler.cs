using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventCallbacks;
using System.IO;

public class ScreenshotHandler : MonoBehaviour
{
    private static ScreenshotHandler instance;

    private Camera myCamera;
    public Player player;
    private bool takeScreenshotOnNextFrame = false;
    private List<byte[]> byteList = new List<byte[]>();
    private List<List<Vector3>> vec3List = new List<List<Vector3>>();
    private List<List<Vector3>> vec3List2 = new List<List<Vector3>>();
    private Dictionary<Vector3, List<Vector3>> ve3dic = new Dictionary<Vector3, List<Vector3>>();
 
    //  private List<Vector3> vec3 = new List<Vector3>();
    public GameController game;
    public GameObject lights;
    public GameObject mainCamera;
    private byte[] LastPictureTaken;
    public List<AudioClip> cameraSound;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        myCamera = gameObject.GetComponent<Camera>();
       // player = transform.parent.GetComponent<Player>();
        myCamera.enabled = false;
    }

    void Update()
    {
        // disabled for now, only enable when fixed (no longer giving light sabotage immunity)

        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    StartCoroutine(player.CameraFlash(0.25f, false, Vector3.zero));
        //}
    }

    WaitForEndOfFrame frameEnd = new WaitForEndOfFrame();

    public IEnumerator TakeScreenshot(int width, int height, bool meeting, Vector3 pos, bool sC, bool aP)
    {
        myCamera.enabled = true;

        Vector3 orgPos = transform.position;
        Vector3 orgPos2 = lights.transform.position;
        Vector3 orgPos3 = mainCamera.transform.position;
        Vector3 pos1;
        Vector3 pos2;
        Vector3 pos3;
        if (meeting)
        {
            pos1 = new Vector3(pos.x, pos.y, transform.position.z);
            transform.position = pos1;
            pos2 = new Vector3(pos.x, pos.y, lights.transform.position.z);
            lights.transform.position = pos2;
            pos3 = new Vector3(pos.x, pos.y, mainCamera.transform.position.z);
            mainCamera.transform.position = pos3;
            Debug.Log("StartScreenshot current Light/Camera/MainCamera Pos" + lights.transform.position + " " + transform.position + " " + mainCamera.transform.position+ "VS original" + orgPos2 + " " + orgPos + " " + orgPos3);
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
            List<Vector3> vec3 = new List<Vector3>();
            foreach (KeyValuePair<ulong, Mob> m in game.handler.mobs)
            {
                vec3.Add(m.Value.transform.position);
            }
            vec3List.Add(vec3);

            GetAllPlayerPositions message = new GetAllPlayerPositions();
            message.playerPos = vec3List[vec3List.Count - 1];
            game.handler.link.Send(message);

            // File.WriteAllBytes(Application.dataPath + "/../SavedScreen.png", byteArray);
            byteList.Add(byteArray);
        }

        RenderTexture.ReleaseTemporary(renderTexture);
        myCamera.targetTexture = null;
      
        LastPictureTaken = byteArray;
        if (meeting) {

            transform.position = orgPos;
            lights.transform.position = orgPos2;

            mainCamera.transform.position = orgPos3;
            Debug.Log("EndScreenshot " + lights.transform.position + " " + myCamera.transform.position + " " + mainCamera.transform.position);

            SendEvidenceEvent sendEvidenceEvent = new SendEvidenceEvent();
            sendEvidenceEvent.byteArray = LastPictureTaken;
            EventSystem.Current.FireEvent(EVENT_TYPE.SNAPSHOT_EVIDENCE, sendEvidenceEvent);
            game.handler.link.Send(new TeleportToMeeting());

        }
        myCamera.enabled = false;
        if (!meeting)
        {
            StartCoroutine(EndCameraFlash(5f, aP, sC));

        }
        else
        {

            if (player.role == 0)
            {
                player.text.color = Color.white;
            }
            else
            {
                player.text.color = Color.red;
            }
            player.visionLight.intensity = 1f;

            player.targetMarker.GetComponent<SpriteRenderer>().enabled = true;

            player.canvasButtons.GetComponent<Canvas>().enabled = true;
            player.arrowParent.SetActive(aP);
            player.stayClose.SetActive(sC);

            player.visionLight.pointLightOuterRadius = player.GetVision();
            player.cameraFlashing = false;
        }



    }

    public IEnumerator EndCameraFlash(float Sec, bool aP, bool sC)
    {
        if(player.role == 0)
        {

            while (player.visionLight.pointLightOuterRadius > player.GetVision())
            {               
                player.visionLight.pointLightOuterRadius -= Sec * Time.deltaTime;
                yield return null;
            }
            
        }

        if (player.role == 0)
        {
            player.text.color = Color.white;
        }
        else
        {
            player.text.color = Color.red;
        }
        player.visionLight.intensity = 1f;

        player.targetMarker.GetComponent<SpriteRenderer>().enabled = true;

        player.canvasButtons.GetComponent<Canvas>().enabled = true;
        player.arrowParent.SetActive(aP);
        player.stayClose.SetActive(sC);

        player.visionLight.pointLightOuterRadius = player.GetVision();
        player.cameraFlashing = false;
    }

    public IEnumerator CameraFlash(float Sec, bool meeting, Vector3 pos)
    {

        if (!meeting)
        {
            SoundEvent se = new SoundEvent();
            se.UnitGameObjectPos = player.transform.position;
            se.UnitSound = cameraSound;
            EventSystem.Current.FireEvent(EVENT_TYPE.PLAY_SOUND, se);
        }
       

        player.cameraFlashing = true;

        bool aP = player.arrowParent.activeSelf;
        bool sC = player.stayClose.activeSelf;
        player.canvasButtons.GetComponent<Canvas>().enabled = false;
        player.arrowParent.SetActive(false);
        player.stayClose.SetActive(false);
        player.visionLight.intensity = 10f;

        player.targetMarker.GetComponent<SpriteRenderer>().enabled = false;

        player.visionLight.pointLightOuterRadius = player.controller.settings.impostorVision;
        player.text.color = Color.white;
        float counter = Sec;

        while (counter > 0)
        {
            
            counter -= Time.deltaTime;
            yield return null;
        }

        ScreenshotHandler.TakeScreenshot_Static(Screen.width, Screen.height, meeting, pos, sC, aP);

      //  yield return frameEnd;

        
    }

    public void AddPos(List<Vector3> list, Vector3 playerPos)
    {
       
        instance.ve3dic.Add(playerPos, list);
    }

    public static void TakeScreenshot_Static(int width, int height, bool meeting, Vector3 pos, bool sC, bool aP)
    {
        instance.StartCoroutine(instance.TakeScreenshot(width, height, meeting, pos, sC, aP));
    }

    public static void StartCameraFlash(float time, bool meeting, Vector3 pos)
    {
        instance.StartCoroutine(instance.CameraFlash(time, meeting, pos));
    }

    public static List<byte[]> GetListOfPicturesTaken()
    {
        return instance.byteList;
    }
    public static Dictionary<Vector3, List<Vector3>> GetListOfPicturesPositions()
    {       
        return instance.ve3dic;
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
