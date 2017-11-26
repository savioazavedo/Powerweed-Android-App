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
using Android.Telephony;
using System.Threading;
using static Android.Provider.Telephony;
using static PWCApp.AssignJobP3Activity;

namespace PWCApp
{
    [Activity(Label = "ViewTodaysPlanActivity", MainLauncher = false, Icon = "@drawable/icon")]
    public class ViewTodaysPlanActivity : Activity
    {
        
        List<Employee> EmpList;
        string txtMsg, WorkersMSG, empMobile;
        ListView lvPlan;
        List<JobsAssigned> jobsPlanList;
        Button btnSendAll, btnBack, btnSelectDate;
        PlanHandler objplan = new PlanHandler();
        EmployeesHandler objemp = new EmployeesHandler();
        EmployeeJob emp1 = new EmployeeJob();
        Employee employees = new Employee();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ViewTodaysPlan);

            lvPlan = FindViewById<ListView>(Resource.Id.lvPlan);
            btnSendAll = FindViewById<Button>(Resource.Id.btnSendAll);
            btnBack = FindViewById<Button>(Resource.Id.btnBackVTP);
            btnSelectDate = FindViewById<Button>(Resource.Id.btnSelectDate);
            jobsPlanList = objplan.ExecuteGetRequest();
            lvPlan.Adapter = new PlanAdapter(this, jobsPlanList);
            EmpList = new List<Employee>();          
            btnSendAll.Click += BtnSendAll_Click;
            btnBack.Click += BtnBack_Click;
            lvPlan.ItemClick += LvPlan_ItemClick;
            btnSelectDate.Click += BtnSelectDate_Click;
        }

        private void LvPlan_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var Item = jobsPlanList[e.Position];
            var send = new Intent(this, typeof(EditPlanItemActivity));

            send.PutExtra("AssignAREA", Item.AssignAREA);
            send.PutExtra("AssignCLIENT", Item.AssignCLIENT);
            send.PutExtra("AssignID", Item.AssignID);
            send.PutExtra("AssignINSTRUCTIONS", Item.AssignINSTRUCTIONS);
            send.PutExtra("AssignJOBNUM", Item.AssignJOBNUM);
            send.PutExtra("AssignSTARTTIME", Item.AssignSTARTTIME);
            send.PutExtra("AssignTRUCK", Item.AssignTRUCK);
            send.PutExtra("AssignWORK", Item.AssignWORK);
            
            StartActivity(send);
            
        }

        private void BtnSelectDate_Click(object sender, EventArgs e)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                btnSelectDate.Text = time.ToShortDateString();
                
                var plans = from d in jobsPlanList
                            where Convert.ToDateTime(d.AssignSTARTTIME).Date == Convert.ToDateTime(btnSelectDate.Text)
                            select d;

                lvPlan.Adapter = new PlanAdapter(this, plans.ToList());
                jobsPlanList = plans.ToList();
            });
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }


        private void BtnBack_Click(object sender, EventArgs e)
        {
            Finish();
        }

        private void BtnSendAll_Click(object sender, EventArgs e)
        {
            AlertDialog.Builder alertDialog = new AlertDialog.Builder(this);

            alertDialog.SetMessage("Are you sure you want to send all?");
            alertDialog.SetPositiveButton("Yes", delegate
            {
                //loop to go through set jobs and send all the texts at once
                foreach (var job in jobsPlanList)
                {

                    if (job.TextSENT == null)
                    {
                        job.TextSENT = DateTime.Now.ToString("yyyy-MM-dd" + "T" + "HH:mm:ss");                      //adding the text sent timestamp              
                        try
                        {
                            objplan.ExecutePutRequest(job);
                        }
                        catch (Exception ex)
                        {

                            Toast.MakeText(this, "Something went wrong: " + ex.Message, ToastLength.Long).Show();
                        }

                        WorkersMSG = "";
                        foreach (var emp in job.EmployeeJobs)
                        {
                            WorkersMSG = WorkersMSG + emp.EmpNAME + ", ";
                        }

                        foreach (var emp in job.EmployeeJobs)
                        {
                            empMobile = GetEmployees.getEmpNumber(emp.EmpNAME);

                            txtMsg = "Team: " + WorkersMSG + "\n" + "Client: " + job.AssignCLIENT + "\n" + "Job Number: " + job.AssignJOBNUM + "\n" + "Area: " + job.AssignAREA + "\nTrucks: " + job.AssignTRUCK + "\n" + "Date: " + Convert.ToDateTime(job.AssignSTARTTIME).ToShortDateString() + "\nStart time: " + Convert.ToDateTime(job.AssignSTARTTIME).TimeOfDay + "\n\nNote: " + job.AssignINSTRUCTIONS;

                            var parts = SmsManager.Default.DivideMessage(txtMsg);
                            SmsManager.Default.SendMultipartTextMessage(empMobile, null, parts, null, null);
                        }
                    }

                }
                jobsPlanList = objplan.ExecuteGetRequest();
                lvPlan.Adapter = new PlanAdapter(this, jobsPlanList);
            });
            alertDialog.SetNegativeButton("Cancel", delegate
            {
                alertDialog.Dispose();
            });
            alertDialog.Show();
            
        }
    }
}