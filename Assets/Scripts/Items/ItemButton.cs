using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EventCallbacks;
using System;

using UnityEngine.EventSystems;

public class ItemButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private static ItemButton instance;


    [HideInInspector]public enum Item { None, Camera, MotionSensor, SmokeGrenade, Knife, PulseChecker };
    [HideInInspector]public Item myItem;

    public Player player;
    public GameController gc;
    public TextMeshProUGUI text2;
    public GameObject buttonItemImage;
    public EvidenceHandler evidenceHandler;
    public GameObject ItemContainers;
    public TextMeshProUGUI infoText;
    private Image myItemImage;
    private Image backgroundImage;
    public Image infoImage;
    [Header("Motion Sensor")]
    public GameObject motionSensorPrefab;
    public Sprite buttonMotionSensorImage;
    public Animation motionSensorBeep;
    public GameObject MotionSensorUI;
    private int motionSensorNumber = 1;

    [Header("Camera")]
    public Sprite CameraSprite;
    public int maxPhotos = 3;
    private int photosTaken = 0;

    [Header("Smoke Grenade")]
    public Sprite SmokeGrenadeSprite;
    public TextMeshProUGUI areaText;


    [Header("Knife")]
    public Sprite KnifeSprite;

    [Header("Pulse Checker")]
    public Sprite pulseSprite;
    public PulseCheckerUI pcUI;
    public float PcCD;

    delegate void Calculation();
    Calculation Click;
    delegate void Calculation2();
    Calculation2 Enter;
    delegate void Calculation3();
    Calculation3 Exit;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        myItemImage = buttonItemImage.GetComponent<Image>();
        backgroundImage = GetComponent<Image>();
        //  SetItem(UnityEngine.Random.Range(1, (Enum.GetValues(typeof(Item)).Length)));
        SetItem(2);
        EventCallbacks.EventSystem.Current.RegisterListener(EVENT_TYPE.PHASE_CHANGED, PhaseChanged);

        if(ItemContainers == null)
        {
            ItemContainers = GameObject.Find("ItemContainers");
        }
    }
    
    public void ItemClick()
    {
        Click();
    }
	
    //if you got no items
    private void NoneClick()
    {
        float closestDistance = 2f;
        GameObject childGo = null;
        int index = 0;
        foreach (Transform child in ItemContainers.transform)
        {
            if (Vector2.Distance(child.position, player.transform.position) < closestDistance)
            {
                childGo = child.gameObject;
                closestDistance = Vector2.Distance(child.position, player.transform.position);
                Debug.Log("Close " + Vector2.Distance(child.position, player.transform.position));
                break;
            }
            index++;
        }
        if (childGo != null)
        {
            int item = (int)childGo.GetComponent<ItemContainer>().item;
            if (item != 0)
            {
                SetItem(item);

                childGo.GetComponent<ItemContainer>().ItemTaken();
                PickupCooldown message = new PickupCooldown();
                message.child = index;
                message.random = UnityEngine.Random.Range(1, Enum.GetValues(typeof(Item)).Length);
                gc.handler.link.Send(message);
            }
        }
    }
	
    //if you got camera item
    private void CameraClick()
    {
        if (photosTaken < maxPhotos)
        {
            gc.handler.link.Send(new TakePhoto());
            photosTaken++;
            if (GetComponent<AudioSource>().clip != null)
                GetComponent<AudioSource>().Play();
        }
        if (photosTaken >= maxPhotos)
        {
            SetItem(0);
        }
    }

    private void MotionSensorClick()
    {
        MotionSensorUI.SetActive(true);
        GameObject ms = Instantiate(motionSensorPrefab, player.transform.position, Quaternion.identity);
        CheckSensor cs = ms.GetComponent<CheckSensor>();
        cs.gc = gc;
        cs.evidenceHandler = evidenceHandler;
        cs.SetNumber(motionSensorNumber);
        cs.anim = motionSensorBeep;
        SetItem(0);
        gc.player.ResetArrows();
        gc.player.MotionSensorCheck = false;
        motionSensorNumber++;
    }

    private void SmokeGrenadeClick()
    {
        SGEvidence sge = new SGEvidence();
        sge.area = areaText.text;
        if(areaText.text == "")
        {
            sge.area = "Outside";
        }
        sge.playerName = player.gameObject.name;
        sge.playerID = gc.handler.playerMobId;
        sge.player = player.sprite;
        evidenceHandler.AddSmokeGrenadeEvidence(sge);
        SmokeGrenadeActivate message = new SmokeGrenadeActivate();
        message.pos = player.transform.position;
        gc.handler.link.Send(message);
        SetItem(0);
    }

    private void KnifeClick()
    {
        gc.Kill();

        gc.knifeItem = false;
        SetItem(0);
    }

    private void PulseClick()
    {
        pcUI.StartCountdown(PcCD);
        SetItem(0);
    }

    //set item based on Enum int
    public void SetItem(int item)
    {
        myItem = (Item)item;

        text2.text = "Item";
        myItemImage.enabled = true;
        backgroundImage.enabled = true;
        infoImage.enabled = true;
        Exit = () => Empty();
        Enter = () => Empty();
        switch (myItem)
        {
            case Item.None:
                Debug.Log("None");
                Click = () => NoneClick();
                myItemImage.enabled = false;
                backgroundImage.enabled = false;
                infoImage.enabled = false;
                text2.text = "";
                infoText.text = "";
                break;
            case Item.Camera:
                Click = () => CameraClick();
                myItemImage.sprite = CameraSprite;
                photosTaken = 0;
                infoText.text = "Takes a picture that can be presented during meetings";
                break;
            case Item.MotionSensor:
                Click = () => MotionSensorClick();
                myItemImage.sprite = buttonMotionSensorImage;
                Exit = () => MotionSensorExit();
                Enter = () => MotionSensorEnter();
                infoText.text = "Puts out a motion sensor. Each other player that walks through it will be compiled into a list that can be presented during meetings";
                break;
            case Item.SmokeGrenade:
                Click = () => SmokeGrenadeClick();
                myItemImage.sprite = SmokeGrenadeSprite;
                infoText.text = "Throws a smoke grenade that covers the area around you with smoke. Hide yourself or distract other players with it";
                break;
            case Item.Knife:
                Click = () => KnifeClick();
                Exit = () => KnifeExit();
                Enter = () => KnifeEnter();
                myItemImage.sprite = KnifeSprite;
                infoText.text = "Knife. Lets you kill another player. Even if your not a spy. Use does not affect kill cooldown";
                break;
            case Item.PulseChecker:
                Click = () => PulseClick();
                myItemImage.sprite = pulseSprite;
                infoText.text = "Pulse Machine. Revealse all enemies around you for its duration. If you find a body while PC is in use you will be able to present the age of the body (In seconds) as evidence";
                break;
        }
    }
    //Detect if the Cursor starts to pass over the GameObject
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        Enter();
    }

    //Detect when Cursor leaves the GameObject
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        Exit();
    }

    void MotionSensorEnter()
    {
        gc.player.MotionSensorCheck = true;
    }
    void MotionSensorExit()
    {
        gc.player.MotionSensorCheck = false;
        gc.player.ResetArrows();
    }

    void KnifeEnter()
    {
        Debug.Log("KnifeEnter");
        gc.knifeItem = true;
    }
    void KnifeExit()
    {
        gc.knifeItem = false;
    }

    void Empty()
    {
        Debug.Log("Empty");
    }
    //Start of game cleanup
    public void PhaseChanged(EventCallbacks.Event eventInfo)
    {
        PhaseChangedEvent pc = (PhaseChangedEvent)eventInfo;

        if (pc.previous == GamePhase.Setup)
        {

            motionSensorNumber = 1;
            int r = UnityEngine.Random.Range(1, (Enum.GetValues(typeof(Item)).Length - 1));

            Debug.Log((Enum.GetValues(typeof(Item)).Length - 1) + "Enum");
            SetItem(2);
        }
        if(pc.phase == GamePhase.Setup)
        {
            MotionSensorUI.SetActive(false);
        }
    }
}
