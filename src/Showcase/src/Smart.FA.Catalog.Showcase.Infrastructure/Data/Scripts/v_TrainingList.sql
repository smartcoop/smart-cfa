CREATE VIEW [dbo].[v_TrainingList]
	AS
	SELECT training.Id, detail.Title, detail.Language, trainer.FirstName, trainer.LastName, category.TrainingTopicId, training.StatusId 
	FROM [Catalog].[dbo].[Training] AS training
	JOIN [Catalog].[dbo].[TrainingDetail] AS detail ON training.Id = detail.TrainingId
	JOIN [Catalog].[dbo].[TrainingCategory] category ON training.Id = category.TrainingId
	JOIN [Catalog].[dbo].[TrainerAssignment] trainingTrainer ON training.Id = trainingTrainer.TrainingId
	JOIN [Catalog].[dbo].[Trainer] trainer ON trainingTrainer.TrainerId = trainer.Id
