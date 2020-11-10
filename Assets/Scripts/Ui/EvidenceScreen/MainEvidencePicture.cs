using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

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

    public void LockEvidencePicture()
    {
        if(sentEvidence == false)
        {
            sentEvidence = true;
            Texture2D texCopy = (ri.texture as Texture2D);
            byte[] b = texCopy.GetRawTextureData();
            List<byte> bList = new List<byte>(b);
            gc.SendEvidence(bList);
        }
    }

}
