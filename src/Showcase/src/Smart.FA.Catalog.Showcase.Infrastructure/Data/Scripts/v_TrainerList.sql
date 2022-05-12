CREATE VIEW [Cfa].[v_TrainerList]
AS
SELECT
    trainer.Id
    ,trainer.FirstName
    ,trainer.LastName
    ,trainer.Title
    ,trainer.PofileImagePath
	,count(assignment.TrainingId) As TrainingCount
FROM Cfa.Trainer trainer
    INNER JOIN Cfa.TrainerAssignment assignment
    ON assignment.TrainerId = trainer.Id
    INNER JOIN Cfa.Training training ON
    assignment.TrainingId = training.Id
WHERE training.TrainingStatusTypeId = 3
GROUP BY trainer.Id, trainer.FirstName, trainer.LastName, trainer.Title, trainer.ProfileImagePath
HAVING count(assignment.TrainingId) > 1
