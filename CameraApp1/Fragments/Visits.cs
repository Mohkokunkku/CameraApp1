using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SQLite;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace CameraApp1.Fragments
{
    public class VisitsFragment : ListFragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            if (Arguments != null)
            {
                string caseid = Arguments.GetString("case");

                string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "database.docstarter");

                SQLiteConnection db = new SQLiteConnection(dbPath);

                List<MonitoringVisit> visits = db.Table<MonitoringVisit>().Where(s => s.casenumber == caseid).ToList();
                JavaList<MonitoringVisit> javavisits = new JavaList<MonitoringVisit>();
                if (visits.Count > 0)
                {
                    foreach (var item in visits)
                    {
                        javavisits.Add(item);
                    }
                }
                else
                {
                    MonitoringVisit empty = new MonitoringVisit();
                    empty.name = "EI VALVONTAKÄYNTEJÄ";
                    javavisits.Add(empty);
                }


                this.ListAdapter = new Models.VisitsAdapter(Android.App.Application.Context, javavisits);
            }

            return base.OnCreateView(inflater, container, savedInstanceState);
        }
    }   
}