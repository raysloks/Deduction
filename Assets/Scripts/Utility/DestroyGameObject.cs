using UnityEngine;
using System.Collections;

public class DestroyGameObject : MonoBehaviour
{
    public void Destroy()
    {
        Destroy(gameObject);
    }
}
