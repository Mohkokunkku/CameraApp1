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
    public class ProjectAdapter : BaseAdapter
    {
        private Context context;
        private JavaList<JavaProject> projects;
        private LayoutInflater inflater;

        public ProjectAdapter(Context context, JavaList<JavaProject> projects)
        {
            this.context = context;
            this.projects = projects;
        }

        public override int Count => projects.Count(); //throw new NotImplementedException();

        public override Java.Lang.Object GetItem(int position)
        {
            return projects.Get(position);
            //throw new NotImplementedException();
        }

        public override long GetItemId(int position)
        {
            return position;
            //throw new NotImplementedException();
        }

        //Tässä tehdään projektilistan rivikohtainen muotoilu, voi myöhemmin hienostella lisää
        //Jos haluaa tehdä rivimuotoilun xml-tiedostolla niin esimerkki https://developer.xamarin.com/api/type/Android.App.ListFragment/
        //Jos haluaa laittaa dataa näkyviin tietokannasta https://docs.microsoft.com/en-us/xamarin/android/user-interface/layouts/list-view/
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            if (inflater == null)
            {
                inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);
            }
            if (convertView == null)
            {
                convertView = inflater.Inflate(Android.Resource.Layout.SimpleListItem1, null);
                convertView.FindViewById<TextView>(Android.Resource.Id.Text1).Text = $"{projects[position].caseId}, {projects[position].name}";
                return convertView;
            }
            return convertView;
           // throw new NotImplementedException();
        }
    }
}