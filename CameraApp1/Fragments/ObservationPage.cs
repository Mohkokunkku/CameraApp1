
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
using Android.Support.V4.Content;
using Uri = Android.Net.Uri;
using Android.Support.Compat;
using Android;

namespace CameraApp1
{
    public class ObservationPageFragment : Fragment
    {
        ImageView imageView;
        EditText etCaption;
        public static File _file;
        public static File _dir;
        ImageButton imageButton;
        public static Observation observation; //havainto jota käsitellään

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here Tähän vois paukuttaa dynaamisesti UI-kamaa 
            
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            return inflater.Inflate(Resource.Layout.fragment_observations, container, false);
            //return base.OnCreateView(Resource.Layout.FirstPage, container, savedInstanceState);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            etCaption = view.FindViewById<EditText>(Resource.Id.captionText);
            imageView = view.FindViewById<ImageView>(Resource.Id.imageholder);
            imageButton = view.FindViewById<ImageButton>(Resource.Id.btnAddToProject);
            imageButton.Click += btnAddToProject_Click;
            imageView.Click += viewCamera_Click;
            CreateDirectoryForPictures();
          
        }

 

        //Lisää havainnon projektille
        private void btnAddToProject_Click(object sender, EventArgs e)
        {
            //tää logiikka täytyy keksiä uusiksi jos observation luodaan jo aikaisemmin --> varmaan vain valmis observation laitetaan johonkin listaan?
            //Java.Net.URI uri = _file.ToURI();
            //Observation newObservation = new Observation(editText.Text, uri);
            observation.observation = etCaption.Text;

        }

        private void viewCamera_Click(object sender, EventArgs e)
        {
            //Intent intent = new Intent("ACTION_IMAGE_CAPTURE");

            try
            {
 
                    if (ContextCompat.CheckSelfPermission(Application.Context, Manifest.Permission.Camera) == Android.Content.PM.Permission.Granted)
                    {
                        Intent intent = new Intent(MediaStore.ActionImageCapture);
                        _file = new Java.IO.File(_dir, string.Format("Image_{0}.jpg", Guid.NewGuid()));
                        // Android.Net.Uri photouri = FileProvider.GetUriForFile(context, )
                        //Uri photoURI = Uri.FromFile(_file);

                        Uri photoUri = FileProvider.GetUriForFile(Android.App.Application.Context, "com.mydomain.fileprovider", _file);
                        intent.PutExtra(MediaStore.ExtraOutput, photoUri);
                        CreateNewObservation(photoUri);
                        intent.AddFlags(ActivityFlags.GrantReadUriPermission);
                        StartActivityForResult(intent, 0); 
                    }
                else
                {
                    
                }
                
            }
            catch (Exception ex)
            {

                System.Console.WriteLine(ex.Message);
            }
        }
        //luo uuden havainnon jota aletaan käsittelemään 
        private void CreateNewObservation(Uri uri)
        {
            observation = new Observation(uri);

        }


        private void CreateDirectoryForPictures()
        {
            _dir = Android.App.Application.Context.GetExternalFilesDir(Android.OS.Environment.DirectoryPictures); //new File.get(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures), );
            
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
                //if (data.Extras.IsEmpty == false) //antaa ekalla käynnistyksellä emulaattorissa jonkin kyselyn mutta toimii myöhemmin ihan normaalisti 
                if (requestCode == 0 && resultCode == Result.Ok)
                {

                    //Bitmap bitmap = (Bitmap)data.Extras.Get("data");
                    //imageView.SetImageBitmap(bitmap);
                    imageView.SetImageURI(observation.imageuri);
                }


            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex);
            }
        }
    }
}