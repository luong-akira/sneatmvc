using System;

namespace Sneat.MVC.Models.DTO.Sprint
{
    public class SprintInputModel
    {
        public string Title { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; }
        public int ProjectID { get; set; }
        
    }

    public class SprintOutputModel  : SprintInputModel
    {
        public int ID { get; set; }
        public string ProjectName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}