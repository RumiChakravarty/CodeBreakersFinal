using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Code_BreakersEventBudget.Models
{
    public class NewUser
    {
        [Required(ErrorMessage = "Non empty, only characters.")]
        [StringLength(20, ErrorMessage = "up to 20 character")]
        public string Name { set; get; }

        [Required(ErrorMessage = "Please enter your email address")]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Please enter your email address")]
        public string Email { get; set; }
    }
}