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
using PWBackend;

namespace PWCApp
{
    [Activity(Label = "EditEmployees1")]
    public class EditEmployees1 : Activity
    {
        ListView lvEmployees;
        List<Employee> myList;

        EmployeesHandler objemp = new EmployeesHandler();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EditEmployees1);
            // Create your application here
            
            Button btnBack = FindViewById<Button>(Resource.Id.btnBackEE1);
            lvEmployees = FindViewById<ListView>(Resource.Id.lvWorkersEE1);
            lvEmployees.ItemClick += LvEmployees_ItemClick;

            try
            {
                myList = objemp.ExecuteGetRequest();
            }
            catch (Exception)
            {

                throw;
            }
            
            lvEmployees.Adapter = new EmployeeAdapter(this, myList);
            
            btnBack.Click += BtnBack_Click;
        }

        private void LvEmployees_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var empl = myList[e.Position];
            AlertDialog.Builder alertDialog = new AlertDialog.Builder(this);


            if(empl.empType == "Unavailable")
            {
                alertDialog.SetMessage("Do you wish to mark this worker as Available");
                alertDialog.SetPositiveButton("Yes", delegate
                {

                    empl.empType = "Available";
                    objemp.ExecutePutRequest(empl);

                    try
                    {
                        myList = objemp.ExecuteGetRequest();
                    }
                    catch (Exception)
                    {

                        throw;
                    }

                    lvEmployees.Adapter = new EmployeeAdapter(this, myList);
                    alertDialog.Dispose();
                });
                alertDialog.SetNegativeButton("No", delegate
                {

                    alertDialog.Dispose();
                });
                alertDialog.Show();
            }
            else if(empl.empType == "Available")
            {
                alertDialog.SetMessage("Do you wish to mark this worker as unavailable");
                alertDialog.SetPositiveButton("Yes", delegate
                {

                    empl.empType = "Unavailable";
                    objemp.ExecutePutRequest(empl);

                    try
                    {
                        myList = objemp.ExecuteGetRequest();
                    }
                    catch (Exception)
                    {

                        throw;
                    }

                    lvEmployees.Adapter = new EmployeeAdapter(this, myList);
                    alertDialog.Dispose();
                });
                alertDialog.SetNegativeButton("No", delegate
                {

                    alertDialog.Dispose();
                });
                alertDialog.Show();
            }
           
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            Finish();
        }
    }
}