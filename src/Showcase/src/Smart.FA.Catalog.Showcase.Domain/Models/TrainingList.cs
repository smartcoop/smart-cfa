// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;

namespace Smart.FA.Catalog.Showcase.Domain.Models;

public partial class TrainingList
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Language { get; set; } = null!;
        public string TrainerFirstName { get; set; } = null!;
        public string TrainerLastName { get; set; } = null!;
        public int Topic { get; set; }
        public int Status { get; set; }
        public int TrainerId { get; set; }
        public string? Goal { get; set; }
        public string? Methodology { get; set; }
        public bool IsGivenBySmart { get; set; }
    }
