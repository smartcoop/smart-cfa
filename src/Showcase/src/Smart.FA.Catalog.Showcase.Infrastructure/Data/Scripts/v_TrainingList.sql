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
    INNER JOIN [Cfa].[TrainingLocalizedDetails] AS detail
    ON training.Id = detail.TrainingId
    INNER JOIN [Cfa].[TrainingTopic] category
    ON training.Id = category.TrainingId
    INNER JOIN [Cfa].[TrainerAssignment] trainingTrainer
    ON training.Id = trainingTrainer.TrainingId
    INNER JOIN [Cfa].[Trainer] trainer
    ON trainingTrainer.TrainerId = trainer.Id
    INNER JOIN Cfa.TrainingStatusType trainingStatusType
    ON trainingStatusType.Id = training.TrainingStatusTypeId
WHERE TrainingStatusType.Name = 'Published'
