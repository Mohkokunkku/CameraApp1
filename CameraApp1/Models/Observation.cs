using Android.Graphics;
using System;
//Tämä on yksi kuva-teksti -havainto joka on otettu kentällä
public class Observation { 
    string observation;
    Bitmap bitmap;
    string guid;

    public Observation(string captiontext, Bitmap picture)
    {
        observation = captiontext;
        bitmap = picture;

        Guid id = Guid.NewGuid();
        guid = $"{id}";
    }

}