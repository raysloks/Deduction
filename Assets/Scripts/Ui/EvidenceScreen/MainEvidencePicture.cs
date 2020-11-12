using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.IO.Compression;
using UnityEngine.UI;
using System.Drawing;
using System.Text;
using System;

public class MainEvidencePicture : MonoBehaviour
{
    public GameObject MainPicture;
    public GameController gc;
    private RawImage ri;
    private bool sentEvidence = false;
    // Start is called before the first frame update
    void Start()
    {
        ri = MainPicture.GetComponent<RawImage>();
    }

    public void SetMainPicture(Texture tex)
    {
        ri.texture = tex;
    }

    public void LockEvidencePicture(List<Vector3> pos)
    {
        if (sentEvidence == false)
        {
            sentEvidence = true;
            gc.SendEvidence(pos);
        }
    }

}
