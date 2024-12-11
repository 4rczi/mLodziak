using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class ApplicationLogging : BaseEntity
    {
        public int Id { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
        public string Logger { get; set; }
        public string Exception { get; set; }
        public string ClassName { get; set; }
        public string MethodName { get; set; }
        public string UserId { get; set; }
        public string SessionId { get; set; }
        public string CustomMessage { get; set; }
    }
}
