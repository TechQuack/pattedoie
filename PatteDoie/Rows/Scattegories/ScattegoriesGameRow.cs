﻿using PatteDoie.Models.Scattergories;

namespace PatteDoie.Rows.Scattegories
{
    public class ScattegoriesGameRow
    {
        public Guid Id { get; set; }
        public ScattergoriesPlayer[] Players { get; set; }
        public int MaxRound { get; set; }
        public int CurrentRound { get; set; }
        public char CurrentLetter { get; set; }
        public ScattergoriesCategory[] Categories { get; set; }
    }
}