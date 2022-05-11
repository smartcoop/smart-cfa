CREATE VIEW [Cfa].[v_TrainerList]
	AS
SELECT Cfa.Trainer.Id,
       Cfa.Trainer.FirstName,
       Cfa.Trainer.LastName,
       Cfa.Trainer.Title,
       Cfa.Trainer.ProfileImagePath
FROM Cfa.Trainer
WHERE
	(
		SELECT
			COUNT(*)
		FROM Cfa.TrainerAssignment assignment
		INNER JOIN Cfa.Training ON assignment.TrainingId = Cfa.Training.Id
		WHERE Cfa.Training.TrainingStatusTypeId = 3
			AND assignment.TrainerId = Trainer.Id
	) > 1
