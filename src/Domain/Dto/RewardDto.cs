using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dto
{
    public class RewardDto:AuditDto
    {
        public string Id { get; set; }
        public string RewardName { get; set; }
        public int Quantity { get; set; }
        public List<SegmentDto> Segments { get; set; }
        public List<ExtraDataDto> ExtraDatas { get; set; }
    }
}
