using Sneat.MVC.Common;
using Sneat.MVC.Models.DTO.Project;
using Sneat.MVC.Models.Enum;
using System;
using System.Collections.Generic;

namespace Sneat.MVC.Models.DTO.WorkPackage
{
    public class WorkPackageInputModel
    {
        public string Subject { get; set; }
        public int Type { get; set; }
        public int Status { get; set; }
        public int? EstimateTime { get; set; }
        public double? SpentTime { get; set; }
        public int? RemainingTime { get; set; }
        public int? PriorityPoint { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public string Description { get; set; }

        public int? AssigneeID { get; set; }
        public int? AssignorID { get; set; }
        public List<int> MemberIds { get; set; }
        public int? ProjectID { get; set; }
        public int? WorPackageID { get; set; }
        public int? SprintID { get; set; }
    }

    public class WorkPackageOutputModel : WorkPackageInputModel
    {
        public int ID { get; set; }
        public DateTime CreateDate { get; set; }
        public List<int> MemberIds { get; set; }
        public double? CompletePercent { get; set; }
        public string StatusStr 
        {
            get
            {
                switch (Status)
                {
                    case (int)WorkPackageStatus.New:
                        return SystemParam.TASK_STATUS_NEW;
                    case (int)WorkPackageStatus.InProgress:
                        return SystemParam.TASK_STATUS_INPROGRESS;
                    case (int)WorkPackageStatus.InTesting:
                        return SystemParam.TASK_STATUS_IN_TESTING;
                    case (int)WorkPackageStatus.Closed:
                        return SystemParam.TASK_STATUS_CLOSED;
                    case (int)WorkPackageStatus.Developed:
                        return SystemParam.TASK_STATUS_DEVELOPED;
                    case (int)WorkPackageStatus.Tested:
                        return SystemParam.TASK_STATUS_TESTED;
                    case (int)WorkPackageStatus.Rejected:
                        return SystemParam.TASK_STATUS_REJECTED;
                    case (int)WorkPackageStatus.TestFailed:
                        return SystemParam.TASK_STATUS_TEST_FAILED;
                    default:
                        return "Unknown Status";
                }
            }
        }

        public string TypeStr 
        {
            get
            {
                switch (Type)
                {
                    case (int)WorkPackageType.UserStory:
                        return SystemParam.TASK_TYPE_USER_STORY;
                    case (int)WorkPackageType.Task:
                        return SystemParam.TASK_TYPE_USER_TASK;
                    case (int)WorkPackageType.Bug:
                        return SystemParam.TASK_TYPE_USER_BUG;
                    case (int)WorkPackageType.Feature:
                        return SystemParam.TASK_TYPE_USER_FEATURE;
                    default:
                        return "Unknown type";
                }
            }
        }

        public string PriorityPointStr 
        {
            get 
            {
                switch (PriorityPoint)
                {
                    case SystemParam.PRIORITY_TYPE_HOT_FIX:
                        return SystemParam.PRIORITY_TYPE_HOT_FIX_STR;
                    case SystemParam.PRIORITY_TYPE_IMMEDIATE:
                        return SystemParam.PRIORITY_TYPE_IMMEDIATE_STR;
                    case SystemParam.PRIORITY_TYPE_HIGH:
                        return SystemParam.PRIORITY_TYPE_HIGH_STR;
                    case SystemParam.PRIORITY_TYPE_NORMAL:
                        return SystemParam.PRIORITY_TYPE_NORMAL_STR;
                    case SystemParam.PRIORITY_TYPE_LOW:
                        return SystemParam.PRIORITY_TYPE_LOW_STR;
                    default:
                        return "Unknown Priority";
                }
            }
        }

        public string StartDateStr
        {
            get
            {
                if (StartDate != null)
                    return StartDate.Value.ToString(SystemParam.CONVERT_DATETIME_DATE_PICKER_BASIC);
                else
                    return null;
            }
        }

        public string FinishDateStr
        {
            get
            {
                if (FinishDate != null)
                    return FinishDate.Value.ToString(SystemParam.CONVERT_DATETIME_DATE_PICKER_BASIC);
                else
                    return null;
            }
        }

        public string AssigneeName { get; set; }
        public string AssigneeEmail { get; set; }
        public string AssigneeAvatar { get; set; }

        public List<WorkPackageOutputModel> ListTasks { get; set; }
        public List<ProjectUserModel> ListUsers { get; set; }
    }
}