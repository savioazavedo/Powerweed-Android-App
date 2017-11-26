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
using Java.Util;
using Android.Util;

namespace PWCApp
{
    [Activity(Label = "EditPlanItemActivity")]
    public class EditPlanItemActivity : Activity
    {
        string JobTime, JobDate, startTime;
        int JobID;
        List<Employee> WorkerList;
        List<Employee> myList = GetEmployees.getEmployees();
        Button btnDate, btnTime, btnSave;
        TextView txtSelectedJob;
        EditText txtTruckNo, txtArea, txtNote;
        AutoCompleteTextView actxtWorkers;
        //List<JobsAssigned> jobsPlanList;
        ListView lvSelectedEmps;
        PlanHandler objplan = new PlanHandler();
        JOBSHandler objjob = new JOBSHandler();
        JobsAssigned ej = new JobsAssigned();
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EditPlanItem);

            WorkerList = new List<Employee>();
            //JobTime = Intent.GetStringExtra("AssignSTARTTIME");
            //JobInstructions = Intent.GetStringExtra("AssignINSTRUCTIONS");
            ////JobDate = Intent.GetStringExtra("JobDate");
            //JobTruckNo = Intent.GetStringExtra("AssignTRUCK");
            //JobName = Intent.GetStringExtra("AssignWork");
            JobID = Intent.GetIntExtra("AssignID", 0);
            //JobClient = Intent.GetStringExtra("AssignCLIENT");
            //JobNumber = Intent.GetIntExtra("AssignJOBNUM", 0);
            //JobArea = Intent.GetStringExtra("AssignAREA");
            ej = objplan.ExecuteGetSelectedRequest(JobID);


            JobDate = Convert.ToDateTime(ej.AssignSTARTTIME).ToShortDateString();
            JobTime = Convert.ToString(Convert.ToDateTime(ej.AssignSTARTTIME).TimeOfDay);
            Button btnBackE = FindViewById<Button>(Resource.Id.btnBackE);
            actxtWorkers = FindViewById<AutoCompleteTextView>(Resource.Id.actxtWorker);
            btnSave = FindViewById<Button>(Resource.Id.btnSaveEdited);
            btnDate = FindViewById<Button>(Resource.Id.btnSelectDate);
            btnTime = FindViewById<Button>(Resource.Id.btnSelectTime);
            txtSelectedJob = FindViewById<TextView>(Resource.Id.txtSelectedJob);
            txtArea = FindViewById<EditText>(Resource.Id.txtArea);
            txtTruckNo = FindViewById<EditText>(Resource.Id.txtTruckNumbers);
            txtNote = FindViewById<EditText>(Resource.Id.txtNote);
            lvSelectedEmps = FindViewById<ListView>(Resource.Id.lvCurrentWorkers);
            txtArea.Text = ej.AssignAREA;          
            txtTruckNo.Text = ej.AssignTRUCK;                                         //showing values in correct areas
            txtNote.Text = ej.AssignINSTRUCTIONS;

            txtSelectedJob.Text = ej.AssignJOBNUM + " - " + ej.AssignWORK + " - " + ej.AssignCLIENT;
            btnDate.Text = "Date: " + Convert.ToDateTime(ej.AssignSTARTTIME).ToShortDateString();
            btnTime.Text = "Start time: " + Convert.ToString(Convert.ToDateTime(ej.AssignSTARTTIME).TimeOfDay);


            foreach (var w in ej.EmployeeJobs)
            {
                Employee emp = new Employee();
                emp.empID = w.employeeJOBSID;                   // storing the employee jobs ID  
                emp.empNAME = w.EmpNAME;
                emp.empMobile = GetEmployees.getEmpNumber(w.EmpNAME);
                WorkerList.Add(emp);

                lvSelectedEmps.Adapter = new SelectEmpAdapter(this, WorkerList);
            }

