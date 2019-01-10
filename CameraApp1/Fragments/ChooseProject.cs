using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace CameraApp1.Fragments
{
    public class ChooseProjectFragment : ListFragment //Tämä sisältää defaulttina ListViewin joten ei ole tehty erillistä layout-tiedostoa
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
            
        }

        //voiko olla näin? on kyllä tutoriaalissakin?
        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            this.ListAdapter = new Models.ProjectAdapter(Android.App.Application.Context, GetProjects());
        }

        //palautaa kovakoodatun projektilistan testikäyttöä varten
        private JavaList<Project> GetProjects()
        {
            return new JavaList<Project>()
            {
                new Project("Aleksiskiven Katu 50", "345"),
                new Project("Ruosilankuja 3", "655"),
                new Project("Kolmaslinja 7", "1008"),
                new Project("Sipulikatu 14", "871")
            };            
        }

        public override void OnListItemClick(ListView l, View v, int position, long id)
        {
            base.OnListItemClick(l, v, position, id);
            //Tämä tavoittaa listasta valitun projektin
            Project project = (Project)this.ListAdapter.GetItem(position);

            //Tämä vaihtaa fragmentin --> on vielä ihan testinä vain kuvanotto fragmentti
            ProjectFragmentOn();

        }

        private void ProjectFragmentOn()
        {
            ObservationPageFragment observation = new ObservationPageFragment();
            FragmentTransaction transaction = this.Activity.FragmentManager.BeginTransaction();
            transaction.Replace(Resource.Id.fragment_placeholder, observation);
            transaction.AddToBackStack(null);
            transaction.Commit();

        }
    }
}