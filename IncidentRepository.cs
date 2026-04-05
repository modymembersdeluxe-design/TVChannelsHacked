using System;
using System.Collections.Generic;

namespace TVChannelsHacked
{
    public static class IncidentRepository
    {
        public static List<ChannelIncident> BuildSeedData()
        {
            return new List<ChannelIncident>
            {
                new ChannelIncident
                {
                    ChannelName = "Cartoon Network Arabic",
                    Country = "United Arab Emirates",
                    Region = "MENA",
                    ContentType = "Kids / Animation",
                    IncidentType = "Promo/Ident take-over",
                    IncidentTimeUtc = DateTime.UtcNow.AddDays(-10),
                    Summary = "Source videos captured around a promo transition where the ident and next bumper were replaced.",
                    SourceVideos = new List<string>
                    {
                        "CNArabic_Source_IntroCapture.mp4",
                        "CNArabic_PromoHijack_Angle2.mp4",
                        "CNArabic_IdentSwap_Archive.mp4"
                    },
                    ComingNext = new List<string>
                    {
                        "Cartoon Replay Marathon",
                        "Promo Recovery Loop",
                        "Emergency Ident Fallback"
                    }
                },
                new ChannelIncident
                {
                    ChannelName = "Spacetoon Arabic",
                    Country = "Saudi Arabia",
                    Region = "MENA",
                    ContentType = "Kids / Music",
                    IncidentType = "Music block interruption",
                    IncidentTimeUtc = DateTime.UtcNow.AddDays(-3),
                    Summary = "Music segment reportedly interrupted by altered source feed and repeated hacked promo insertions.",
                    SourceVideos = new List<string>
                    {
                        "Spacetoon_MusicBlock_Before.mp4",
                        "Spacetoon_HackedPromo_Insert.mp4",
                        "Spacetoon_IdentRecovery.mp4"
                    },
                    ComingNext = new List<string>
                    {
                        "Next Planet Special",
                        "Ident Clean Feed",
                        "Source Verification Segment"
                    }
                },
                new ChannelIncident
                {
                    ChannelName = "Global Kids Feed",
                    Country = "United Kingdom",
                    Region = "Europe",
                    ContentType = "Kids / Family",
                    IncidentType = "Unexpected source routing",
                    IncidentTimeUtc = DateTime.UtcNow.AddDays(-1),
                    Summary = "Worldwide monitoring report describing short source routing issue and logo mismatch.",
                    SourceVideos = new List<string>
                    {
                        "GlobalKids_SourceSwitch_01.mp4",
                        "GlobalKids_LogoMismatch_02.mp4"
                    },
                    ComingNext = new List<string>
                    {
                        "Regional continuity",
                        "On-screen bug correction",
                        "Broadcast ops announcement"
                    }
                }
            };
        }
    }
}
