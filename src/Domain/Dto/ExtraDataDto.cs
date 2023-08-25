using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dto
{
    public class ExtraDataDto : AuditDto
    {
        public string Id { get; set; }
        public string ExtraDataName { get; set; }
        public string ExtraDataValue { get; set; }
        public string RewardId { get; set; }
        public RewardDto Reward { get; set; }
    }
}
