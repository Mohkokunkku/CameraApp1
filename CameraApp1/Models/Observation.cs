using Android.Graphics;
using System;
using Android.Net;
using Uri = Android.Net.Uri;
//Tämä on yksi kuva-teksti -havainto joka on otettu kentällä
public class Observation { 
    public string observation { get; set; }
    public Uri imageuri { get; }
    string guid;

    public Observation(Uri uri, string captiontext = "")
    {
        observation = captiontext;
        imageuri = uri;

        Guid id = Guid.NewGuid();
        guid = $"{id}";
    }

}