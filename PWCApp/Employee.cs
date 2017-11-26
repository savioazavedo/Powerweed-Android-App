namespace PWBackend
{
    using System;
    using System.Collections.Generic;
    
    public partial class Employee
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Employee()
        {
            this.EmployeeJobs = new HashSet<EmployeeJob>();
        }
    
        public int empID { get; set; }
        public string empNAME { get; set; }
        public string empType { get; set; }
        public string empMobile { get; set; }
        public string empPhone { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmployeeJob> EmployeeJobs { get; set; }
    }
}
