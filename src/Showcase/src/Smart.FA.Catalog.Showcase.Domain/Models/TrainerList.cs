// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;

namespace Smart.FA.Catalog.Showcase.Domain.Models;

public partial class TrainerList
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string? ProfileImagePath { get; set; }
    public int? TrainingCount { get; set; }
}
