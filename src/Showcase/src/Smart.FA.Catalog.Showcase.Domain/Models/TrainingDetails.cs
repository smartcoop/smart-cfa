// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;

namespace Smart.FA.Catalog.Showcase.Domain.Models;

public partial class TrainingDetails
{
    public int Id { get; set; }
    public string TrainingTitle { get; set; } = null!;
    public string? Methodology { get; set; }
    public string? Goal { get; set; }
    public string? PracticalModalities { get; set; }
    public int TrainingTopicId { get; set; }
    public string Language { get; set; } = null!;
    public string TrainerFirstName { get; set; } = null!;
    public string TrainerLastName { get; set; } = null!;
    public int TrainerId { get; set; }
    public string TrainerTitle { get; set; } = null!;
    public int StatusId { get; set; }
    public string? ProfileImagePath { get; set; }
}
