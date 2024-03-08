using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailWorkerService.Models
{
    public class EmailSettings
    {
        public string host { get; set; }
        public string password { get; set; }
        public string email { get; set; }
    }
}
