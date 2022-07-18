using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Smart.FA.Catalog.Showcase.Infrastructure.Migrations
{
    public partial class AddProfilImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // TrainingDetails.
            migrationBuilder.Sql(@"
               ALTER VIEW [Cfa].[v_TrainingDetails]
               AS
               SELECT training.Id,
                      details.Title,
                      details.Methodology,
                      details.Goal,
                      details.PracticalModalities,
                      topic.TopicId,
                      details.Language,
                      trainer.FirstName,
                      trainer.LastName,
                      trainer.Id AS TrainerId,
                      trainer.Title AS trainerTitle,
                      training.TrainingStatusTypeId,
                      trainer.ProfileImagePath
              FROM [Cfa].[Training] AS training
                    INNER JOIN [Cfa].[TrainingLocalizedDetails] AS details
                    ON training.Id = details.TrainingId
                    INNER JOIN [Cfa].[TrainerAssignment] trainingTrainer
                    ON training.Id = trainingTrainer.TrainingId
                    INNER JOIN [Cfa].[Trainer] trainer
                    ON trainingTrainer.TrainerId = trainer.Id
                    INNER JOIN [Cfa].[TrainingTopic] topic
                    ON training.Id = topic.TrainingId
                    INNER JOIN Cfa.TrainingStatusType trainingStatusType
                    ON trainingStatusType.Id = training.TrainingStatusTypeId
                WHERE TrainingStatusType.Name = 'Published'");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
               ALTER VIEW [Cfa].[v_TrainingDetails]
               AS
               SELECT training.Id,
                      details.Title,
                      details.Methodology,
                      details.Goal,
                      details.PracticalModalities,
                      topic.TopicId,
                      details.Language,
                      trainer.FirstName,
                      trainer.LastName,
                      trainer.Id AS TrainerId,
                      trainer.Title AS trainerTitle,
                      training.TrainingStatusTypeId
              FROM [Cfa].[Training] AS training
                    INNER JOIN [Cfa].[TrainingLocalizedDetails] AS details
                    ON training.Id = details.TrainingId
                    INNER JOIN [Cfa].[TrainerAssignment] trainingTrainer
                    ON training.Id = trainingTrainer.TrainingId
                    INNER JOIN [Cfa].[Trainer] trainer
                    ON trainingTrainer.TrainerId = trainer.Id
                    INNER JOIN [Cfa].[TrainingTopic] topic
                    ON training.Id = topic.TrainingId
                    INNER JOIN Cfa.TrainingStatusType trainingStatusType
                    ON trainingStatusType.Id = training.TrainingStatusTypeId
                WHERE TrainingStatusType.Name = 'Published'");
        }
    }
}
