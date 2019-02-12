using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using CameraApp1.Models.ListAdapters;
using Com.Tubb.Smrv;
using SQLite;

namespace CameraApp1.Fragments
{
    public class Fragment_Visits_Swipe_Menu : Fragment
    {
        public string caseId { get; set; }
        public string casename { get; set; }
        RecyclerView recyclerView { get; set; }
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
            SetHasOptionsMenu(true);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
 


            return inflater.Inflate(Resource.Layout.swipe_menu_visits, container, false);

            //return base.OnCreateView(inflater, container, savedInstanceState);
        }
        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            if (Arguments != null)
            {
                string caseid = Arguments.GetString("case");
                caseId = caseid;
                casename = Arguments.GetString("casename");
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
                recyclerView = View.FindViewById<SwipeMenuRecyclerView>(Resource.Id.swipe_menu_visits);
                recyclerView.SetAdapter(new Visits_Swipe_Adapter(this.Activity, javavisits));
                recyclerView.SetLayoutManager(new LinearLayoutManager(this.Activity));

                //this.ListAdapter = new Models.VisitsAdapter(Android.App.Application.Context, javavisits);

                ((AppCompatActivity)Activity).SupportActionBar.SetTitle(Resource.String.visits_title);


            }


            base.OnViewCreated(view, savedInstanceState);
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            this.Activity.MenuInflater.Inflate(Resource.Menu.project_menu, menu);
            base.OnCreateOptionsMenu(menu, inflater);
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


            recyclerView.SetAdapter(new Visits_Swipe_Adapter(Activity, javavisits));
        }

        //public void ShowObservations(IMonitoringVisit visit)
        //{
        //    base.OnListItemClick(l, v, position, id);
        //    MonitoringVisit visit = (MonitoringVisit)this.ListAdapter.GetItem(position); //ListAdapteriin vois tehdä ihan customfunkkarin palauttamaan guidin?
        //    ObservationFragment observations = new ObservationFragment();
        //    Bundle args = new Bundle();
        //    args.PutString("visitguid", visit.GUID);
        //    observations.Arguments = args;

        //    FragmentTransaction transaction = this.Activity.FragmentManager.BeginTransaction();
        //    transaction.Replace(Resource.Id.fragment_placeholder, observations, "observation");
        //    transaction.AddToBackStack(null);
        //    transaction.Commit();
        //}
    }
}