CREATE VIEW [Cfa].[v_TrainerList]
AS
SELECT
    trainer.Id
    ,trainer.FirstName
    ,trainer.LastName
    ,trainer.Title
    ,trainer.ProfileImagePath
    ,count(assignment.TrainingId) As TrainingCount
FROM Cfa.Trainer trainer
    INNER JOIN Cfa.TrainerAssignment assignment
    ON assignment.TrainerId = trainer.Id
    INNER JOIN Cfa.Training training ON
    assignment.TrainingId = training.Id
    INNER JOIN Cfa.TrainingStatusType trainingStatusType
    ON trainingStatusType.Id = training.TrainingStatusTypeId
WHERE TrainingStatusType.Name = 'Published'
GROUP BY trainer.Id, trainer.FirstName, trainer.LastName, trainer.Title, trainer.ProfileImagePath
HAVING count(assignment.TrainingId) >= 1
