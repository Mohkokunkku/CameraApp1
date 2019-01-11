using System;
using System.Collections.Generic;
using System.IO;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
namespace CameraApp1
{
    [Activity(Label = "@string/app_name", Theme = "@style/MyTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, BottomNavigationView.IOnNavigationItemSelectedListener
    {  //CompatAppActivity
       // TextView textMessage;
       // ImageView imageView;
        RelativeLayout llMain;
        //EditText editText;
       // public LocalDB localDB;

        //public static List<MonitoringVisit> monitoringvisits;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                //alustaa ToolBarin


                base.OnCreate(savedInstanceState);
                    SetContentView(Resource.Layout.activity_main);
                    llMain = FindViewById<RelativeLayout>(Resource.Id.container);

                var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
                toolbar.SetTitleTextColor(Android.Graphics.Color.White);
                SetActionBar(toolbar);
                ActionBar.Title = "VALITSE PROJEKTI";
                CheckCameraPermission();
                
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }

            Android.App.FragmentTransaction fragmentTx = this.FragmentManager.BeginTransaction();
            // DetailsFragment aDifferentDetailsFrag = 
            //Fragment firstpage = Resource.Layout.FirstPage
            //21.12.2018 jäi nyt tilanteeseen että pitäis declarata MainPagesta instanssi ja laittaa trasaction parametriksi
            //ideana saada tämä pätkä julkaisemaan MainPageFragment --> FirstPage.axml on vastinparina
            //ObservationPageFragment mainPageFragment = new ObservationPageFragment();
            Fragments.ChooseProjectFragment mainPageFragment = new Fragments.ChooseProjectFragment();

            fragmentTx.Add(Resource.Id.fragment_placeholder, mainPageFragment);
            fragmentTx.AddToBackStack(null);
            fragmentTx.Commit();
        }

        private void CheckCameraPermission()
        {
           if (ActivityCompat.ShouldShowRequestPermissionRationale(this, Manifest.Permission.Camera))
            {
                var requiredPermissions = new string[] { Manifest.Permission.Camera };
                Snackbar.Make(FindViewById(Resource.Id.container), Resource.String.camera_permission_question, Snackbar.LengthIndefinite)
                    .SetAction(Resource.String.camera_ok, new Action<View>(delegate (View obj)
                    {
                        ActivityCompat.RequestPermissions(this, requiredPermissions, 1888);
                    }
                    )).Show(); 
            }
           else
            {
                ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.Camera }, 1888);
            }
        }

        //Tämä taitaa jäädä turhaksi?
        public bool OnNavigationItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.navigation_home:
                   // textMessage.SetText(Resource.String.title_home);
                    return true;
                case Resource.Id.navigation_dashboard:
               //     Android.App.Fragment testFragment = new Test();
                    var fm = FragmentManager;
                    Android.App.FragmentTransaction ft = fm.BeginTransaction();
                 //   ft.Add(Resource.LtestFragment);
              
                    //textMessage.SetText(Resource.String.title_dashboard);
                    return true;
                case Resource.Id.navigation_notifications:
                    //textMessage.SetText(Resource.String.title_notifications);
                    return true;
            }
            return false;
        }



 
    }
}

