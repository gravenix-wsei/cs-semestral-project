//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace cs_semestral_project
{
    using System;
    using System.Collections.ObjectModel;
    
    public partial class reservation
    {
        public int reservation_id { get; set; }
        public Nullable<int> room_id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public Nullable<int> id_address_customer { get; set; }
        public System.DateTime date_from { get; set; } = DateTime.Now;
        public System.DateTime date_to { get; set; } = DateTime.Now;
    
        public virtual address address { get; set; }
        public virtual room room { get; set; }
    }
}
