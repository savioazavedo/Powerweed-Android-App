namespace PWBackend
{
    using System;
    using System.Collections.Generic;
    
    public partial class EmployeeJob
    {
        public int employeeJOBSID { get; set; }
        public Nullable<int> AssignID { get; set; }
        //public Nullable<int> empID { get; set; }
        public string EmpNAME { get; set; }
        public virtual JobsAssigned JobsAssigned { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
