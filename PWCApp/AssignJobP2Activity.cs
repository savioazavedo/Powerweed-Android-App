using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace PWCApp
{
    [Activity(Label = "AssignJobP2", MainLauncher = false, Icon = "@drawable/icon")]
    public class AssignJobP2Activity : Activity
    {
        ListView lvJobs;
        JOBSHandler objRest = new JOBSHandler();
        protected override void OnCreate(Bundle bundle)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.AssignJobP2);
            // Create your application here
            lvJobs = FindViewById<ListView>(Resource.Id.lvSelectJobs);
            lvJobs.Adapter = new DataAdapter(this, GetData.myList);
            lvJobs.ItemClick += LvJobs_ItemClick;
            Button btnBack = FindViewById<Button>(Resource.Id.btnBackAJP2);
            btnBack.Click += BtnBack_Click;
        }

        private void LvJobs_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var Item = GetData.myList[e.Position];
            var send = new Intent(this, typeof(AssignJobP3Activity));

            send.PutExtra("JobName", Item.JobWORK);
            send.PutExtra("JobID", Item.JobID);
            send.PutExtra("JobNumber", Item.JobNUMBER);
            send.PutExtra("JobClients", Item.JobCLIENTS);
            send.PutExtra("JobArea", Item.JobArea);
            StartActivity(send);
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {           
            Finish();
        }

        protected override void OnResume()
        {
            base.OnResume();
            lvJobs.Adapter = new DataAdapter(this, GetData.myList);
        }
    }
}