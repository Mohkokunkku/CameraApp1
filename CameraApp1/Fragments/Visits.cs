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
using Android.Support.V7.App;
using Android.Support.V7.RecyclerView;


namespace CameraApp1.Fragments
{
    public class VisitsFragment : ListFragment
    {
        public string caseId { get; set; }
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetHasOptionsMenu(true);
            // Create your fragment here
        }

        public override void OnResume()
        {
            ReFreshView();
            base.OnResume();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            if (Arguments != null)
            {
                string caseid = Arguments.GetString("case");
                caseId = caseid;
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
                    empty.visitname = "EI VALVONTAKÄYNTEJÄ";
                    javavisits.Add(empty);
                }


                this.ListAdapter = new Models.VisitsAdapter(Android.App.Application.Context, javavisits);

                ((AppCompatActivity)Activity).SupportActionBar.SetTitle(Resource.String.visits_title);
             

            }

            return base.OnCreateView(inflater, container, savedInstanceState);
        }
        //LISÄÄ TOOLBARIN +-NÄPPÄIMEN UUDEN VALVONTAKÄYNNIN LISÄYSTÄ VARTEN
        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            this.Activity.MenuInflater.Inflate(Resource.Menu.project_menu, menu);
            base.OnCreateOptionsMenu(menu, inflater);
        }

        //AVAA KÄYTTÄJÄLLE VALITUN VALVONTAKÄYNNIN HAVAINTOSISÄLLÖN
        public override void OnListItemClick(ListView l, View v, int position, long id)
        {
            base.OnListItemClick(l, v, position, id);
            MonitoringVisit visit = (MonitoringVisit)this.ListAdapter.GetItem(position); //ListAdapteriin vois tehdä ihan customfunkkarin palauttamaan guidin?
            ObservationFragment observations = new ObservationFragment();
            Bundle args = new Bundle();
            args.PutString("visitguid", visit.visitguid);
            observations.Arguments = args;
           
            FragmentTransaction transaction = this.Activity.FragmentManager.BeginTransaction();
            transaction.Replace(Resource.Id.fragment_placeholder, observations, "observation");
            transaction.AddToBackStack(null);
            transaction.Commit();
        }

        public void ReFreshView()
        {
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "database.docstarter");

            SQLiteConnection db = new SQLiteConnection(dbPath);

            List<MonitoringVisit> visits = db.Table<MonitoringVisit>().Where(s => s.casenumber == caseId).ToList();
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
                empty.visitname = "EI VALVONTAKÄYNTEJÄ";
                javavisits.Add(empty);
            }


            this.ListAdapter = new Models.VisitsAdapter(Android.App.Application.Context, javavisits);
        }

        

    }   
}