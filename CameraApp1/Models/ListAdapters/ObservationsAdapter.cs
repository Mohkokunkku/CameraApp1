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
using CameraApp1.Fragments;
using FFImageLoading;
using FFImageLoading.Views;
using Java.Lang;
using static Android.Views.View;

namespace CameraApp1.Models
{
    class ObservationsAdapter : BaseAdapter
    {
        private Activity context;
        private JavaList<Observation> observations;
        private LayoutInflater inflater;
        private int currentlyFocusedRow;

        public ObservationsAdapter(Activity context, JavaList<Observation> observations)
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

        public void ChangeText(string text, int position)
        {
             
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
                        DownSample(width:110).
                        Into(image);
                    //image.SetImageURI(uri);
                    
                    //LAITTAA TEKSTIN RIVIN EDITTEXTIIN
                    TextView text = convertView.FindViewById<TextView>(Resource.Id.observation_text);
                    text.Text = $"{observation.observation}";
                    ImageView imageView = convertView.FindViewById<ImageView>(Resource.Id.right_menu_delete);
                   // imageView.SetTag(Resource.Id.imageholder, position);
                    imageView.Clickable = true;
                    text.Clickable = true;
                    text.Click += (sender, n) => ObservationText_Click(sender, n, position);
                    convertView.LongClickable = true;
                    convertView.LongClick += (sender, n) => Show_DeleteIcon(sender, n, convertView); 
                    imageView.Click += (sender, n) => Delete_Item(sender, n, position);
                }
                catch (System.Exception ex)
                {

                    string test = ex.Message;
                }
            }
            return convertView;
        }

        private void ObservationText_Click(object sender, EventArgs n, int position)
        {
            Bundle args = new Bundle();
            IObservation observation = (IObservation)GetItem(position);
            args.PutString("kuvateksti", $"{observation.observation}");
            args.PutInt("position", position);
            Android.App.FragmentTransaction transaction = context.FragmentManager.BeginTransaction();
            Android.App.DialogFragment fragment = new Fragments.Fragment_AddObservationText();
            fragment.Arguments = args;
            fragment.Show(context.FragmentManager, "addtext");
        }


        //näyttää Delete-ikonin kun itemiä pitää pitkään pohjassa
        private void Show_DeleteIcon(object sender, LongClickEventArgs n, View convertView)
        {
            ImageView image = convertView.FindViewById<ImageView>(Resource.Id.right_menu_delete);
            if (image.Visibility == ViewStates.Gone)
            {
                image.Visibility = ViewStates.Visible;
            }
            else
            {
                image.Visibility = ViewStates.Gone;
            }
        }


        private void Delete_Item(object position, EventArgs convertView, int pos)
        {
            LocalDB.DeleteObservation((IObservation)GetItem(pos));
            ObservationFragment observationfragment = (ObservationFragment)context.FragmentManager.FindFragmentByTag("observation");
            observationfragment.RefreshView();
            //throw new NotImplementedException();
        }

        //private EventHandler ImageClick(EventArgs args)
        //{
        //    Console.WriteLine
        //}

        //public void OnClick(View v)
        //{
        //    var tag = v.GetTag(0);
        //    int position = (int)tag; 
        //    observations.RemoveAt(position);


    }
}