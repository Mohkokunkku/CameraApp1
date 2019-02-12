using Android.Graphics;
using System;
using Android.Net;
using Uri = Android.Net.Uri;
using SQLite;
using Newtonsoft.Json;
//Tämä on yksi kuva-teksti -havainto joka on otettu kentällä

[Table(""), JsonObject(MemberSerialization.OptOut)]
public class Observation : Java.Lang.Object, IObservation
{ 
    
    [PrimaryKey, AutoIncrement, JsonIgnore]
    public int pkId { get; set; }
    public string observation { get; set; }
    [JsonIgnore]
    public string absolutepath { get; set; }
    public string imageuri { get; set; }
    [JsonIgnore]
    public string cachepath { get; set; }
    public string observationguid; //tämä on kuvahavainnon oma guid
    public string visitname { get; set; }
    public string visitguid { get; set; } //tämä kertoo mihin valvontakäyntiin kuvahavaintokuuluu
    //public Observation(Uri uri, string captiontext = "")
    //{
    //    observation = captiontext;
    //    imageuri = uri;

    //    Guid id = Guid.NewGuid();
    //    guid = $"{id}";
    //}

}

[Table(""), JsonObject(MemberSerialization.OptOut)]
public class SendObservation : IObservation
{

    [PrimaryKey, AutoIncrement, JsonIgnore]
    public int pkId { get; set; }
    public string observation { get; set; }
    [JsonIgnore]
    public string absolutepath { get; set; }
    public string imageuri { get; set; }
    [JsonIgnore]
    public string cachepath { get; set; }
    public string observationguid; //tämä on kuvahavainnon oma guid

    public string visitguid { get; set; } //tämä kertoo mihin valvontakäyntiin kuvahavaintokuuluu
    public string visitname { get; set; }
    //public Observation(Uri uri, string captiontext = "")
    //{
    //    observation = captiontext;
    //    imageuri = uri;

    //    Guid id = Guid.NewGuid();
    //    guid = $"{id}";
    //}

}


