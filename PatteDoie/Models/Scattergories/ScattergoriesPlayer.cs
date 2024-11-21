﻿using PatteDoie.Models.Platform;

namespace PatteDoie.Models.Scattergories
{
    public class ScattergoriesPlayer
    {
        public Guid Id { get; set; }
        public int Score { get; set; }
        public required PlatformUser User { get; set; }
    }
}
