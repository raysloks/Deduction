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

    public void LockEvidencePicture(Texture texture)
    {
        if (sentEvidence == false)
        {
            sentEvidence = true;
            Texture2D texCopy = (ri.texture as Texture2D);
            Texture2D texCopy2 = (texture as Texture2D);
            texCopy = Resize(texCopy, texCopy.width / 10, texCopy.height / 10);
            byte[] b = texCopy.EncodeToPNG();
            byte[] b2 = texCopy2.EncodeToPNG();
;
            if (b != null && b.Length != 0)
            {

                Debug.Log(b.Length + " b1 Normal Length b2 " + b2.Length);

                Debug.Log(Compress(b).Length + " b1 Compress Length b2 " + Compress(b2).Length);

                gc.SendEvidence(b);

            }
            else
            {
                Debug.Log("Null or 0");
            }
        }
    }

    Texture2D Resize(Texture2D texture2D, int targetX, int targetY)
    {
        RenderTexture rt = new RenderTexture(targetX, targetY, 24);
        RenderTexture.active = rt;
        Graphics.Blit(texture2D, rt);
        Texture2D result = new Texture2D(targetX, targetY, TextureFormat.RGB24, false);

        texture2D.filterMode = FilterMode.Bilinear;
        texture2D.wrapMode = TextureWrapMode.Clamp;
        result.ReadPixels(new Rect(0, 0, targetX, targetY), 0, 0);
        result.Apply();
        return result;
    }

    public static string CompressString(string text)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(text);
        var memoryStream = new MemoryStream();
        using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
        {
            gZipStream.Write(buffer, 0, buffer.Length);
        }

        memoryStream.Position = 0;

        var compressedData = new byte[memoryStream.Length];
        memoryStream.Read(compressedData, 0, compressedData.Length);

        var gZipBuffer = new byte[compressedData.Length + 4];
        Buffer.BlockCopy(compressedData, 0, gZipBuffer, 4, compressedData.Length);
        Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gZipBuffer, 0, 4);
        return Convert.ToBase64String(gZipBuffer);
    }

    

    public static byte[] Compress(byte[] data)
    {

        MemoryStream output = new MemoryStream();
        using (DeflateStream dstream = new DeflateStream(output, System.IO.Compression.CompressionLevel.Optimal))
        {
            dstream.Write(data, 0, data.Length);
        }
        return output.ToArray();
    }

    public static byte[] Compress2(byte[] inputData)
    {

        using (var compressIntoMs = new MemoryStream())
        {
            using (var gzs = new BufferedStream(new GZipStream(compressIntoMs,
             CompressionMode.Compress), 1))
            {
                gzs.Write(inputData, 0, inputData.Length);
            }
            return compressIntoMs.ToArray();
        }
    }

    public static byte[] Decompress(byte[] data)
    {
        MemoryStream input = new MemoryStream(data);
        MemoryStream output = new MemoryStream();
        using (DeflateStream dstream = new DeflateStream(input, CompressionMode.Decompress))
        {
            dstream.CopyTo(output);
        }
        return output.ToArray();
    }

    public static Texture2D RenderMaterial(ref Material material, Vector2Int resolution, string filename = "")
    {
        RenderTexture renderTexture = RenderTexture.GetTemporary(resolution.x, resolution.y);
        Graphics.Blit(null, renderTexture, material);

        Texture2D texture = new Texture2D(resolution.x, resolution.y, TextureFormat.ARGB32, false);
        texture.filterMode = FilterMode.Bilinear;
        texture.wrapMode = TextureWrapMode.Clamp;
        RenderTexture.active = renderTexture;
        texture.ReadPixels(new Rect(Vector2.zero, resolution), 0, 0);
        RenderTexture.active = null;
        RenderTexture.ReleaseTemporary(renderTexture);

        return texture;
    }

}
