using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventCallbacks;
using System.IO;
using TMPro;

public class ScreenshotHandler : MonoBehaviour
{
    private static ScreenshotHandler instance;

    private Camera myCamera;
    public Player player;
    private List<byte[]> byteList = new List<byte[]>();
    private Dictionary<Vector3, List<Vector3>> ve3dic = new Dictionary<Vector3, List<Vector3>>();
 
    //  private List<Vector3> vec3 = new List<Vector3>();
    public GameController game;
    public GameObject lights;
    public GameObject mainCamera;
    public List<AudioClip> cameraSound;

    [Header("Camera Flash Stuff")]
    public GameObject canvasButtons;
    public GameObject targetMarker;
    public TextMeshProUGUI text;
    public GameObject arrowParent;
    public GameObject stayClose;
    private bool aP;
    private bool sC;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        myCamera = gameObject.GetComponent<Camera>();
        myCamera.enabled = false;
    }

    void Start()
    {
        EventSystem.Current.RegisterListener(EVENT_TYPE.PHASE_CHANGED, PhaseChanged);
    }

    WaitForEndOfFrame frameEnd = new WaitForEndOfFrame();

    public IEnumerator TakeScreenshot(int width, int height, bool meeting, Vector3 pos)
    {
        myCamera.enabled = true;

        Vector3 orgPos = transform.position;
        Vector3 orgPos2 = lights.transform.position;
        Vector3 orgPos3 = mainCamera.transform.position;
        if (meeting)
        {
            transform.position = new Vector3(pos.x, pos.y, transform.position.z);
            lights.transform.position = new Vector3(pos.x, pos.y, lights.transform.position.z);
            mainCamera.transform.position = new Vector3(pos.x, pos.y, mainCamera.transform.position.z);
            Debug.Log("StartScreenshot current Light/Camera/MainCamera Pos" + lights.transform.position + " " + transform.position + " " + mainCamera.transform.position+ "VS original" + orgPos2 + " " + orgPos + " " + orgPos3);
            DisableUI();
        }
        myCamera.targetTexture = RenderTexture.GetTemporary(width, height, 0);
       
        yield return frameEnd;     

        RenderTexture renderTexture = myCamera.targetTexture;

        Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
        Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
        renderResult.ReadPixels(rect, 0, 0);
        byte[] byteArray = renderResult.EncodeToPNG();

        if (!meeting){          
            List<Vector3> vec3 = new List<Vector3>();
            foreach (KeyValuePair<ulong, Mob> m in game.handler.mobs)
            {
                vec3.Add(m.Value.transform.position);
            }

            GetAllPlayerPositions message = new GetAllPlayerPositions();
            message.playerPos = vec3;
            game.handler.link.Send(message);

            byteList.Add(byteArray);
            StartCoroutine(EndCameraFlash(5f));

        }

        RenderTexture.ReleaseTemporary(renderTexture);
        myCamera.targetTexture = null;
      
        if (meeting) {
            EnableUI();

            transform.position = orgPos;
            lights.transform.position = orgPos2;

            mainCamera.transform.position = orgPos3;
            Debug.Log("EndScreenshot " + lights.transform.position + " " + myCamera.transform.position + " " + mainCamera.transform.position);
            
            SendEvidenceEvent sendEvidenceEvent = new SendEvidenceEvent();
            sendEvidenceEvent.byteArray = byteArray;
            EventSystem.Current.FireEvent(EVENT_TYPE.SNAPSHOT_EVIDENCE, sendEvidenceEvent);
        }
        myCamera.enabled = false;
       



    }

    public IEnumerator EndCameraFlash(float Sec)
    {
        if(player.role == 0)
        {
            while (player.visionLight.pointLightOuterRadius > player.GetVision())
            {               
                player.visionLight.pointLightOuterRadius -= Sec * Time.deltaTime;
                yield return null;
            }           
        }
        EnableUI();
    }

    public IEnumerator CameraFlash(float Sec, bool meeting, Vector3 pos)
    {

        SoundEvent se = new SoundEvent();
        se.UnitGameObjectPos = player.transform.position;
        se.UnitSound = cameraSound;
        EventSystem.Current.FireEvent(EVENT_TYPE.PLAY_SOUND, se);
        

        DisableUI();

        float counter = Sec;

        while (counter > 0)
        {            
            counter -= Time.deltaTime;
            yield return null;
        }

        ScreenshotHandler.TakeScreenshot_Static(Screen.width, Screen.height, meeting, pos);

        
    }
    void DisableUI()
    {
        player.cameraFlashing = true;

        aP = arrowParent.activeSelf;
        sC = stayClose.activeSelf;
        canvasButtons.GetComponent<Canvas>().enabled = false;
        arrowParent.SetActive(false);
        stayClose.SetActive(false);
        player.visionLight.intensity = 10f;

        targetMarker.GetComponent<SpriteRenderer>().enabled = false;

        player.visionLight.pointLightOuterRadius = player.controller.settings.impostorVision;
        text.color = Color.white;
    }
    void EnableUI()
    {
        if (player.role == 0)
            text.color = Color.white;
        else
            text.color = Color.red;
        
        player.visionLight.intensity = 1f;

        targetMarker.GetComponent<SpriteRenderer>().enabled = true;

        canvasButtons.GetComponent<Canvas>().enabled = true;
        arrowParent.SetActive(aP);
        stayClose.SetActive(sC);

        player.visionLight.pointLightOuterRadius = player.GetVision();
        player.cameraFlashing = false;
    }

    public void AddPos(List<Vector3> list, Vector3 playerPos)
    {      
        instance.ve3dic.Add(playerPos, list);
    }

    public static void TakeScreenshot_Static(int width, int height, bool meeting, Vector3 pos)
    {
        instance.StartCoroutine(instance.TakeScreenshot(width, height, meeting, pos));
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
        return instance.byteList[instance.byteList.Count - 1];      
    }

    public void PhaseChanged(EventCallbacks.Event eventInfo)
    {
        PhaseChangedEvent pc = (PhaseChangedEvent)eventInfo;

        if (pc.phase == GamePhase.Setup)
        {
            ve3dic.Clear();
            byteList.Clear();
        }
    }
}
