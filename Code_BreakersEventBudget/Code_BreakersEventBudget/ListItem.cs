//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Code_BreakersEventBudget
{
    using System;
    using System.Collections.Generic;

    public partial class ListItem
    {
        public string ListID { get; set; }
        public string ListItemID { get; set; }
        public string Price { get; set; }
        public string GiftFor { get; set; }
        public string Product { get; set; }

        public virtual List List { get; set; }
    }
}
