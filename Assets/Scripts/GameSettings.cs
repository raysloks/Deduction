﻿using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameSettings : MonoBehaviour
{
    public List<GameSetting> settings = new List<GameSetting>
    {
        new GameSettingInteger{ name = "Impostor Count", value = 1 },
        new GameSettingTime{name = "Kill Cooldown" , value = 30000000000 },
        new GameSettingTime{name = "Vote Time" , value = 30000000000 },
        new GameSettingTime{name = "Discussion Time" , value = 90000000000 },
        new GameSettingBoolean{name = "Test", value = 0 }
    };

    public RectTransform window;
    public GameObject textFieldPrefab;
    public GameObject togglePrefab;

    private List<Selectable> inputs = new List<Selectable>();

    public GameController controller;

    private void Awake()
    {
        for (int i = 0; i < settings.Count; ++i)
        {
            GameSetting setting = settings[i];
            Selectable input;
            GameObject go;
            int index = i;
            if (setting is GameSettingBoolean boolean)
            {
                go = Instantiate(togglePrefab, window);
                var toggle = go.GetComponentInChildren<Toggle>();
                toggle.isOn = setting.value != 0;
                toggle.onValueChanged.AddListener((bool value) =>
                {
                    setting.value = value ? 1 : 0;
                    GameSettingSet message;
                    message.setting = (ushort)index;
                    message.value = setting.value;
                    controller.handler.link.Send(message);
                });
                input = toggle;
            }
            else
            {
                go = Instantiate(textFieldPrefab, window);
                var inputField = go.GetComponentInChildren<InputField>();
                inputField.text = setting.Get();
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
                input = inputField;
            }
            go.GetComponentInChildren<Text>().text = setting.name;
            inputs.Add(input);
        }
    }

    private void Update()
    {
        window.gameObject.SetActive(controller.phase == GameController.GamePhase.Setup);
    }

    public void SetSetting(int setting, long value)
    {
        settings[setting].value = value;
        UpdateInputDisplay(setting);
    }

    private void UpdateInputDisplay(int setting)
    {
        var input = inputs[setting];
        if (input is InputField inputField)
            inputField.SetTextWithoutNotify(settings[setting].Get());
        if (input is Toggle toggle)
            toggle.SetIsOnWithoutNotify(settings[setting].value != 0);
    }
}