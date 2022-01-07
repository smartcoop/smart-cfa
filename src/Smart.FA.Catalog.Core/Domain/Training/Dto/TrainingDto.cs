namespace Core.Domain.Dto;

public class TrainingDto
{
    public int TrainingId { get; set; }
    public int StatusId { get; set; }
    public string Title { get; set; }
    public string Goal { get; set; }
    public string Language { get; set; }
}
