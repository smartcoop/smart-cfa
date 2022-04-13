CREATE VIEW [dbo].[v_TrainingDetails]
AS
SELECT training.Id, detail.Title, detail.Methodology, detail.Goal, category.TrainingTopicId, detail.Language, trainer.FirstName, trainer.LastName, training.StatusId
FROM [Catalog].[dbo].[Training] AS training
JOIN [Catalog].[dbo].[TrainingDetail] AS detail ON training.Id = detail.TrainingId
JOIN [Catalog].[dbo].[TrainerAssignment] trainingTrainer ON training.Id = trainingTrainer.TrainingId
JOIN [Catalog].[dbo].[Trainer] trainer ON trainingTrainer.TrainerId = trainer.Id
JOIN [Catalog].[dbo].[TrainingCategory] category ON training.Id = category.TrainingId
