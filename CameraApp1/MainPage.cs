
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Net;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.IO;

namespace CameraApp1
{
    public class MainPageFragment : Fragment
    {
        ImageView imageView;
        EditText editText;
        public static File _file;
        public static File _dir;
        ImageButton imageButton;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
            
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            return inflater.Inflate(Resource.Layout.FirstPage, container, false);
            //return base.OnCreateView(Resource.Layout.FirstPage, container, savedInstanceState);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            EditText etCaption = view.FindViewById<EditText>(Resource.Id.captionText);
            imageView = view.FindViewById<ImageView>(Resource.Id.imageholder);
            imageButton = view.FindViewById<ImageButton>(Resource.Id.btnAddToProject);
            imageButton.Click += btnAddToProject_Click;
            imageView.Click += viewCamera_Click;
            CreateDirectoryForPictures();
        }
        //Lisää havainnon projektille
        private void btnAddToProject_Click(object sender, EventArgs e)
        {
            Java.Net.URI uri = _file.ToURI();
            Observation newObservation = new Observation(editText.Text, uri);

        }

        private void viewCamera_Click(object sender, EventArgs e)
        {
            //Intent intent = new Intent("ACTION_IMAGE_CAPTURE");

            Intent intent = new Intent(MediaStore.ActionImageCapture);
            _file = new Java.IO.File(_dir, string.Format("Image_{0}.jpg", Guid.NewGuid()));
            intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(_file));
            
            StartActivityForResult(intent, 0);
        }

        private void CreateDirectoryForPictures()
        {
            _dir = new File(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures), "DocStarterImages");
            if (!_dir.Exists())
            {
                _dir.Mkdirs();
            }
        }
        

                public override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            try
            {
                
                base.OnActivityResult(requestCode, resultCode, data);
                if (data.Extras.IsEmpty == false) //antaa ekalla käynnistyksellä emulaattorissa jonkin kyselyn mutta toimii myöhemmin ihan normaalisti 
                {
                    
                    Bitmap bitmap = (Bitmap)data.Extras.Get("data");
                    imageView.SetImageBitmap(bitmap);
                    
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex);
            }
        }
    }
}