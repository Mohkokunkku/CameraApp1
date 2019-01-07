using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace CameraApp1
{
    public class MainPageFragment : Fragment
    {
        ImageView imageView;
        EditText editText;
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
            imageView.Click += viewCamera_Click;
        }

        private void viewCamera_Click(object sender, EventArgs e)
        {
            //Intent intent = new Intent("ACTION_IMAGE_CAPTURE");
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            StartActivityForResult(intent, 0);
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
                Console.WriteLine(ex);
            }
        }
    }
}