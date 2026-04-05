using System;
using System.Collections.Generic;

namespace TVChannelsHacked
{
    public static class IncidentRepository
    {
        public static List<ChannelIncident> BuildSeedData()
        {
            var incidents = new List<ChannelIncident>
            {
                new ChannelIncident
                {
                    ChannelName = "Cartoon Network Arabic",
                    Country = "United Arab Emirates",
                    Region = "MENA",
                    ContentType = "Animated series and cartoons",
                    IncidentType = "Promo/Ident take-over",
                    IncidentTimeUtc = DateTime.UtcNow.AddDays(-10),
                    Summary = "Hacked played scenario with coming-next playback mismatch and swapped ident output.",
                    SourceVideos = new List<string>
                    {
                        "CNArabic_Source_IntroCapture.mp4",
                        "CNArabic_PromoHijack_Angle2.avi",
                        "CNArabic_IdentSwap_Archive.wmv",
                        "CNArabic_CutawayAlert.3gp"
                    },
                    SourceAudioTracks = new List<string>
                    {
                        "CNArabic_AudioBed_Hacked_Played.mp3",
                        "CNArabic_Ident_Audio_Shift.wav",
                        "CNArabic_Continuity_Override.ogg"
                    },
                    ShowCuts = new List<string>
                    {
                        "Cartoon block cut after hacked promo",
                        "Ad break cut to fallback ident"
                    },
                    ComingNext = new List<string>
                    {
                        "The Amazing World of Gumball",
                        "Ben 10",
                        "Emergency Ident Fallback"
                    },
                    IsComingNextHijack = true,
                    ScheduledBlockTimeUtc = DateTime.UtcNow.Date.AddHours(16).AddMinutes(5)
                },
                new ChannelIncident
                {
                    ChannelName = "Spacetoon Arabic",
                    Country = "Saudi Arabia",
                    Region = "MENA",
                    ContentType = "Music / Action Planets",
                    IncidentType = "Music block interruption",
                    IncidentTimeUtc = DateTime.UtcNow.AddDays(-3),
                    Summary = "Music Block Before Hacked played / Hacked Promo played / Ident Hacked played / Shows hacked played cuts detected.",
                    SourceVideos = new List<string>
                    {
                        "Spacetoon_MusicBlock_Before_Hacked_Played.mp4",
                        "Spacetoon_HackedPromo_Played.avi",
                        "Spacetoon_Ident_Hacked_Played.wmv",
                        "Spacetoon_Shows_Hacked_Played_Cuts.3gp"
                    },
                    SourceAudioTracks = new List<string>
                    {
                        "Spacetoon_Music_Audio_Hacked_Played.mp3",
                        "Spacetoon_Promo_Audio_Hacked_Bed.wav",
                        "Spacetoon_Ident_Audio_Hacked_Variant.ogg"
                    },
                    ShowCuts = new List<string>
                    {
                        "Show cut into hacked promo",
                        "Cut from ident to emergency card",
                        "Cut back to music block"
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
                    ChannelName = "Spacetoon Arabic",
                    Country = "Saudi Arabia",
                    Region = "MENA",
                    ContentType = "Kids / Action",
                    IncidentType = "Coming-next block hijack",
                    IncidentTimeUtc = DateTime.UtcNow.AddHours(-5),
                    ScheduledBlockTimeUtc = DateTime.UtcNow.Date.AddHours(22).AddMinutes(29).AddSeconds(23),
                    IsComingNextHijack = true,
                    Summary = "Space Power block 10:29:23 PM hacked coming-next playback details and related source videos were captured.",
                    SourceVideos = new List<string>
                    {
                        "Spacetoon_SpacePowerBlock_ComingNext_Hacked_102923PM.mp4",
                        "Spacetoon_SpacePowerBlock_Promo_Hijack_Played.avi",
                        "Spacetoon_SpacePowerBlock_Ident_Recovery.wmv",
                        "Spacetoon_SpacePowerBlock_QuickCapture.3gp"
                    },
                    SourceAudioTracks = new List<string>
                    {
                        "SpacePower_Audio_Hacked_Played_102923.mp3",
                        "SpacePower_Promo_Audio_Override.wav",
                        "SpacePower_Ident_Bed_Override.ogg"
                    },
                    ShowCuts = new List<string>
                    {
                        "Cut from coming-next to hacked Space Power slate",
                        "Cut from hacked promo to ident recovery"
                    },
                    ComingNext = new List<string>
                    {
                        "10:29:23 PM — Space Power block (hacked play-out)",
                        "Emergency continuity card",
                        "Clean source fallback"
                    }
                }
            };

            incidents.AddRange(BuildGlobalKidsChannels());
            return incidents;
        }

        private static IEnumerable<ChannelIncident> BuildGlobalKidsChannels()
        {
            return new List<ChannelIncident>
            {
                MakeScenario("Disney Channel", "USA", "North America", "Shows for kids and teenagers"),
                MakeScenario("Disney Jr.", "USA", "North America", "Preschool programming"),
                MakeScenario("Disney XD", "USA", "North America", "Kids and teen action/comedy"),
                MakeScenario("Nickelodeon", "USA", "North America", "Children's entertainment"),
                MakeScenario("Nick Jr.", "USA", "North America", "Preschool educational shows"),
                MakeScenario("Nicktoons", "USA", "North America", "Animation-focused kids content"),
                MakeScenario("CBeebies", "United Kingdom", "Europe", "BBC preschool learning content"),
                MakeScenario("CBBC", "United Kingdom", "Europe", "BBC educational entertainment"),
                MakeScenario("Boomerang", "United Kingdom", "Europe", "Classic cartoons"),
                MakeScenario("Pop", "United Kingdom", "Europe", "Animated series and cartoons"),
                MakeScenario("Pop Max", "United Kingdom", "Europe", "Animated series and cartoons"),
                MakeScenario("Tiny Pop", "United Kingdom", "Europe", "Preschool animation"),
                MakeScenario("Discovery Family Channel", "USA", "North America", "Educational and entertainment programs"),
                MakeScenario("Gulli", "France", "Europe", "Free-to-air kids channel"),
                MakeScenario("Okoo", "France", "Europe", "Dedicated youth service"),
                MakeScenario("Canal+ Kids", "France", "Europe", "Premium kids programming"),
                MakeScenario("Canal J", "France", "Europe", "Programming aimed at ages 7–12"),
                MakeScenario("Cartoonito (France)", "France", "Europe", "Young children programming"),
                MakeScenario("Disney Channel (France)", "France", "Europe", "Kid and teen entertainment"),
                MakeScenario("Discovery Kids", "Brazil", "Latin America", "Preschool and young children"),
                MakeScenario("Cartoon Network (Brazil)", "Brazil", "Latin America", "Animation and comedy"),
                MakeScenario("Cartoonito", "Brazil", "Latin America", "Educational and fun content"),
                MakeScenario("Nickelodeon Portugal", "Portugal", "Europe", "Animated and youth content"),
                MakeScenario("Nick Jr. Portugal", "Portugal", "Europe", "Preschool lineup"),
                MakeScenario("Disney Junior Portugal", "Portugal", "Europe", "Disney preschool content"),
                MakeScenario("Paramount Kids", "Portugal", "Europe", "Specialized kids programming"),
                MakeScenario("MBC 3", "Saudi Arabia", "MENA", "Arabic dubbed and local youth shows"),
                MakeScenario("Nickelodeon Arabia", "United Arab Emirates", "MENA", "Dubbed Nickelodeon favorites")
            };
        }

        private static ChannelIncident MakeScenario(string channelName, string country, string region, string contentType)
        {
            return new ChannelIncident
            {
                ChannelName = channelName,
                Country = country,
                Region = region,
                ContentType = contentType,
                IncidentType = "Coming-next playback hacked",
                IncidentTimeUtc = DateTime.UtcNow.AddHours(-2),
                ScheduledBlockTimeUtc = DateTime.UtcNow.Date.AddHours(18).AddMinutes(40),
                IsComingNextHijack = true,
                Summary = "Hacked played scenario including coming-next playback details, video source replacement, and audio track overrides.",
                SourceVideos = new List<string>
                {
                    $"{SafeName(channelName)}_ComingNext_Hacked.mp4",
                    $"{SafeName(channelName)}_Promo_Hacked.avi",
                    $"{SafeName(channelName)}_Ident_Hacked.wmv",
                    $"{SafeName(channelName)}_Emergency_Capture.3gp"
                },
                SourceAudioTracks = new List<string>
                {
                    $"{SafeName(channelName)}_Audio_Hacked.mp3",
                    $"{SafeName(channelName)}_Audio_Fallback.wav",
                    $"{SafeName(channelName)}_Audio_Bed.ogg"
                },
                ShowCuts = new List<string>
                {
                    "Cut to hacked promo",
                    "Cut to hacked ident",
                    "Cut to clean recovery"
                },
                ComingNext = new List<string>
                {
                    "Hacked coming-next bumper",
                    "Emergency continuity",
                    "Recovered clean feed"
                }
            };
        }

        private static string SafeName(string channel)
        {
            return channel
                .Replace(" ", string.Empty)
                .Replace("+", "Plus")
                .Replace("/", string.Empty)
                .Replace("(", string.Empty)
                .Replace(")", string.Empty)
                .Replace(".", string.Empty);
        }
    }
}
