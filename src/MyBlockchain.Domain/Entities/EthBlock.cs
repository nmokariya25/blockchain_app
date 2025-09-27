using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlockchain.Domain.Entities
{
    public class EthBlock

    {
        [Key]
        public int Id { get; set; } 
        public string? Name { get; set; }
        public long Height { get; set; }
        public string Hash { get; set; }
        public DateTime Time { get; set; }
        public string? LatestUrl { get; set; }
        public string? PreviousHash { get; set; }
        public string? PreviousUrl { get; set; }
        public int PeerCount { get; set; }
        public int UnconfirmedCount { get; set; }
        public long HighGasPrice { get; set; }
        public long MediumGasPrice { get; set; }
        public long LowGasPrice { get; set; }
        public long HighPriorityFee { get; set; }
        public long MediumPriorityFee { get; set; }
        public long LowPriorityFee { get; set; }
        public long BaseFee { get; set; }
        public long LastForkHeight { get; set; }
        public string? LastForkHash { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

}
