using Android.App;
using Android.Widget;
using Android.OS;
using PWBackend;
using System;
using Android.Views;

namespace PWCApp
{
    [Activity(Label = "PWCApp", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        JOBSHandler objRest = new JOBSHandler();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            Button btnAssign = FindViewById<Button>(Resource.Id.btnAssign);
            Button btnEditEmployees = FindViewById<Button>(Resource.Id.btnModEmployees);
            Button btnViewPlan = FindViewById<Button>(Resource.Id.btnViewPlan);

            btnAssign.Click += BtnAssign_Click;
            btnEditEmployees.Click += BtnEditEmployees_Click;
            btnViewPlan.Click += BtnViewPlan_Click;


            bool isConnected = CheckInternet.CheckForInternetConnection();

            if (isConnected == true)
            {
                GetData.GetJobData();
            }
            else
            {
                AlertDialog.Builder alertDialog = new AlertDialog.Builder(this);

                alertDialog.SetMessage("No internet connection");
                alertDialog.SetPositiveButton("Close", delegate
                {
                    Finish();
                    alertDialog.Dispose();
                });            
                alertDialog.Show();
            }
            
        }

        private void BtnViewPlan_Click(object sender, System.EventArgs e)
        {
            StartActivity(typeof(ViewTodaysPlanActivity));
        }

        private void BtnEditEmployees_Click(object sender, System.EventArgs e)
        {
            StartActivity(typeof(EditEmployees1));
        }

        private void BtnAssign_Click(object sender, System.EventArgs e)
        {
            StartActivity(typeof(AssignJobP2Activity));
        }
    }
}



