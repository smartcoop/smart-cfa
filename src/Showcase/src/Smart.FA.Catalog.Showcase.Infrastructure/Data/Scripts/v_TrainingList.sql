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
          ,detail.Goal
          ,detail.Methodology
	FROM [Cfa].[Training] AS training
	JOIN [Cfa].[TrainingLocalizedDetails] AS detail ON training.Id = detail.TrainingId
	JOIN [Cfa].[TrainingTopic] category ON training.Id = category.TrainingId
	JOIN [Cfa].[TrainerAssignment] trainingTrainer ON training.Id = trainingTrainer.TrainingId
	JOIN [Cfa].[Trainer] trainer ON trainingTrainer.TrainerId = trainer.Id
