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

namespace PWCApp
{
    public class Job
    {
        public int JobID { get; set; }
        public string JobCLIENTS { get; set; }
        public int JobNUMBER { get; set; }
        public string JobWORK { get; set; }
        public string JobArea { get; set; }
    }
}