using Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Reward : BaseEntityAudit<string>
    {
        public string RewardName { get; set; }
        public int Quantity { get; set; }
        public int Consumed { get; set; } = 0;
        public List<Segment> Segments { get; set; }
        public List<ExtraData> ExtraDatas { get; set; }
    }
}
