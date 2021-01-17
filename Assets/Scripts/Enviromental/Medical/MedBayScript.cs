using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MedBayScript : MonoBehaviour
{
    public GameController game;
    //public Dictionary<ulong, MedicalInformation> medicalInfo = new Dictionary<ulong, MedicalInformation>();
    public List<MedicalInformation> medicalInfo;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var n in game.handler.mobs)
        {
            if (n.Value.role == 0)
            {

            }
        }
    }

    void StartGame()
    {
        foreach (var n in game.handler.mobs)
        {
            MedicalInformation mi = new MedicalInformation();
            mi.name = n.Value.GetComponentInChildren<TextMeshPro>().text;
            //mi.head = n.Value.GetComponentInChildren<>
            //medicalInfo.Add(new MedicalInformation);
            mi.id = n.Key;
            medicalInfo.Add(mi);
        }

    }
}
