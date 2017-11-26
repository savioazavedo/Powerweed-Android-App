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
using Android.Views;
using PWBackend;
using Android.App;

namespace PWCApp
{

    public class EmployeeAdapter : BaseAdapter<Employee>
    {

        List<Employee> items;

        Activity context;
        public EmployeeAdapter(Activity context, List<Employee> items)
            : base()
        {
            this.context = context;
            this.items = items;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override Employee this[int position]
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

            view.FindViewById<TextView>(Resource.Id.lblTitle).Text = item.empNAME;
            view.FindViewById<TextView>(Resource.Id.lblSubTitle).Text = item.empType;
            view.FindViewById<TextView>(Resource.Id.lblInfo).Text = item.empMobile;

            return view;
        }
    }
}
