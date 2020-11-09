using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class MainEvidencePicture : MonoBehaviour
{
    public GameObject MainPicture;
    private RawImage ri;
    // Start is called before the first frame update
    void Start()
    {
        ri = MainPicture.GetComponent<RawImage>();
    }

    public void SetMainPicture(Texture tex)
    {
        ri.texture = tex;
    }
}
