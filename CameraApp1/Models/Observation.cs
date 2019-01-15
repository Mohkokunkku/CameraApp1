using Android.Graphics;
using System;
using Android.Net;
using Uri = Android.Net.Uri;
//Tämä on yksi kuva-teksti -havainto joka on otettu kentällä
public class Observation : Java.Lang.Object { 
    public string observation { get; set; }

    public string imageuri { get; set; }

    public string observationguid; //tämä on kuvahavainnon oma guid

    public string visitguid { get; set; } //tämä kertoo mihin valvontakäyntiin kuvahavaintokuuluu
    //public Observation(Uri uri, string captiontext = "")
    //{
    //    observation = captiontext;
    //    imageuri = uri;

    //    Guid id = Guid.NewGuid();
    //    guid = $"{id}";
    //}

}