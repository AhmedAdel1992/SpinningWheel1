using Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class SpinningWheel : BaseEntityAudit<string>
    {
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
        public List<Segment> Segments { get; set; }
    }
}
