using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels
{
    public class NotificationRequestModel
    {
        public string? Title { get; set; }
        public string? DeviceToken { get; set; }
        public string NotificationId { get; set; }
        public string? PhysicalLocationId { get; set; }
        public DateTime CreationDate { get; set; }


    }
}