            var workerAdapter = new ArrayAdapter<string>(this, Resource.Layout.list_item, GetEmployees.getEmpNames());        //autocomplete textbox
            actxtWorkers.Adapter = workerAdapter;
            btnDate.Click += BtnDate_Click;
            btnTime.Click += BtnTime_Click;
            actxtWorkers.ItemClick += ActxtWorkers_ItemClick;
            lvSelectedEmps.ItemLongClick += LvSelectedEmps_ItemLongClick;
            btnSave.Click += BtnSave_Click;
            btnBackE.Click += BtnBack_Click; 
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            Finish();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            JobsAssigned job = new JobsAssigned();
            startTime = Convert.ToDateTime(JobDate).ToString("yyyy-MM-dd") + "T" + JobTime;
            job.AssignJOBNUM = ej.AssignJOBNUM;
            job.AssignCLIENT = ej.AssignCLIENT;
            job.AssignWORK = ej.AssignWORK;
            job.AssignAREA = txtArea.Text;
            job.AssignID = ej.AssignID;
            job.AssignINSTRUCTIONS = txtNote.Text;
            job.AssignTRUCK = txtTruckNo.Text;
            job.TextSENT = null;
            job.AssignSTARTTIME = startTime;

            try
            {
                objplan.ExecutePutRequest(job);
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, "Something went wrong:" + ex.Message, ToastLength.Long).Show();
            }

            AlertDialog.Builder alertDialog = new AlertDialog.Builder(this);
            alertDialog.SetMessage("Job Updated.");
            alertDialog.SetPositiveButton("Okay", delegate
            {             
                alertDialog.Dispose();
            });
           
            alertDialog.Show();
        }

        private void BtnTime_Click(object sender, EventArgs e)
        {
            TimePickerFragment frag = TimePickerFragment.NewInstance(delegate (TimeSpan time)
            {
                btnTime.Text = "Start Time: " + time.ToString();
                JobTime = time.ToString();
            });
            frag.Show(FragmentManager, TimePickerFragment.TAG);
        }

        private void BtnDate_Click(object sender, EventArgs e)
        {
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {

                btnDate.Text = "Date: " + time.ToString("yyyy-MM-dd");
                JobDate = time.ToString("yyyy-MM-dd");

            });

            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        private void LvSelectedEmps_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            AlertDialog.Builder alertDialog = new AlertDialog.Builder(this);

            alertDialog.SetMessage("Do you want to remove selected worker from this job?");
            alertDialog.SetPositiveButton("Yes", delegate
            {
                var selectedEmp = WorkerList[e.Position];

                // Delete from Database 

                if (objjob.DeleteEmployeefromJobs(selectedEmp.empID.ToString()))
                {
                    WorkerList.Remove(selectedEmp);
                    lvSelectedEmps.Adapter = new SelectEmpAdapter(this, WorkerList);
                    Toast.MakeText(this, "Employee removed from the Job", ToastLength.Long).Show();
                }
                else
                {
                    Toast.MakeText(this, "Something went wrong !!! Can't remove the employee , try the website", ToastLength.Long).Show();
                }

                alertDialog.Dispose();
            });
            alertDialog.SetNegativeButton("Cancel", delegate
            {
                alertDialog.Dispose();
            });
            alertDialog.Show();
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

                    EmployeeJob tempej = new EmployeeJob();
                    tempej.AssignID = ej.AssignID;
                    tempej.EmpNAME = actxtWorkers.Text;

                    if (objjob.AddEmployeeToJobs(tempej))
                    {
                        WorkerList.Add(emp);
                        lvSelectedEmps.Adapter = new SelectEmpAdapter(this, WorkerList);
                        Toast.MakeText(this, "Worker added to the Job", ToastLength.Long).Show();
                    }
                    else
                    {
                        Toast.MakeText(this, "Something went wrong !!! Cannot add worker to jobs , try the website", ToastLength.Long).Show();
                    }

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