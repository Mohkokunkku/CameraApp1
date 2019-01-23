﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using CameraApp1.Fragments;
using Com.Tubb.Smrv;

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
            vh._itemName.Text = visit.name;
            vh.Sml.SwipeEnable = true;
            // You can set click listners to indvidual items in the viewholder here
            vh.SetItemClickListener(this);
           // vh.Sml.SetOnClickListener();
            //vh.Sml.SetOnLongClickListener(this);
            
        }

        public void OnClick(View itemView, int position, bool isLongClick)
        {
            if(isLongClick)
            {
                Toast.MakeText(_context, "Clikki toimii", ToastLength.Short);
            }
            else
            {
                MonitoringVisit visit = (MonitoringVisit)_visits.Get(position);
                Bundle args = new Bundle();
                args.PutString("visitguid", visit.GUID);
                ObservationFragment observations = new ObservationFragment();
                observations.Arguments = args;
                FragmentTransaction transaction = _context.FragmentManager.BeginTransaction();
                transaction.Replace(Resource.Id.fragment_placeholder, observations, "observation");
                transaction.AddToBackStack(null);
                transaction.Commit();

            }
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
        public readonly SwipeHorizontalMenuLayout Sml;
        private Interfaces.IItemClickListener _itemClickListener;

        public Visits_Swipe_AdapterViewHolder(View itemName): base(itemName)
        {
            _itemName = itemName.FindViewById<TextView>(Resource.Id.visit_content);
            _itemName.SetOnLongClickListener(this);
            _itemName.SetOnClickListener(this);
            Sml = itemName.FindViewById<SwipeHorizontalMenuLayout>(Resource.Id.visit_item);
            //Sml.SetOnClickListener(this);
            //Sml.SetOnLongClickListener(this);
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