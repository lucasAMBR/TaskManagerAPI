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
                throw new Exception("Leader not found");
            }

            if (equipData.Project == null)
            {
                throw new Exception("cannot find this project");
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
    }
}