using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlockchain.Domain.Entities
{
    public class ApiAuditLog
    {
        public int Id { get; set; }
        public string HttpMethod { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public string QueryString { get; set; } = string.Empty;
        public string RequestBody { get; set; } = string.Empty;
        public string Scheme { get; set; } = string.Empty;
        public bool IsHttps { get; set; }
        public int StatusCode { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime ResponseDate { get; set; }
    }
}
