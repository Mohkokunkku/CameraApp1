using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FFImageLoading;
using FFImageLoading.Views;
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
                    ImageViewAsync image = convertView.FindViewById<ImageViewAsync>(Resource.Id.observation_image);
                    Observation observation = (Observation)GetItem(position);
                    if (File.Exists(observation.imageuri))
                    {
                        Console.WriteLine($"Tiedosto löytyi {observation.absolutepath}");
                    }
                    else
                    {
                        Console.WriteLine($"Tiedosto ei löytynyt {observation.absolutepath}");
                    }
                    var loadingimgpath = $"android.resource://CameraApp1.CameraApp1/{Resource.Drawable.loading}";
                    //Android.Net.Uri uri = Android.Net.Uri.Parse($"{observation.imageuri}");
                    ImageService.Instance.                       
                        //LoadingPlaceholder($"/android.resource://CameraApp1.CameraApp1/{Resource.Drawable.loading}").
                        LoadFile(@observation.absolutepath).                      
                        LoadingPlaceholder(loadingimgpath).
                        Retry(3, 200).
                        DownSample(width:120).
                        Into(image);
                    //image.SetImageURI(uri);
                    
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