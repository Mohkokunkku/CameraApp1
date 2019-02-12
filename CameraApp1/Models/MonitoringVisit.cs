using SQLite;
using System;
using System.Collections.Generic;

//Pohdista: Onko tämä Visits tarpeellinen vai pitäisikö laittaa projektille vain List<MonitoringVisit> 
//--> tekee ehkä vain yhden turhan mutkan matkalla jos homma pysyy simppelinä listana
//toisaalta lista olisi hyvä tallentaa erikseen? no katsotaan 
//public class Visits
//{
//    public List<MonitoringVisit> GetVisits = new List<MonitoringVisit>();
//    public string caseId;
//    public string GUID;
//    public Visits(List<MonitoringVisit> getVisits, string caseId)
//    {
//        GetVisits = getVisits;
//        this.caseId = caseId;

//    }
//}
[Table("")]
public class MonitoringVisit : Java.Lang.Object, IMonitoringVisit
{
   // public List<Observation> observations = new List<Observation>();
  
    public string visitguid { get; set; }
    //private int number;
    public string casenumber { get; set; }
    public string casename { get; set; }
    //private string projectGUID;
    public string visitname { get; set; }
    [PrimaryKey, AutoIncrement]
    public int pkId { get; set; }
}

public class SendMonitoringVisit : IMonitoringVisit
{
    public string casenumber { get; set; }
    public string visitguid { get; set; }
    public string visitname { get; set; }
    public int pkId { get; set; }
    public string casename { get; set; }
}