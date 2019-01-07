using Android.Graphics;
using System;
//Tämä on yksi kuva-teksti -havainto joka on otettu kentällä
public class Observation { 
    string observation;
    Java.Net.URI imageuri;
    string guid;

    public Observation(string captiontext, Java.Net.URI uri)
    {
        observation = captiontext;
        imageuri = uri;

        Guid id = Guid.NewGuid();
        guid = $"{id}";
    }

}