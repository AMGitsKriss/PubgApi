//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class SurvivalObjective
    {
        public int ID { get; set; }
        public string Task { get; set; }
        public int Quantity { get; set; }
        public Nullable<int> Reward { get; set; }
        public int Tier { get; set; }
        public bool IsDaily { get; set; }
    }
}
