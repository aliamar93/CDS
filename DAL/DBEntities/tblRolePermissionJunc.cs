//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAL.DBEntities
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblRolePermissionJunc
    {
        public long RolePermissionJuncID { get; set; }
        public int PermissionID { get; set; }
        public int RoleID { get; set; }
    
        public virtual tblPermission tblPermission { get; set; }
        public virtual tblRole tblRole { get; set; }
    }
}
