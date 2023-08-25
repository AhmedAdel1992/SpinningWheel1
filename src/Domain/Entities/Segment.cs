using Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Segment : BaseEntityAudit<string>
    {
        public string Label { get; set; }
        public string Color { get; set; }
        public string TextColor { get; set; }
        public string SpinningWheelId { get; set; }
        public SpinningWheel SpinningWheel { get; set; }
        public string RewardId { get; set; }
        public Reward Reward { get; set; }
    }
}
