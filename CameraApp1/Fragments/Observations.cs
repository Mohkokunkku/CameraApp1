using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Android;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using SQLite;

namespace CameraApp1.Fragments
{
    public class ObservationFragment : ListFragment
    {
        public string visitguid { get; set; } //Valvontakäynnin GUID talteen 
        public static Java.IO.File _dir;
        public Observation observation { get; set; }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetHasOptionsMenu(true);
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            try
            {
                //TÄHÄN PITÄISI TUODA MAHDOLLISESTI JO OTETUT HAVAINTOKUVAT JA ANTAA KÄYTTÄJÄLLE MAHDOLLISUUS OTTAA UUSIA KUVIA --> KUVIEN OTTO NAPPULA VOISI OLLA ESIM TOOLBARISSA?
                if (Arguments != null)
                {
                    string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "database.docstarter");
                    SQLiteConnection db = new SQLiteConnection(dbPath);
                    db.CreateTable<Observation>();
                    visitguid = Arguments.GetString("visitguid");
                    List<Observation> observations = db.Table<Observation>().Where(s => s.visitguid == visitguid).ToList();
                    JavaList<Observation> javaobservations = new JavaList<Observation>();

                    if (observations.Count > 0)
                    {
                        foreach (var item in observations)
                        {
                            javaobservations.Add(item);
                        }
                    }

                    this.ListAdapter = new Models.ObservationsAdapter(Android.App.Application.Context, javaobservations);
                }

            }
            catch (Exception ex)
            {

                string test = ex.Message;
            }
            ((AppCompatActivity)Activity).SupportActionBar.SetTitle(Resource.String.observations_title);
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            return base.OnCreateView(inflater, container, savedInstanceState);
        }
        //LISÄÄ TOOLBARIIN KAMERAIKONIN JOSTA ON TARKOITUS OTTAA UUSIA VALOKUVIA
        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            this.Activity.MenuInflater.Inflate(Resource.Menu.take_photo_menu, menu);
            base.OnCreateOptionsMenu(menu, inflater);
        }

        //pitäisi avata kuva jotenkin? vai täytyykö olla joku ImageViewclick?
        public override void OnListItemClick(ListView l, View v, int position, long id)
        {
            base.OnListItemClick(l, v, position, id);


        }

        //KUVANOTTO SKRIPTIT --> KOPIOITU VANHASTA ObservationPage-fragmentistä jolle ei välttämättä ole enää käyttöä projektissa

        public void TakePhoto()
        {
            try
            {

                if (ContextCompat.CheckSelfPermission(Application.Context, Manifest.Permission.Camera) == Android.Content.PM.Permission.Granted)
                {
                    CreateDirectoryForPictures();
                    Intent intent = new Intent(MediaStore.ActionImageCapture);
                    Java.IO.File _file = new Java.IO.File(_dir, string.Format("Image_{0}.jpg", Guid.NewGuid()));
                    // Android.Net.Uri photouri = FileProvider.GetUriForFile(context, )
                    //Uri photoURI = Uri.FromFile(_file);

                    Android.Net.Uri photoUri = FileProvider.GetUriForFile(Android.App.Application.Context, "com.mydomain.fileprovider", _file);
                    intent.PutExtra(MediaStore.ExtraOutput, photoUri);
                    //CreateNewObservation(photoUri);
                   
                    intent.AddFlags(ActivityFlags.GrantReadUriPermission);
                    
                    CreateNewObservation(photoUri);

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

        public override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == 0 && resultCode == Result.Ok)
            {

                string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "database.docstarter");
                SQLiteConnection db = new SQLiteConnection(dbPath);
                db.CreateTable<Observation>();
                db.Insert(observation);
                RefreshView();
            }
        }

        private void CreateDirectoryForPictures()
        {
            _dir = Android.App.Application.Context.GetExternalFilesDir(Android.OS.Environment.DirectoryPictures); //new File.get(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures), );

            if (!_dir.Exists())
            {
                _dir.Mkdirs();
            }
        }

        public void CreateNewObservation(Android.Net.Uri uri)
        {
            observation = new Observation() {imageuri = $"{uri}", visitguid = $"{visitguid}", observationguid = $"{ Guid.NewGuid() }"  };

        }

        private void RefreshView()
        {
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "database.docstarter");
            SQLiteConnection db = new SQLiteConnection(dbPath);
            List<Observation> observations = db.Table<Observation>().Where(s => s.visitguid == visitguid).ToList();
            JavaList<Observation> javaobservations = new JavaList<Observation>();

            if (observations.Count > 0)
            {
                foreach (var item in observations)
                {
                    javaobservations.Add(item);
                }
            }

            this.ListAdapter = new Models.ObservationsAdapter(Android.App.Application.Context, javaobservations);
        }
    }
}