CREATE VIEW [Cfa].[v_TrainingList]
	AS
	SELECT training.Id
          ,detail.Title
          ,detail.Language
          ,trainer.FirstName
          ,trainer.LastName
          ,category.TopicId
          ,training.TrainingStatusTypeId
          ,trainer.Id AS TrainerId
	FROM [Catalog].[Cfa].[Training] AS training
	JOIN [Catalog].[Cfa].[TrainingLocalizedDetails] AS detail ON training.Id = detail.TrainingId
	JOIN [Catalog].[Cfa].[TrainingTopic] category ON training.Id = category.TrainingId
	JOIN [Catalog].[Cfa].[TrainerAssignment] trainingTrainer ON training.Id = trainingTrainer.TrainingId
	JOIN [Catalog].[Cfa].[Trainer] trainer ON trainingTrainer.TrainerId = trainer.Id
