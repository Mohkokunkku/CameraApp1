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
    public class Fragment_AddObservationText : DialogFragment
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

            return base.OnCreateView(inflater, container, savedInstanceState);
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(Activity);
            builder.SetTitle("Anna kuvateksti");
            EditText text = new EditText(Activity);
            text.Text = Arguments.GetString("kuvateksti");
            builder.SetView(text);
            builder.SetPositiveButton(Resource.String.camera_ok, delegate
            {
                if (Arguments != null)
                {
                    ObservationFragment observations = (ObservationFragment)FragmentManager.FindFragmentByTag("observation");
                    IObservation observation = (IObservation)observations.ListAdapter.GetItem(Arguments.GetInt("position"));
                    observation.observation = text.Text;
                    LocalDB.UpdateObservation(observation);
                    observations.RefreshView();
                    builder.Dispose();
                    
                }
            });

            builder.SetNegativeButton(Resource.String.cancel, delegate
            {
                builder.Dispose();
            });
            return builder.Create();
        }
    }
}