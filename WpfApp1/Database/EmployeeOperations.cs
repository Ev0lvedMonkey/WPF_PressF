//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WpfApp1.Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class EmployeeOperations
    {
        public int EmployeeID { get; set; }
        public int OperationID { get; set; }
        public int EO_ID { get; set; }
    
        public virtual Employees Employees { get; set; }
        public virtual Operations Operations { get; set; }
    }
}
