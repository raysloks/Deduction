using UnityEngine;
using System.Collections;

public class PersistentUnique : MonoBehaviour
{
    private static bool exists = false;

    private void Awake()
    {
        if (!exists)
        {
            exists = true;
            DontDestroyOnLoad(gameObject);
            transform.DetachChildren();
        }
        else
        {
            foreach (Transform child in transform)
                child.gameObject.SetActive(false);
        }
        Destroy(gameObject);
    }
}
