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
using Android.Telephony;
using Android.Util;
using PWBackend;

namespace PWCApp
{
    [Activity(Label = "AssignJobP4")]
    public class AssignJobP4Activity : Activity
    {
        
        string JobTime, JobDate, JobTruckNo, JobName, JobClient, JobArea, JobInstructions, workersMSG, startTime, JobEndTime;
        int JobID, JobNumber;
        TextView txtMsg;
        JOBSHandler objJOBS = new JOBSHandler();

        Button btnSend;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AssignJobP4);

            txtMsg = FindViewById<TextView>(Resource.Id.txtMessage);
            btnSend = FindViewById<Button>(Resource.Id.btnSend);
            Button btnBack = FindViewById<Button>(Resource.Id.btnBackAJP4);
            Button btnSave = FindViewById<Button>(Resource.Id.btnSaveAJP4);
            txtMsg.Text = "";

            JobTime = Intent.GetStringExtra("JobTime");
            JobEndTime = Intent.GetStringExtra("JobEndTime");
            JobDate = Intent.GetStringExtra("JobDate");
            JobTruckNo = Intent.GetStringExtra("JobTruckNumbers");
            JobName = Intent.GetStringExtra("JobName");
            JobID = Intent.GetIntExtra("JobID", 0);
            JobClient = Intent.GetStringExtra("JobClients");
            JobNumber = Intent.GetIntExtra("JobNumber", 0);
            JobArea = Intent.GetStringExtra("JobArea");

            workersMSG = "";
            foreach (var worker in AssignJobP3Activity.WorkerList)
            {
                workersMSG = workersMSG + worker.empNAME + ", ";
            }
           
            txtMsg.Text = "Team: " + workersMSG + "\n" + "Client: " + JobClient + "\n" + "Job Number: " + JobNumber + "\n" + "Area: " + JobArea + "\nTrucks: " + JobTruckNo + "\n" + "Date: " + Convert.ToDateTime(JobDate).ToShortDateString() + "\n" + "Start Time: " + Convert.ToDateTime(JobTime).TimeOfDay + "\n\nNote: ";           
            btnSend.Click += BtnSend_Click;
            btnBack.Click += BtnBack_Click;
            btnSave.Click += BtnSave_Click;
        }


        private void BtnSave_Click(object sender, EventArgs e)
        {

            JobInstructions = txtMsg.Text.Split(new string[] { "Note:" }, StringSplitOptions.None).Last();          //NEEDSTEST
            JobsAssigned job = new JobsAssigned();
            foreach (var worker in AssignJobP3Activity.WorkerList)
            {
                EmployeeJob emp1 = new EmployeeJob();
                emp1.EmpNAME = worker.empNAME;
                job.EmployeeJobs.Add(emp1);
            }

            startTime = JobDate + "T" + JobTime;

            job.AssignJOBNUM = JobNumber.ToString();
            job.AssignCLIENT = JobClient;
            job.AssignWORK = JobName;
            job.AssignAREA = JobArea;
            job.AssignINSTRUCTIONS = JobInstructions;
            job.AssignTRUCK = JobTruckNo;
            job.TextSENT = null;
            job.AssignSTARTTIME = startTime;
            

            try
            {
                objJOBS.ExecutePostRequest(job);
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, "Something went wrong:" + ex.Message, ToastLength.Long).Show();
            }

            AlertDialog.Builder alertDialog = new AlertDialog.Builder(this);

            alertDialog.SetMessage("Message saved.");
            alertDialog.SetPositiveButton("New Message", delegate                        
            {
                StartActivity(typeof(AssignJobP2Activity));
                alertDialog.Dispose();
            });
            alertDialog.SetNegativeButton("Menu", delegate
            {
                StartActivity(typeof(MainActivity));
                alertDialog.Dispose();
            });
            alertDialog.Show();
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            Finish();
        }

        private void BtnSend_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    foreach (var i in AssignJobP3Activity.WorkerList)
                    {


                        var parts = SmsManager.Default.DivideMessage(txtMsg.Text);
                        SmsManager.Default.SendMultipartTextMessage(i.empMobile, null, parts, null, null);

                    }
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, "A message failed to send\n" + ex, ToastLength.Long).Show();
                }

                Toast.MakeText(this, "Messages sent", ToastLength.Long).Show();
                btnSend.Enabled = false;
                
                JobInstructions = txtMsg.Text.Split(new string[] { "Note:" }, StringSplitOptions.None).Last();          //NEEDSTEST
                JobsAssigned job = new JobsAssigned();
                foreach (var worker in AssignJobP3Activity.WorkerList)
                {
                    EmployeeJob emp1 = new EmployeeJob();
                    emp1.EmpNAME = worker.empNAME;
                    job.EmployeeJobs.Add(emp1);
                }

                startTime = JobDate + "T" + JobTime;

                job.AssignJOBNUM = JobNumber.ToString();
                job.AssignCLIENT = JobClient;
                job.AssignWORK = JobName;
                job.AssignAREA = JobArea;
                job.AssignINSTRUCTIONS = JobInstructions;
                job.AssignTRUCK = JobTruckNo;
                job.TextSENT = DateTime.Now.ToString("yyyy-MM-dd" + "T" + "HH:mm:ss");
                job.AssignSTARTTIME = startTime;


                try
                {
                    objJOBS.ExecutePostRequest(job);
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, "Something went wrong:" + ex.Message, ToastLength.Long).Show();
                }

                AlertDialog.Builder alertDialog = new AlertDialog.Builder(this);

                alertDialog.SetMessage("Message sent and saved.");
                alertDialog.SetPositiveButton("New Message", delegate
                {
                    StartActivity(typeof(AssignJobP2Activity));
                    alertDialog.Dispose();
                });
                alertDialog.SetNegativeButton("Menu", delegate
                {
                    StartActivity(typeof(MainActivity));
                    alertDialog.Dispose();
                });
                alertDialog.Show();

            }
            catch (Exception ex)
            {
                Toast.MakeText(this, "Something went wrong:" + ex.Message, ToastLength.Long).Show();
            }
            

        }




        
    }
}