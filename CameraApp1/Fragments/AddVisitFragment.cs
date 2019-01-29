using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using SQLite;

namespace CameraApp1.Fragments
{
    public class NewVisitFragment : DialogFragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        //public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        //{
        //    // Use this to return your custom view for this Fragment
        //    // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

        //    return base.OnCreateView(inflater, container, savedInstanceState);
        //}
        //public override void OnDismiss(IDialogInterface dialog)
        //{
        //    VisitsFragment.r
        //    base.OnDismiss(dialog);
        //}

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            SetHasOptionsMenu(true);
            
            AlertDialog.Builder builder = new AlertDialog.Builder(Activity);

            builder.SetTitle("Uusi valvontakäynti");
            //builder.SetOnDismissListener(new OnD)
            EditText text = new EditText(Activity);
            text.Text = $"Viikko { GetWeekNumber() }";
            builder.SetView(text);
            builder.SetPositiveButton(Resource.String.camera_ok, delegate
            {
                if (Arguments != null)
                {
                    string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal),"database.docstarter");
                    SQLiteConnection db = new SQLiteConnection(dbPath);
                   
                    string caseid = Arguments.GetString("case");
                    //laittaa uuden visitin SQLiteen
                    MonitoringVisit visit = new MonitoringVisit { name = text.Text, casenumber = caseid, GUID = $"{ Guid.NewGuid()}" };
                    db.CreateTable<MonitoringVisit>();
                    db.Insert(visit);

                    //Päivittää käyntilistan heti kun OK-nappulaa painetaan
                    Fragments.Fragment_Visits_Swipe_Menu casefrag = (Fragments.Fragment_Visits_Swipe_Menu)FragmentManager.FindFragmentByTag("CaseId");
                    casefrag.ReFreshView();
                    builder.Dispose();
                    
                }
            });

            builder.SetNegativeButton(Resource.String.cancel, delegate 
            {
                builder.Dispose();
            });

            return builder.Create();
            //return base.OnCreateDialog(savedInstanceState);
        }

        private int GetWeekNumber()
        {
            CultureInfo ciCurr = CultureInfo.CurrentCulture;
            int weekNum = ciCurr.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            return weekNum;
        }
    }
}