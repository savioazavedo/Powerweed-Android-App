namespace PWBackend
{
    using System;
    using System.Collections.Generic;

    public partial class JobsAssigned
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public JobsAssigned()
        {
            this.EmployeeJobs = new HashSet<EmployeeJob>();
        }

        public int AssignID { get; set; }
        public string AssignJOBNUM { get; set; }
        public string AssignCLIENT { get; set; }
        public string AssignWORK { get; set; }
        public string AssignAREA { get; set; }
        public string AssignSTARTTIME { get; set; }
        public string AssignINSTRUCTIONS { get; set; }
        public string AssignTRUCK { get; set; }
        public string TextSENT { get; set; }
        public string AssignENDTIME { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmployeeJob> EmployeeJobs { get; set; }
    }
}
