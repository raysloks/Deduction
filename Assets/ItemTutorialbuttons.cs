using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemTutorialbuttons : MonoBehaviour
{
    //General
    public TextMeshProUGUI immediateUse;
    public TextMeshProUGUI meetingUse;
    public Image Screenshot1;
    public Image Screenshot2;
    public TextMeshProUGUI ScreenshotText1;
    public TextMeshProUGUI ScreenshotText2;
    public TextMeshProUGUI itemName;
    



    //Smoke Grenade
    public Sprite SmokeGrenadeScreenshot1;
    public Sprite SmokeGrenadeScreenshot2;
    private string smokeString = "When clicked this items fire a smoke grenade. You can hide in the smoke or distract other players with it";
    private string smokeString2 = "During meetings you can present an area of where you fired your last smoke grenade. Use it to confirm that you've been in an area";

    //Camera
    public Sprite CameraScreenshot1;
    public Sprite CameraScreenshot2;
    private string cameraString = "When clicked this item takes a screenshot";
    private string cameraString2 = "The screenshots taken can be presented to other players during a meetings.";

    // Motion Sensor
    public Sprite MotionSensorScreenshot1;
    public Sprite MotionSensorScreenshot2;
    private string MotionSensorString = "When clicked this item places a motion sensor in the level. You will be notified when another player passes the motion sensor and they will be added to a list. Each motion sensor has an index number so you can tell them apart in and out of meetings";
    private string MotionSensorString2 = "To the other players you can present a list of everyone who walked through your motion sensor during this round. The motion sensor does not disapear after a meeting but its list gets reset. ";

    //Pulse Checker
    public Sprite PulseCheckerScreenshot1;
    public Sprite PulseCheckerScreenshot2;
    private string pulseString = "When clicked this item lets you see players through walls and even when hidden. Lasts for 20 seconds";
    private string pulseString2 = "If you find a body while this item is active. You can present the age of the body (in seconds) to the other players";


    public void pulseCheckerClick()
    {
        Screenshot1.gameObject.SetActive(true);
        Screenshot2.gameObject.SetActive(true);

        Screenshot1.sprite = PulseCheckerScreenshot1;
        Screenshot2.sprite = PulseCheckerScreenshot2;
        immediateUse.text = pulseString;
        meetingUse.text = pulseString2;
        ScreenshotText1.text = "A Screenshot of pulse checker in use";
        ScreenshotText2.text = "A Screenshot of pulse checker evidence";
        itemName.text = "Pulse Checker";
    }
    public void MotionSensorClick()
    {
        Screenshot1.gameObject.SetActive(true);
        Screenshot2.gameObject.SetActive(true);

        Screenshot1.sprite = MotionSensorScreenshot1;
        Screenshot2.sprite = MotionSensorScreenshot2;
        immediateUse.text = MotionSensorString;
        meetingUse.text = MotionSensorString2;

        ScreenshotText1.text = "A screenshot of a place motion sensor";
        ScreenshotText2.text = "A screenshot of motion sensor evidence";
        itemName.text = "Motion Sensor";
    }
    public void cameraClick()
    {
        Screenshot1.gameObject.SetActive(true);
        Screenshot2.gameObject.SetActive(true);

        Screenshot1.sprite = CameraScreenshot1;
        Screenshot2.sprite = CameraScreenshot2;
        immediateUse.text = cameraString;
        meetingUse.text = cameraString2;

        ScreenshotText1.text = "A screenshot of a player taking a picture";
        ScreenshotText2.text = "A screenshot of picture evidence";
        itemName.text = "Camera";
    }
    public void SmokeGrenadeClick()
    {

        Screenshot1.gameObject.SetActive(true);
        Screenshot2.gameObject.SetActive(true);

        Screenshot1.sprite = SmokeGrenadeScreenshot1;
        Screenshot2.sprite = SmokeGrenadeScreenshot2;
        immediateUse.text = smokeString;
        meetingUse.text = smokeString2;
        ScreenshotText1.text = "A screenshot of the smoke grenade in use";
        ScreenshotText2.text = "A screenshot of smoke grenade evidence";
        itemName.text = "Smoke Grenade";
    }
    public void backItemClick()
    {
        Screenshot1.gameObject.SetActive(false);
        Screenshot2.gameObject.SetActive(false);
        immediateUse.text = "Here is a description of the immediate use of an item";
        meetingUse.text = "Here is a description of the use of an items evidence during meetings";
        ScreenshotText1.text = "";
        ScreenshotText2.text = "";
        itemName.text = "";
    }
}
