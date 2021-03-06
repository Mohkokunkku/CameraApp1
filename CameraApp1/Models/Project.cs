﻿using System;
using System.Collections.Generic;
using Java.Lang;
//Tätä voi myöhemmin laajentaa ominaisuuksilla jota DocStarterin projekteissa käytetään jos tarvitsee --> voisi oikeastaan tehdä ihan json-serializoinnilla suoraan kannasta?

public class Project: Java.Lang.Object
{
    public string name { get; set; }

    public string caseId { get; set; }

}

public class JavaProject: Java.Lang.Object
{
    public string name { get; set; }

    public string caseId { get; set; }

    public JavaProject(string name, string caseId)
    {
        this.name = name;
        this.caseId = caseId;
    }
}


public class Projects: Java.Lang.Object
{
    public List<Project> projects { get; set; }
    
}