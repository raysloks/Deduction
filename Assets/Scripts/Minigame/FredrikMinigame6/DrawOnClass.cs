using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Linq;

public class DrawOnClass : MonoBehaviour
{
    public float radius;
    public Color InitialColor;
    public TextMeshProUGUI text;

    private RaycastHit2D hitInfo;
    private bool isDone = false;
    private List<int> indexes = new List<int>();
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && isDone == false)
        {
            hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hitInfo)
            {
                if (hitInfo.collider != null)
                {
                    UpdateTexture();

                }
            }
            
           
        }
        if (Input.GetMouseButtonUp(0) && isDone == false)
        {
            Resources.UnloadUnusedAssets();
            SpriteRenderer mySpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            Texture2D texture = mySpriteRenderer.sprite.texture;
            int Done = 0;
            for (int x = 0; x < texture.width; x++)
            {
                for (int y = 0; y < texture.height; y++)
                {
                    if(texture.GetPixel(x, y).a != 0f)
                    {
                        Done++;
                    }
                }
            }
            if(Done < 20)
            {
                text.text = "Done";
                isDone = true;
                FindObjectOfType<MinigamePopupScript>().MinigameWon();
            }
        }
    }

    public Texture2D CopyTexture2D(Texture2D copiedTexture2D)
    {
        float differenceX;
        float differenceY;

        //Create a new Texture2D, which will be the copy
        Texture2D texture = new Texture2D(copiedTexture2D.width, copiedTexture2D.height);
        texture.SetPixels(copiedTexture2D.GetPixels());

        int m1 = (int)((hitInfo.point.x - hitInfo.collider.bounds.min.x) * (copiedTexture2D.width / hitInfo.collider.bounds.size.x));
        int m2 = (int)((hitInfo.point.y - hitInfo.collider.bounds.min.y) * (copiedTexture2D.height / hitInfo.collider.bounds.size.y));
        
        int index = 0;
        for (int x = 0; x < copiedTexture2D.width; x++)
        {
            for (int y = 0; y < copiedTexture2D.height; y++)
            {
                index++;
                differenceX = x - m1;
                differenceY = y - m2;

                if (differenceX * differenceX + differenceY * differenceY <= radius * radius)
                {
                    if(indexes.Contains(index) == false)
                    {
                        indexes.Add(index);
                        texture.SetPixel(x, y, InitialColor);
                    }              
                }

            }
        }
        

        texture.Apply(false);
        return texture;
    }

    public void UpdateTexture()
    {
        SpriteRenderer mySpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        string oldName = mySpriteRenderer.sprite.name;
        Texture2D newTexture2D = CopyTexture2D(mySpriteRenderer.sprite.texture);

        mySpriteRenderer.sprite = Sprite.Create(newTexture2D, mySpriteRenderer.sprite.rect, new Vector2(0.5f, 0.5f));
        mySpriteRenderer.sprite.name = oldName;

    }

}
