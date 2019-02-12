using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;
namespace CameraApp1.Models
{
    public class VisitsAdapter : BaseAdapter
    {
        private Context context;
        private JavaList<MonitoringVisit> visits;
        private LayoutInflater inflater;

        public VisitsAdapter(Context context, JavaList<MonitoringVisit> visits)
        {
            this.context = context;
            this.visits = visits;
        }

        public override int Count => visits.Count;

        public override Object GetItem(int position)
        {
            return visits.Get(position);
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
                convertView = inflater.Inflate(Android.Resource.Layout.SimpleListItem1, null);
                convertView.FindViewById<TextView>(Android.Resource.Id.Text1).Text = $" {visits[position].visitname}";

            }
            return convertView;
        }
    }
}