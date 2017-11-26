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
using Android.Util;
using Java.Util;

namespace PWCApp
{
    [Activity(Label = "AssignJobP3", WindowSoftInputMode =SoftInput.AdjustPan | SoftInput.StateVisible)]
    public class AssignJobP3Activity : Activity
    {

        Button btnSelectDate, btnSelectTime;       
        List<Employee> myList = GetEmployees.getEmployees();
        public static List<Employee> WorkerList;
        AutoCompleteTextView actxtWorkers;
        ListView lvSelectedEmps;
        TextView txtSelectedJob;
        TextView txtTruckNo, txtArea;
        string JobName, JobClient, JobArea, StartTime, EndTime, DateofJob;
        int JobID, JobNumber;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            
            SetContentView(Resource.Layout.AssignJobP3);

            // Create your application here

            WorkerList = new List<Employee>();
            Button btnNext = FindViewById<Button>(Resource.Id.btnNextAJP3);
            Button btnBack = FindViewById<Button>(Resource.Id.btnBackAJP3);
            txtSelectedJob = FindViewById<TextView>(Resource.Id.txtSelectedJob);
            lvSelectedEmps = FindViewById<ListView>(Resource.Id.lvCurrentWorkers);

            txtArea = FindViewById<TextView>(Resource.Id.txtArea);
            txtTruckNo = FindViewById<TextView>(Resource.Id.txtTruckNumbers);
            btnSelectDate = FindViewById<Button>(Resource.Id.btnSelectDate);
            btnSelectTime = FindViewById<Button>(Resource.Id.btnSelectTime);

            btnSelectDate.Text = "Date: " +  DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");  //setting default date on btn
            DateofJob = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            StartTime = "06:00:00";
            EndTime = "18:00:00";

            JobName = Intent.GetStringExtra("JobName");          
            JobID = Intent.GetIntExtra("JobID", 0);
            JobClient = Intent.GetStringExtra("JobClients");
            JobNumber = Intent.GetIntExtra("JobNumber", 0);
            JobArea = Intent.GetStringExtra("JobArea");

            txtSelectedJob.Text = JobNumber + " - " + JobName + " - " + JobClient;
            actxtWorkers = FindViewById<AutoCompleteTextView>(Resource.Id.actxtWorker);
            var workerAdapter = new ArrayAdapter<string>(this, Resource.Layout.list_item, GetEmployees.getEmpNames());
           
            actxtWorkers.Adapter = workerAdapter;
            btnBack.Click += BtnBack_Click;
            btnNext.Click += BtnNext_Click;
            lvSelectedEmps.ItemLongClick += LvSelectedEmps_ItemLongClick;
            btnSelectDate.Click += BtnSelectDate_Click;
            btnSelectTime.Click += BtnSelectTime_Click;

            actxtWorkers.ItemClick += ActxtWorkers_ItemClick;    
            
        }

