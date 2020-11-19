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
    private List<byte[]> byteList = new List<byte[]>();
    private List<List<Vector3>> vec3List = new List<List<Vector3>>();
    private List<Vector3> vec3 = new List<Vector3>();

    public Player player;
    public GameController game;
    public GameObject lights;
    public GameObject mainCamera;
    public List<AudioClip> cameraSound;

    [Header("Camera Flash Stuff")]
    public GameObject canvasButtons;
    public GameObject targetMarker;
    public TextMeshPro text;
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


    //Takes a screenshot.
    //If its not a meeting it adds this screenshot to a list and record the players positions
    //If it is a meeting before taking a picture it moves all the player to the recorded position and moves the camera/Light to the position of the player who took the picture
    //then it sends that picture to the votebutton of the player that originally took the picture
    public IEnumerator TakeScreenshot(int width, int height, bool meeting, Vector3 pos, VoterEvidence va)
    {
        myCamera.enabled = true;
        float counter = 0.3f;
        Vector3 orgPos = transform.position;
        Vector3 orgPos2 = lights.transform.position;
        Vector3 orgPos3 = mainCamera.transform.position;
        if (meeting)
        {         
            //For some reason need to wait a bit before taking picture. Verkar som att mobsen inte hinner teleporteras av nån anledning
            while (counter > 0)
            {
                counter -= Time.deltaTime;
                yield return null;
            }
            transform.position = new Vector3(pos.x, pos.y, transform.position.z);
            lights.transform.position = new Vector3(pos.x, pos.y, lights.transform.position.z);
            mainCamera.transform.position = new Vector3(pos.x, pos.y, mainCamera.transform.position.z);
            yield return frameEnd;

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
            
            List<Vector3> vec3players = new List<Vector3>();
            foreach (KeyValuePair<ulong, Mob> m in game.handler.mobs)
            {
                vec3players.Add(m.Value.transform.position);
            }

            GetAllPlayerPositions message = new GetAllPlayerPositions();
            message.playerPos = vec3players;
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

            va.ba = byteArray;
            va.newEvidence.SetActive(true);
        }
        myCamera.enabled = false;
    }

    //Ends the camera flash
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

    //Starts the camera flash and sound effect
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

        ScreenshotHandler.TakeScreenshot_Static(Screen.width, Screen.height, meeting, pos, null);

        
    }
    void DisableUI()
    {
        player.cameraFlashing = true;

        aP = arrowParent.activeSelf;
        sC = stayClose.activeSelf;
        canvasButtons.GetComponent<Canvas>().enabled = false;
        arrowParent.SetActive(false);
        stayClose.SetActive(false);
        player.visionLight.intensity = 1f;

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
        instance.vec3.Add(playerPos);
        instance.vec3List.Add(list);
    }

    public static void TakeScreenshot_Static(int width, int height, bool meeting, Vector3 pos, VoterEvidence va)
    {
        instance.StartCoroutine(instance.TakeScreenshot(width, height, meeting, pos, va));
    }

    public static void StartCameraFlash(float time, bool meeting, Vector3 pos)
    {
        instance.StartCoroutine(instance.CameraFlash(time, meeting, pos));
    }

    public static List<byte[]> GetListOfPicturesTaken()
    {
        return instance.byteList;
    }
    public static List<List<Vector3>> GetListOfPicturesPositions()
    {       
        return instance.vec3List;
    }

    public static List<Vector3> GetListOfPlayerPositions()
    {
        return instance.vec3;
    }

    public static void ClearListOfPicturesTaken()
    {
        instance.byteList.Clear();
    }
    public static byte[] GetLastPicture()
    {
        return instance.byteList[instance.byteList.Count - 1];      
    }

    //End of game cleanup
    public void PhaseChanged(EventCallbacks.Event eventInfo)
    {
        PhaseChangedEvent pc = (PhaseChangedEvent)eventInfo;

        if (pc.phase == GamePhase.Setup )
        {
            vec3.Clear();
            vec3List.Clear();
            byteList.Clear();
        }
    }
}
