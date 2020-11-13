using Assets;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LoadImages : MonoBehaviour
{
    //this static variable will be filled upon gallaries button click, to catch which button clicked and show the images accordengly.
    private static string ActiveGallery = "";
    // Start is called before the first frame update
    void Start()
    {
        //Load the images from the galley json file
        string StearmingPath = "assets/StreamingAssets/";
        string JsonFileName = "gallery.json";
        var json = File.ReadAllText(StearmingPath + JsonFileName);

        //conver json into objects.
        var GalleryItems = JsonHelper.ReadGalleryJson(json);

        //find the content of the scroll view to append the images
        var ScrollView = GameObject.Find("Content");

        //get selectd gallery items
        var SelectedGalleryItems = GalleryItems.objects.Where(x => x.galleryId == ActiveGallery).FirstOrDefault();
      
        //loop all images within the gallery.
        foreach (var item2 in SelectedGalleryItems.items)
        {
            //initiat the image that will be dynamically added to the UI.
            GameObject imgObject = new GameObject("GalleryItem");
            RectTransform trans = imgObject.AddComponent<RectTransform>();
            trans.transform.SetParent(ScrollView.transform); // setting parent as the scrollview
            trans.localScale = Vector3.one;
            trans.anchoredPosition = new Vector2(0f, 0f); // setting position, will be on center                              

            Image image = imgObject.AddComponent<Image>();  
                
            //convert the image into sprite.
            image.sprite = this.LoadNewSprite(StearmingPath+item2.Path);


            //calc width and keep aspect ratio.
            var aspectRatio = (image.sprite.rect.width / image.sprite.rect.height);
            var newHeight = ((Screen.width-17) / aspectRatio); //-17 to remove the reserved area by scrollbars, to avoid horizontal scrolls
            trans.sizeDelta = new Vector2((Screen.width - 17), newHeight); // width = screen, height as calculated.
            image.preserveAspect = true;

            //append to ui by setting the parent as the scrollview.
            imgObject.transform.SetParent(ScrollView.transform);
                
        }
       
    }


    // on gallery type click, hide the buttons and show the selected gallery.
    public void MenuClick(string GalleryName)
    {
        ActiveGallery = GalleryName;
        var Gallery = GameObject.Find("Gallery");
        Gallery.transform.Find("Canvas").gameObject.SetActive(true);
        GameObject.Find("Buttons").gameObject.SetActive(false);
    }

    // extra functions to convert image to sprite
    private Sprite LoadNewSprite(string FilePath, float PixelsPerUnit = 100.0f)
    {
        Sprite NewSprite;
        Texture2D SpriteTexture = LoadTexture(FilePath);
        NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0), PixelsPerUnit);

        return NewSprite;
    }
    private Texture2D LoadTexture(string FilePath)
    {
        Texture2D Tex2D;
        byte[] FileData;

        if (File.Exists(FilePath))
        {
            FileData = File.ReadAllBytes(FilePath);
            Tex2D = new Texture2D(2, 2);          
            if (Tex2D.LoadImage(FileData))
                return Tex2D;
        }
        return null;
    }
}
