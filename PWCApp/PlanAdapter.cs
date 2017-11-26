using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Net;
using Android.Graphics;
using Java.IO;
using Android.Graphics.Drawables;
using Android.Util;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using Android;
using System.Collections.Generic;
using PWBackend;
using System;

namespace PWCApp
{

    public class PlanAdapter : BaseAdapter<JobsAssigned>
    {
        
        List<JobsAssigned> items;

        Activity context;
        public PlanAdapter(Activity context, List<JobsAssigned> items)
            : base()
        {
            this.context = context;
            this.items = items;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override JobsAssigned this[int position]
        {
            get { return items[position]; }
        }
        public override int Count
        {
            get { return items.Count; }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = items[position];
            View view = convertView;
            if (view == null) // no view to re-use, create new
                view = context.LayoutInflater.Inflate(Resource.Layout.CustomRow, null);

            string workersMSG = "";
            foreach (var worker in item.EmployeeJobs)
            {
                workersMSG = workersMSG + worker.EmpNAME + ", ";
            }

            view.FindViewById<TextView>(Resource.Id.lblTitle).Text = item.AssignWORK + " - Job #: " + item.AssignJOBNUM + " - Client: " + item.AssignCLIENT;
            view.FindViewById<TextView>(Resource.Id.lblSubTitle).Text = "Workers: " + workersMSG;
            view.FindViewById<TextView>(Resource.Id.lblInfo).Text = "Note: " + item.AssignINSTRUCTIONS;

            return view; 
        }
    }
}
