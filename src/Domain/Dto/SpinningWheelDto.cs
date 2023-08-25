using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dto
{
    public class SpinningWheelDto:AuditDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string BackgroundColor { get; set; }
        public string Color { get; set; }
        public string TextColor { get; set; }
        public string TopHeader { get; set; }
        public string BottomHeader { get; set; }
        public string ButtonText { get; set; }
        public string Type { get; set; }
        public bool IsActive { get; set; }
        public bool ShowRewardsImages { get; set; }
        public bool EnableRegistration { get; set; }
        public List<SegmentDto> Segments { get; set; }
    }
}
