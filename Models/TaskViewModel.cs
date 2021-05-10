
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskList.Models
{
    public class TaskViewModel
    {
        public int Id { get; set; }

        //public string UserEmail { get; set; }

        [Display(Name = "Task Description")]
        public string TaskDescription { get; set; }

        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; }

        [Display(Name = "Is Complete")]
        public bool IsComplete { get; set; }

        public IdentityUser User { get; set; }
    }
}
