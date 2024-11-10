using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sneat.MVC.Models.DTO.Task
{
    public class UpdateTaskStatusModel
    {
        public int ID { get; set; }
        public int Status { get; set; }
    }
}