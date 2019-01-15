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
    public class MainActivity : AppCompatActivity
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

                Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
                //toolbar.InflateMenu(Resource.Menu.navigation);
                toolbar.SetTitleTextColor(Android.Graphics.Color.White);
                toolbar.SetTitle(Resource.String.projects_title);
                SetSupportActionBar(toolbar);
                
                //ActionBar.Title = "VALITSE PROJEKTI";
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


        //Laittaa menun näkyviin Toolbarinn
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.navigation, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.add_visit)
            {
                //hakee CaseId:n avatusta Valvontakäynti-fragmentista
                Fragments.VisitsFragment casefrag = (Fragments.VisitsFragment)FragmentManager.FindFragmentByTag("CaseId");
                string caseId = casefrag.caseId;

                Bundle args = new Bundle();
                args.PutString("case", caseId);
                
                Android.App.FragmentTransaction transaction = this.FragmentManager.BeginTransaction();
                Android.App.DialogFragment fragment = new Fragments.NewVisitFragment();
                fragment.Arguments = args;
                //transaction.Replace(Resource.Id.fragment_placeholder, fragment);
                //transaction.AddToBackStack(null);
                //transaction.Commit();
                fragment.Show(FragmentManager, "Formi");
                
                
            }
            else if (item.ItemId == Resource.Id.take_photo_menu)
            {
                Fragments.ObservationFragment observationfrag = (Fragments.ObservationFragment)FragmentManager.FindFragmentByTag("observation");
                observationfrag.TakePhoto();
            }
            //Toast.MakeText(this, $"Action selected: { item.TitleFormatted }", ToastLength.Short).Show();
            
            return base.OnOptionsItemSelected(item);
        }
    }
}

