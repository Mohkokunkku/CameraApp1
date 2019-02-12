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


        //db.InsertAll(AddTestProjects());
        db.InsertAll(AddTestMonitorings());
        
    }

    public static void DeleteObservation(IObservation observation)
    {
        string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "database.docstarter");
        SQLiteConnection db = new SQLiteConnection(dbPath);
        db.Delete<Observation>(observation.pkId);
        
        //poistaa kuvan kännykän muistista
        if (File.Exists(observation.absolutepath))
        {
            File.Delete(observation.absolutepath);
        }
    }

    public static void UpdateObservation(IObservation observation)
    {
        string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "database.docstarter");
        SQLiteConnection db = new SQLiteConnection(dbPath);
        db.InsertOrReplace(observation);
    }

    public static void UpdateProjects(List<Project> onlineprojects)
    {
        //Pitäiskö nyt alkuun tehdä vaan niin että droppais koko tablen jos projektilista on päivittynyt ja loisi sen uusiksi?

        try
        {
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "database.docstarter");
            SQLiteConnection db = new SQLiteConnection(dbPath);
            db.DropTable<Project>();

            db.CreateTable<Project>();

            foreach (var onlineproject in onlineprojects)
            {
                db.InsertOrReplace(onlineproject); 
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Virhe sqliten päivittämisessä" + ex.Message);
            throw;
        }

    }

    public static void DeleteVisit(IMonitoringVisit visit)
    {

        try
        {
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "database.docstarter");
            var visitguid = visit.visitguid;
            SQLiteConnection db = new SQLiteConnection(dbPath);
            db.CreateTable<MonitoringVisit>();

            //TUHOAA VIIKKOKÄYNNIN LISTASTA
            db.Delete<MonitoringVisit>(visit.pkId);
            db.CreateTable<Observation>();

            //HAKEE LISTAN KUVISTA JA TUHOAA NE TIETOKANNASTA FOREACH-LOOPISSA
            List<Observation> observations = db.Table<Observation>().Where(s => s.visitguid == visitguid).ToList();
            foreach (var item in observations)
            {
                db.Table<Observation>().Where(x => x.visitguid == item.visitguid).Delete();
                if (File.Exists(item.absolutepath))
                {
                    File.Delete(item.absolutepath);
                    if (File.Exists(item.absolutepath) == false)
                    {
                        Console.WriteLine("Kuva poistettu onnistuneesti");
                    }
                    else
                    {
                        Console.WriteLine("Kuvan poistossa häikkää");
                    }
                }
                else
                {
                    Console.WriteLine("Valokuvan poisto muistista ei onnistunut");
                }
            }
            
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
        //List<MonitoringVisit> visits = db.Table<MonitoringVisit>().Where(s => s.casenumber == caseId).ToList();
    }
    //public static List<Project> GetProjects()
    //{
    //    //var projects = (JavaList)db.Table<Project>();
    //    return db.Table<Project>().ToList();
    //}

    //public static List<Project> AddTestProjects()
    //{
    //    Project project1 = new Project();
    //    project1.caseId = "345";
    //    project1.name = "Aleksiskiven Katu 50";

    //    Project project2 = new Project();
    //    project2.caseId = "655";
    //    project2.name = "Ruosilankuja 3";

    //    Project project3 = new Project();
    //    project3.caseId = "1008";
    //    project3.name = "Kolmaslinja 7";

    //    return new List<Project> { project1, project2, project3};
    //}

    public static List<MonitoringVisit> AddTestMonitorings()
    {
        MonitoringVisit visit1 = new MonitoringVisit();
        visit1.visitname = "Viikko 11";
        visit1.casenumber = "345";

        MonitoringVisit visit2 = new MonitoringVisit();
        visit2.visitname = "Viikko 12";
        visit2.casenumber = "345";

        MonitoringVisit visit3 = new MonitoringVisit();
        visit3.visitname = "Viikko 13";
        visit3.casenumber = "345";

        return new List<MonitoringVisit>()
            {
                visit1, visit2, visit3
            };
    }



    
    }
