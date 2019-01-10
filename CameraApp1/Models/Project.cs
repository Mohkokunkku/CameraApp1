using System;
using Java.Lang;
//Tätä voi myöhemmin laajentaa ominaisuuksilla jota DocStarterin projekteissa käytetään jos tarvitsee --> voisi oikeastaan tehdä ihan json-serializoinnilla suoraan kannasta?
public class Project: Java.Lang.Object
{
   public string Name;
   public string CaseId; //DocStarterin projektiID
    string GUID; //uniikki ID varoiksi jos tarvii 
    

    public Project(string name, string id)
    {
        Name = name;
        CaseId = id;
        Guid guid = Guid.NewGuid();
        GUID = $"{guid}";
    }


}