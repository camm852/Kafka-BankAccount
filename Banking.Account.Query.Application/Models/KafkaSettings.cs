using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Account.Query.Application.Models
{
    public class KafkaSettings
    {
        public string GroupId { get; set; } = string.Empty;
        public string HostName { get; set; } = string.Empty;
        public string Port { get; set; } = string.Empty;


    }
}
