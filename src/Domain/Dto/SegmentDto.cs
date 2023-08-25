using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dto
{
    public class SegmentDto:AuditDto
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public string Color { get; set; }
        public string TextColor { get; set; }
        public string SpinningWheelId { get; set; }
        public SpinningWheelDto SpinningWheel { get; set; }
        public string RewardId { get; set; }
        public RewardDto Reward{ get; set; }
    }
}
