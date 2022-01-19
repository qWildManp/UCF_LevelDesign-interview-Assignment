using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class playGIF : MonoBehaviour
{
    private RawImage image;
    private List<UniGif.GifTexture> textureList;
    int index = -1;
    float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<RawImage>();
        string filePath = Application.dataPath + "/UI/heartbeat.gif";
        Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        byte[] img_bytes = new byte[stream.Length];
        stream.Read(img_bytes, 0, (int)stream.Length);
        StartCoroutine(UniGif.GetTextureListCoroutine(img_bytes,LoadGifHandler));
    }
    void LoadGifHandler(List<UniGif.GifTexture> list,int count,int wdith,int height)
    {
        textureList = list;
        index = 0;
    }
    // Update is called once per frame
    void Update()
    {
        if (index == -1)
            return;
        timer += Time.deltaTime;
        if(timer >= 0.05f)
        {
            timer = 0;
            image.texture = textureList[index++].m_texture2d;
            if (index >= textureList.Count)
                index = 0;
        }
    }
}
