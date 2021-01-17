using UnityEngine;
using System.Collections;

public class LowFramerateCameraObserver : MonoBehaviour
{
    private void Awake()
    {
        LowFramerateCamera.observer = this;
    }
}
