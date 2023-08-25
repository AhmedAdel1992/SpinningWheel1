using Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ExtraData : BaseEntityAudit<string>
    {
        public string ExtraDataName { get; set; }
        public string ExtraDataValue { get; set; }
        public string RewardId { get; set; }
        public Reward Reward { get; set; }
    }
}
