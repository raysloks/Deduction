using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CopyCodeButton : MonoBehaviour
{
    public GameController game;

    private Text text;

    private void Awake()
    {
        text = GetComponentInChildren<Text>();
    }

    private void Update()
    {
        text.text = GUIUtility.systemCopyBuffer == game.matchmaker.lobby ? "Copied!" : "Copy";
    }

    public void CopyCodeToClipboard()
    {
        GUIUtility.systemCopyBuffer = game.matchmaker.lobby;
    }
}
