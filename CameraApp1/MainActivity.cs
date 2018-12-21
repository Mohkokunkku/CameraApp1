using System;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
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
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, BottomNavigationView.IOnNavigationItemSelectedListener
    {
        TextView textMessage;
        ImageView imageView;
        RelativeLayout llMain;
        EditText editText;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            llMain = FindViewById<RelativeLayout>(Resource.Id.container);

            //editText = FindViewById<EditText>(Resource.Id.captionText);
            //textMessage = FindViewById<TextView>(Resource.Id.message);
            //var btnCamera = FindViewById<Button>(Resource.Id.btnCamera);
            //imageView = FindViewById<ImageView>(Resource.Id.imageView1);
            //BottomNavigationView navigation = FindViewById<BottomNavigationView>(Resource.Id.navigation);
            //navigation.SetOnNavigationItemSelectedListener(this);

            //btnCamera.Click += btnCamera_Click;
            //imageView.Click += viewCamera_Click;
            //if (savedInstanceState == null)
            //{
            //    Fragment fragment = 
            //}

            Android.App.FragmentTransaction fragmentTx = this.FragmentManager.BeginTransaction();
           // DetailsFragment aDifferentDetailsFrag = 
           //Fragment firstpage = Resource.Layout.FirstPage
           //21.12.2018 jäi nyt tilanteeseen että pitäis declarata MainPagesta instanssi ja laittaa trasaction parametriksi
           //ideana saada tämä pätkä julkaisemaan MainPageFragment --> FirstPage.axml on vastinparina
           fragmentTx.Add(MainPage,  )
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
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
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void viewCamera_Click(object sender, EventArgs e)
        {
            //Intent intent = new Intent("ACTION_IMAGE_CAPTURE");
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            StartActivityForResult(intent, 0);
        }

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

