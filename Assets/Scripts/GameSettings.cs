using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    public List<GameSetting> settings = new List<GameSetting>
    {
        new GameSettingInteger{ name = "Impostor Count", value = 1 },
        new GameSettingTime{name = "Kill Cooldown" , value = 30000000000 },
        new GameSettingTime{name = "Vote Time" , value = 30000000000 },
        new GameSettingTime{name = "Discussion Time" , value = 90000000000 }
    };

    public RectTransform window;
    public GameObject prefab;

    private List<InputField> inputFields = new List<InputField>();

    public GameController controller;

    private void Awake()
    {
        for (int i = 0; i < settings.Count; ++i)
        {
            GameSetting setting = settings[i];
            var go = Instantiate(prefab, window);
            go.GetComponentInChildren<Text>().text = setting.name;
            var inputField = go.GetComponentInChildren<InputField>();
            inputField.text = setting.Get();
            int index = i;
            inputField.onEndEdit.AddListener((string text) =>
            {
                long prev = setting.value;
                setting.Set(text);
                if (prev != setting.value)
                {
                    GameSettingSet message;
                    message.setting = (ushort)index;
                    message.value = setting.value;
                    controller.handler.link.Send(message);
                }
                else
                {
                    inputField.SetTextWithoutNotify(setting.Get());
                }
            });
            inputFields.Add(inputField);
        }
    }

    private void Update()
    {
        window.gameObject.SetActive(controller.phase == GameController.GamePhase.Setup);
    }

    public void SetSetting(int setting, long value)
    {
        settings[setting].value = value;
        inputFields[setting].SetTextWithoutNotify(settings[setting].Get());
    }
}
