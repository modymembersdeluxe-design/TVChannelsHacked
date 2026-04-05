using System;
using System.Collections.Generic;

namespace TVChannelsHacked
{
    public sealed class ChannelIncident
    {
        public string ChannelName { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public DateTime IncidentTimeUtc { get; set; }
        public string IncidentType { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public List<string> SourceVideos { get; set; } = new List<string>();
        public List<string> ComingNext { get; set; } = new List<string>();

        public string DisplayTitle => $"{ChannelName} ({Country})";
    }
}
