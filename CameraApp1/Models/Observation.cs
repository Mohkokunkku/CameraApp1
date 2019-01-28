using Android.Graphics;
using System;
using Android.Net;
using Uri = Android.Net.Uri;
using SQLite;
//Tämä on yksi kuva-teksti -havainto joka on otettu kentällä
[Table("")]
public class Observation : Java.Lang.Object, IObservation
{ 
    [PrimaryKey, AutoIncrement]
    public int pkId { get; set; }
    public string observation { get; set; }
    public string absolutepath { get; set; }
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