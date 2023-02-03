using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Models
{
    public class UserSettings
    {
        public string Guid { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string ConnectionString { get; set; }
        public DateTime PayDate { get; set; } = new DateTime(1, 1, 1);
    }
}
