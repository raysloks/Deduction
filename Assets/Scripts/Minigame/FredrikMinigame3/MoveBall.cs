using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBall : MonoBehaviour
{
    Vector3 oldMousePosition;

    float xMax;
    float xMin;
    float yMax;
    float yMin;

    void Awake()
    {
        GetBounds();
    }
    void Start()
    {
        Vector2 currentWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = (Vector3)currentWorldPos;
        GetBounds();
    }
    // Update is called once per frame
    void Update()
    {
    //    Vector2 oldWorldPos = Camera.main.ScreenToWorldPoint(oldMousePosition);
        Vector2 currentWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = (Vector3)currentWorldPos;
     //   transform.position = ClampedVector3(transform.position);
        oldMousePosition = Input.mousePosition;
    }

    private Vector2 ClampedVector3(Vector3 v)
    {
        Vector2 clampedV = v;
        clampedV.x = Mathf.Min(Mathf.Max(clampedV.x, xMin), xMax);
        clampedV.y = Mathf.Min(Mathf.Max(clampedV.y, yMin), yMax);
        return clampedV;
    }

    private void GetBounds()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Sprite sprite = sr.sprite;

        // x and y Units are how many units it takes to get to edge
        // of the screen from the middle (0,0)
        float yUnits = Camera.main.orthographicSize;
        float xUnits = yUnits * Camera.main.aspect;

        // figure out how much of the sprite is offscreen in Units
        float xRight = sprite.texture.width - sprite.pivot.x;
        xRight *= transform.localScale.x;
        xRight /= sprite.pixelsPerUnit;
        xMin = -(xRight - xUnits);

        float xLeft = sprite.pivot.x;
        xLeft *= transform.localScale.x;
        xLeft /= sprite.pixelsPerUnit;
        xMax = (xLeft - xUnits);

        float yUp = sprite.texture.height - sprite.pivot.y;
        yUp *= transform.localScale.y;
        yUp /= sprite.pixelsPerUnit;
        yMin = -(yUp - yUnits);

        float yDown = sprite.pivot.y;
        yDown *= transform.localScale.y;
        yDown /= sprite.pixelsPerUnit;
        yMax = yDown - yUnits;
    }
}
