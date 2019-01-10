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
    public class ChooseProjectFragment : ListFragment
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
        //public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        //{
        //    // Use this to return your custom view for this Fragment
        //    //Tämä on ListFragment niin tarviiko tätä viewiä lainkaan? 
        //    return inflater.Inflate(Resource.Layout.fragment_choose_project, container, false);

        //    //return base.OnCreateView(inflater, container, savedInstanceState);
        //}

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
    }
}