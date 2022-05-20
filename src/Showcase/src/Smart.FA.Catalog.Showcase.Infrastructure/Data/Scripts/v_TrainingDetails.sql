CREATE VIEW [Cfa].[v_TrainingDetails]
AS
SELECT training.Id
        ,details.Title
        ,details.Methodology
        ,details.Goal
        ,details.PracticalModalities
        ,topic.TopicId
        ,details.Language
        ,trainer.FirstName
        ,trainer.LastName
        ,trainer.Id AS TrainerId
        ,trainer.Title AS trainerTitle
        ,training.TrainingStatusTypeId
FROM [Cfa].[Training] AS training
JOIN [Cfa].[TrainingLocalizedDetails] AS details ON training.Id = details.TrainingId
JOIN [Cfa].[TrainerAssignment] trainingTrainer ON training.Id = trainingTrainer.TrainingId
JOIN [Cfa].[Trainer] trainer ON trainingTrainer.TrainerId = trainer.Id
JOIN [Cfa].[TrainingTopic] topic ON training.Id = topic.TrainingId
