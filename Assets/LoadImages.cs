using Assets;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LoadImages : MonoBehaviour
{
    private static string ActiveGallery = "";
    // Start is called before the first frame update
    void Start()
    {
        //Load the images from the galley json file
        string StearmingPath = "assets/StreamingAssets/";
        string JsonFileName = "gallery.json";
        var json = File.ReadAllText(StearmingPath + JsonFileName);
        var GalleryItems = JsonHelper.ReadGalleryJson(json);

        //find the content of the scroll view to append the images
        var ScrollView = GameObject.Find("Content");
        
        //loop the images and append to UI
        foreach (var item in GalleryItems.objects)
        {
            if (item.galleryId != ActiveGallery)
                continue;

            foreach (var item2 in item.items)
            {
                //initial image object
                GameObject imgObject = new GameObject("GalleryItem");

                RectTransform trans = imgObject.AddComponent<RectTransform>();
                trans.transform.SetParent(ScrollView.transform); // setting parent
                trans.localScale = Vector3.one;
                trans.anchoredPosition = new Vector2(0f, 0f); // setting position, will be on center                              

                Image image = imgObject.AddComponent<Image>();                
                image.sprite = this.LoadNewSprite(StearmingPath+item2.Path);

                //calc width and keep aspect ratio.
                var aspectRatio = (image.sprite.rect.width / image.sprite.rect.height);
                var newHeight = ((Screen.width-17) / aspectRatio); //-17 to remove the reserved area by scrollbars.
                trans.sizeDelta = new Vector2((Screen.width - 17), newHeight); // custom size
                image.preserveAspect = true;

                //append to ui
                imgObject.transform.SetParent(ScrollView.transform);
                
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MenuClick(string GalleryName)
    {
        ActiveGallery = GalleryName;
        var Gallery = GameObject.Find("Gallery");
        Gallery.transform.Find("Canvas").gameObject.SetActive(true);
        GameObject.Find("Buttons").gameObject.SetActive(false);
    }

    // extra functions to convert image to sprite
    public Sprite LoadNewSprite(string FilePath, float PixelsPerUnit = 100.0f)
    {
        Sprite NewSprite;
        Texture2D SpriteTexture = LoadTexture(FilePath);
        NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0), PixelsPerUnit);

        return NewSprite;
    }
    public Texture2D LoadTexture(string FilePath)
    {
        Texture2D Tex2D;
        byte[] FileData;

        if (File.Exists(FilePath))
        {
            FileData = File.ReadAllBytes(FilePath);
            Tex2D = new Texture2D(2, 2);           // Create new "empty" texture
            if (Tex2D.LoadImage(FileData))           // Load the imagedata into the texture (size is set automatically)
                return Tex2D;                 // If data = readable -> return texture
        }
        return null;                     // Return null if load failed
    }
}
