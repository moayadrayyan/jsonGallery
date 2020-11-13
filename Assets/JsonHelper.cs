using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets
{
    public static class JsonHelper
    {
        public static Gallery ReadGalleryJson(string json)
        {
            return JsonUtility.FromJson<Gallery>("{ \"objects\":"+json+"}");
        }
        [Serializable]
        public class Gallery
        {
            public List<Galleries> objects;
        }
        [Serializable]
        public class Galleries
        {
            public string galleryId;
            public List<items> items;
        }
        [Serializable]
        public class items
        {
            public string ItemId;
            public string Path;
        }
    }
}
