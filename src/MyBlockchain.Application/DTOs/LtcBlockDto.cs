using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyBlockchain.Application.DTOs
{
    public class LtcBlockDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("height")]
        public int Height { get; set; }

        [JsonPropertyName("hash")]
        public string Hash { get; set; }

        [JsonPropertyName("time")]
        public DateTime Time { get; set; }

        [JsonPropertyName("latest_url")]
        public string LatestUrl { get; set; }

        [JsonPropertyName("previous_hash")]
        public string PreviousHash { get; set; }

        [JsonPropertyName("previous_url")]
        public string PreviousUrl { get; set; }

        [JsonPropertyName("peer_count")]
        public int PeerCount { get; set; }

        [JsonPropertyName("unconfirmed_count")]
        public int UnconfirmedCount { get; set; }

        [JsonPropertyName("high_fee_per_kb")]
        public int HighFeePerKb { get; set; }

        [JsonPropertyName("medium_fee_per_kb")]
        public int MediumFeePerKb { get; set; }

        [JsonPropertyName("low_fee_per_kb")]
        public int LowFeePerKb { get; set; }

        [JsonPropertyName("last_fork_height")]
        public int LastForkHeight { get; set; }

        [JsonPropertyName("last_fork_hash")]
        public string LastForkHash { get; set; }
    }
}