        private void ActxtWorkers_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            try
            {
                bool empExists = false;
                foreach (var i in WorkerList)
                {
                    if (actxtWorkers.Text == i.empNAME)
                    {
                        empExists = true;
                    }
                }
                if (actxtWorkers.Text != "" && empExists == false)
                {
                    Employee emp = new Employee();
                    emp.empNAME = actxtWorkers.Text;
                    emp.empMobile = GetEmployees.getEmpNumber(actxtWorkers.Text);
                    WorkerList.Add(emp);

                    lvSelectedEmps.Adapter = new SelectEmpAdapter(this, WorkerList);
                }
                else
                {
                    Toast.MakeText(this, "Duplicate found.", ToastLength.Long).Show();
                }
                actxtWorkers.Text = "";
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, "Something went wrong:" + ex.Message, ToastLength.Long).Show();
            }
           
        }



        //private void BtnSelectEndTime_Click(object sender, EventArgs e)         //selecting times/dates
        //{
        //    TimePickerFragment frag1 = TimePickerFragment.NewInstance(delegate (TimeSpan time1)
        //    {
        //        btnSelectEndTime.Text = "End Time: " + time1.ToString();
        //        EndTime = time1.ToString();
        //    });
        //    frag1.Show(FragmentManager, TimePickerFragment.TAG);
        //}

        private void BtnSelectTime_Click(object sender, EventArgs e)
        {
            TimePickerFragment frag = TimePickerFragment.NewInstance(delegate (TimeSpan time)
            {
                btnSelectTime.Text = "Start Time: " + time.ToString();
                StartTime = time.ToString();
            });
            frag.Show(FragmentManager, TimePickerFragment.TAG);
        }

        private void BtnSelectDate_Click(object sender, EventArgs e)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                
                btnSelectDate.Text = "Date: " + time.ToString("yyyy-MM-dd");
                DateofJob = time.ToString("yyyy-MM-dd");
                
            });
            
            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        private void BtnNext_Click(object sender, EventArgs e)
        {
            var msgInfo = new Intent(this, typeof(AssignJobP4Activity));

            msgInfo.PutExtra("JobTime", StartTime);
            msgInfo.PutExtra("JobEndTime", EndTime);
            msgInfo.PutExtra("JobDate", DateofJob);
            msgInfo.PutExtra("JobTruckNumbers", txtTruckNo.Text);
            msgInfo.PutExtra("JobName", JobName);
            msgInfo.PutExtra("JobID", JobID);
            msgInfo.PutExtra("JobClients", JobClient);
            msgInfo.PutExtra("JobNumber", JobNumber);
            msgInfo.PutExtra("JobArea", txtArea.Text);

            StartActivity(msgInfo);

            

        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            Finish();
        }

        private void LvSelectedEmps_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)     //long press for deleting workers from job
        {
            AlertDialog.Builder alertDialog = new AlertDialog.Builder(this);

            alertDialog.SetMessage("Do you want to remove selected worker from this job?");
            alertDialog.SetPositiveButton("Yes", delegate
            {
                var selectedEmp = WorkerList[e.Position];
                WorkerList.Remove(selectedEmp);
                lvSelectedEmps.Adapter = new SelectEmpAdapter(this, WorkerList);
                alertDialog.Dispose();
            });
            alertDialog.SetNegativeButton("Cancel", delegate
            {
                alertDialog.Dispose();
            });
            alertDialog.Show();

        }

        public class DatePickerFragment : DialogFragment,
                                  DatePickerDialog.IOnDateSetListener
        {
            // TAG can be any string of your choice.
            public static readonly string TAG = "X:" + typeof(DatePickerFragment).Name.ToUpper();

            // Initialize this value to prevent NullReferenceExceptions.
            Action<DateTime> _dateSelectedHandler = delegate { };

            public static DatePickerFragment NewInstance(Action<DateTime> onDateSelected)
            {
                DatePickerFragment frag = new DatePickerFragment();
                frag._dateSelectedHandler = onDateSelected;
                return frag;
            }

            public override Dialog OnCreateDialog(Bundle savedInstanceState)
            {
                DateTime currently = DateTime.Now;
                DatePickerDialog dialog = new DatePickerDialog(Activity,
                                                               this,
                                                               currently.Year,
                                                               currently.Month - 1,
                                                               currently.Day + 1);        //NEEDSTEST
                return dialog;
            }

            public void OnDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth)
            {
                // Note: monthOfYear is a value between 0 and 11, not 1 and 12!
                DateTime selectedDate = new DateTime(year, monthOfYear + 1, dayOfMonth);
                Log.Debug(TAG, selectedDate.ToLongDateString());
                _dateSelectedHandler(selectedDate);
            }
        }

        //Time Picker
        public class TimePickerFragment : DialogFragment,
                                  TimePickerDialog.IOnTimeSetListener
        {
            // TAG can be any string of your choice.
            public static readonly string TAG = "Y:" + typeof(TimePickerFragment).Name.ToUpper();

            // Initialize this value to prevent NullReferenceExceptions.
            Action<TimeSpan> _timeSelectedHandler = delegate { };

            public static TimePickerFragment NewInstance(Action<TimeSpan> onTimeSet)
            {
                TimePickerFragment frag = new TimePickerFragment();
                frag._timeSelectedHandler = onTimeSet;
                return frag;
            }

            public override Dialog OnCreateDialog(Bundle savedInstanceState)
            {
                Calendar c = Calendar.Instance;
                int hour = 6;
                int minute = 00;
                //bool is24HourView = false;
                TimePickerDialog dialog = new TimePickerDialog(Activity,
                                                               this,
                                                               hour,
                                                               minute,
                                                               false);
                return dialog;
            }

            public void OnTimeSet(TimePicker view, int hourOfDay, int minute)
            {
                //Do something when time chosen by user
                TimeSpan selectedTime = new TimeSpan(hourOfDay, minute, 00);
                Log.Debug(TAG, selectedTime.ToString());
                _timeSelectedHandler(selectedTime);
            }
        }
    }
}