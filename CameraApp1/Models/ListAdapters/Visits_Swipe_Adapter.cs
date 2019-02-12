using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using CameraApp1.Fragments;
using Com.Tubb.Smrv;
using SQLite;

namespace CameraApp1.Models.ListAdapters
{
    public class Visits_Swipe_Adapter : RecyclerView.Adapter, Interfaces.IItemClickListener
    {
        private readonly JavaList<MonitoringVisit> _visits;
        private readonly Activity _context;

        public Visits_Swipe_Adapter(Activity context, JavaList<MonitoringVisit> visits)
        {
            _visits = visits;
            _context = context;
        }

        public override int ItemCount => _visits.Count();

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            MonitoringVisit visit = (MonitoringVisit)_visits.Get(position);
            var vh = (Visits_Swipe_AdapterViewHolder)holder;
            vh._itemName.Text = visit.visitname;
            vh.Sml.SwipeEnable = true;
            
            // You can set click listners to indvidual items in the viewholder here
            vh.SetItemClickListener(this);
            // vh.Sml.SetOnClickListener();
            //vh.Sml.SetOnLongClickListener(this);
        }

        public void OnClick(View itemView, int position, bool isLongClick)
        {
            //SWITCH KÄSITTELEE MITÄ KOHTAA KÄYTTÄJÄ KLIKKAA RECYCLERVIEWISSÄ
            int id = itemView.Id;
            switch (id)
            {
                case Resource.Id.send_to_server_button:
                    Console.WriteLine("Serverille lähetys alkaa");
                    //täytyy hakea puhelimen tietokannasta observationit tässä vaiheessa
                    string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "database.docstarter");
                    SQLiteConnection db = new SQLiteConnection(dbPath);
                    var visit1 = (MonitoringVisit)_visits.Get(position);
                    List<Observation> observationsToServer = db.Table<Observation>().Where(s => s.visitguid == visit1.visitguid).ToList();
                    var observsender = new ObservationSender(observationsToServer, visit1);
                    var uiContext = TaskScheduler.FromCurrentSynchronizationContext();
                    //Task.Run(() => observsender.SendVisit()).c;
                    Task.Factory.StartNew(() => observsender.SendVisit(), CancellationToken.None, TaskCreationOptions.None, uiContext);
                    Toast.MakeText(Android.App.Application.Context, "Sending data to server...", ToastLength.Long).Show();
                    
                    break;
                case Resource.Id.right_menu_delete_item:
                    MonitoringVisit removevisit = (MonitoringVisit)_visits.Get(position);
                    LocalDB.DeleteVisit(removevisit);
                    //Console.WriteLine("Itemin poisto alkaa");
                    break;
                case Resource.Id.visit_content:
                    MonitoringVisit visit = (MonitoringVisit)_visits.Get(position);
                    Bundle args = new Bundle();
                    args.PutString("visitguid", visit.visitguid);
                    args.PutString("visitname", visit.visitname);
                    ObservationFragment observations = new ObservationFragment();
                    observations.Arguments = args;
                    FragmentTransaction transaction = _context.FragmentManager.BeginTransaction();
                    transaction.Replace(Resource.Id.fragment_placeholder, observations, "observation");
                    transaction.AddToBackStack(null);
                    transaction.Commit();
                    break;
            }
            //if (itemView.Id == Resource.Id.visit_content)
            //{
            //        MonitoringVisit visit = (MonitoringVisit)_visits.Get(position);
            //        Bundle args = new Bundle();
            //        args.PutString("visitguid", visit.GUID);
            //        ObservationFragment observations = new ObservationFragment();
            //        observations.Arguments = args;
            //        FragmentTransaction transaction = _context.FragmentManager.BeginTransaction();
            //        transaction.Replace(Resource.Id.fragment_placeholder, observations, "observation");
            //        transaction.AddToBackStack(null);
            //        transaction.Commit();               
            //}
            //else if (itemView.Id == Resource.Id.send_to_server_button)
            //{
            //    Console.WriteLine("Serverille lähetys alkaa");
            //}
            //else if (itemView.Id == Resource.Id.right_menu_delete_item)
            //{
            //    Console.WriteLine("Poisto alkaa");
            //}
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var view = LayoutInflater.From(_context).Inflate(Resource.Layout.visit_item, parent, false);

            return new Visits_Swipe_AdapterViewHolder(view);
        }
    }

    //ViewHolder pitikö tän ehkä olla nested-classi? Ei oikein hajua mutta kannattaa ehkä tarkkaan katsoa vielä tutoriaalia 

    class Visits_Swipe_AdapterViewHolder : RecyclerView.ViewHolder, View.IOnClickListener, View.IOnLongClickListener
    {
        public readonly TextView _itemName;
        public readonly ImageView _deleItem;
        public readonly SwipeHorizontalMenuLayout Sml;
        public readonly ImageView _uploadbutton;
        private Interfaces.IItemClickListener _itemClickListener;

        public Visits_Swipe_AdapterViewHolder(View itemName): base(itemName)
        {
            _itemName = itemName.FindViewById<TextView>(Resource.Id.visit_content);
            _itemName.SetOnLongClickListener(this);
            _itemName.SetOnClickListener(this);           
            _uploadbutton = itemName.FindViewById<ImageView>(Resource.Id.send_to_server_button);
            _uploadbutton.SetOnClickListener(this);
            _deleItem = itemName.FindViewById<ImageView>(Resource.Id.right_menu_delete_item);
            _deleItem.SetOnClickListener(this);
            Sml = itemName.FindViewById<SwipeHorizontalMenuLayout>(Resource.Id.visit_item);
            Sml.SetOnClickListener(this);
            Sml.SetOnLongClickListener(this);
        }
        public void SetItemClickListener(Interfaces.IItemClickListener itemClickListener)
        {
            this._itemClickListener = itemClickListener;
        }

        public void OnClick(View v)
        {
            _itemClickListener.OnClick(v, AdapterPosition, false);
        }

        public bool OnLongClick(View v)
        {
            _itemClickListener.OnClick(v, AdapterPosition, true);
            return true;
        }


        //Your adapter views to re-use
        //public TextView Title { get; set; }
    }
}