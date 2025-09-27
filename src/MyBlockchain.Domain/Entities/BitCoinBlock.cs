using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlockchain.Domain.Entities
{
    public class BitCoinBlock
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public int Height { get; set; }
        public string Hash { get; set; } = string.Empty;
        public DateTime Time { get; set; }
        public string LatestUrl { get; set; } = string.Empty;
        public string PreviousHash { get; set; } = string.Empty;
        public string PreviousUrl { get; set; } = string.Empty;
        public int PeerCount { get; set; }
        public int UnconfirmedCount { get; set; }
        public int HighFeePerKb { get; set; }
        public int MediumFeePerKb { get; set; }
        public int LowFeePerKb { get; set; }
        public int LastForkHeight { get; set; }
        public string LastForkHash { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}