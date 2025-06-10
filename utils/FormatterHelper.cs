using DTOs;
using Models;

namespace Formatter
{
    public class FormatterHelper
    {
        public static EquipResponseDTO EquipFormater(Equip equipData)
        {
            if (equipData.Leader == null)
            {
                throw new Exception("Error: Could not locate team leader.");
            }

            if (equipData.Project == null)
            {
                throw new Exception("Error: The specified project doesn't exist.");
            }

            EquipResponseDTO equipFormated = new EquipResponseDTO
            {
                Id = equipData.Id,
                LeaderId = equipData.LeaderId,
                LeaderName = equipData.Leader.Name,
                ProjectId = equipData.ProjectId,
                ProjectName = equipData.Project.Name,
                Departament = equipData.Departament,
                Description = equipData.Description
            };

            return equipFormated;
        }

        public static FormattedTaskDTO TaskFormatter(Models.Task task)
        {
            if (task.Equip == null)
            {
                throw new Exception("Error: Team not found.");
            }

            FormattedTaskDTO newFormTask = new FormattedTaskDTO
            {
                Id = task.Id,
                Description = task.Description,
                Priority = task.Priority,
                InitialDate = task.InitialDate,
                FinalDate = task.FinalDate,
                EquipId = task.EquipId,
                EquipDepartament = task.Equip.Departament,
                IsDone = task.IsDone
            };

            if (task.AssigneeId != null && task.Assignee != null)
            {
                newFormTask.AssigneeId = task.AssigneeId;
                newFormTask.AssigneeName = task.Assignee.Name;
            }

            return newFormTask;
        }
    }
}