using System;
using System.Collections.Generic;
using Java.Lang;
//Tätä voi myöhemmin laajentaa ominaisuuksilla jota DocStarterin projekteissa käytetään jos tarvitsee --> voisi oikeastaan tehdä ihan json-serializoinnilla suoraan kannasta?
public class Project: Java.Lang.Object
{
   public string Name { get; set; }
   public string CaseId { get; set; } //DocStarterin projektiID
   //string GUID; //uniikki ID varoiksi jos tarvii --> tällä voisi myöhemmin etsiä projektille kuuluvan valvontakäyntilistan
  // public List<MonitoringVisit> Visits; //lista projektille tehdyistä valvontakäynneistä

    //public Project(string name, string id)
    //{
    //    Name = name;
    //    CaseId = id;
    //    Guid guid = Guid.NewGuid();
    //    GUID = $"{guid}";
    //}


}