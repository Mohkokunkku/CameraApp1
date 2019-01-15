using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace CameraApp1.Models
{
    class ObservationsAdapter : BaseAdapter
    {
        private Context context;
        private JavaList<Observation> observations;
        private LayoutInflater inflater;

        public ObservationsAdapter(Context context, JavaList<Observation> observations)
        {
            this.context = context;
            this.observations = observations;
            //this.inflater = inflater; //Katsotaan myöhemmin tarviiko inflateria
        }

        public override int Count => observations.Count();

        public override Java.Lang.Object GetItem(int position)
        {
            return observations.Get(position);
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            if (inflater == null)
            {
                inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);
            }
            if (convertView == null)
            {
                try
                {
                    convertView = inflater.Inflate(Resource.Layout.observation_row, null);
                    //LAITTAA KUVAN RIVIN KUVAPAIKKAAN
                    ImageView image = convertView.FindViewById<ImageView>(Resource.Id.observation_image);
                    Observation observation = (Observation)GetItem(position);
                    Android.Net.Uri uri = Android.Net.Uri.Parse($"{observation.imageuri}");
                    image.SetImageURI(uri);
                    //LAITTAA TEKSTIN RIVIN EDITTEXTIIN
                    EditText text = convertView.FindViewById<EditText>(Resource.Id.observation_text);
                    text.Text = $"{observation.observation}";
                }
                catch (System.Exception ex)
                {

                    string test = ex.Message;
                }
            }
            return convertView;
        }
    }
}