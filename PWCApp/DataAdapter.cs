
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

namespace PWCApp
{

    public class DataAdapter : BaseAdapter<Job>
    {

        List<Job> items;

        Activity context;
        public DataAdapter(Activity context, List<Job> items)
            : base()
        {
            this.context = context;
            this.items = items;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override Job this[int position]
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

            view.FindViewById<TextView>(Resource.Id.lblTitle).Text = "Job: " + item.JobWORK;
            view.FindViewById<TextView>(Resource.Id.lblSubTitle).Text = "Job Number: " + item.JobNUMBER;
            view.FindViewById<TextView>(Resource.Id.lblInfo).Text = "Client: " + item.JobCLIENTS;

            return view;
        }
    }
}
