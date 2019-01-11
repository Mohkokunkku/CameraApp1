using Android.Runtime;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;

public static class LocalDB
{
    //public SQLiteConnection db;

    public static void AddProjectsTest(ref SQLiteConnection db)
    {


        db.InsertAll(AddTestProjects());
        db.InsertAll(AddTestMonitorings());
        
    }

    //public static List<Project> GetProjects()
    //{
    //    //var projects = (JavaList)db.Table<Project>();
    //    return db.Table<Project>().ToList();
    //}

    public static List<Project> AddTestProjects()
    {
        Project project1 = new Project();
        project1.CaseId = "345";
        project1.Name = "Aleksiskiven Katu 50";

        Project project2 = new Project();
        project2.CaseId = "655";
        project2.Name = "Ruosilankuja 3";

        Project project3 = new Project();
        project3.CaseId = "1008";
        project3.Name = "Kolmaslinja 7";

        return new List<Project> { project1, project2, project3};
    }

    public static List<MonitoringVisit> AddTestMonitorings()
    {
        MonitoringVisit visit1 = new MonitoringVisit();
        visit1.name = "Viikko 11";
        visit1.casenumber = "345";

        MonitoringVisit visit2 = new MonitoringVisit();
        visit2.name = "Viikko 12";
        visit2.casenumber = "345";

        MonitoringVisit visit3 = new MonitoringVisit();
        visit3.name = "Viikko 13";
        visit3.casenumber = "345";

        return new List<MonitoringVisit>()
            {
                visit1, visit2, visit3
            };
    }

    
    }
