CREATE VIEW [Cfa].[v_TrainerList]
	AS
SELECT Cfa.Trainer.Id,
       Cfa.Trainer.FirstName,
       Cfa.Trainer.LastName,
       Cfa.Trainer.Title,
       Cfa.Trainer.ProfileImagePath,
       Cfa.Training.TrainingStatusTypeId

FROM Cfa.Trainer
INNER JOIN Cfa.TrainerAssignment ON Cfa.Trainer.Id = Cfa.TrainerAssignment.TrainerId
INNER JOIN Cfa.Training ON Cfa.TrainerAssignment.TrainingId = Cfa.Training.Id
WHERE Cfa.Training.TrainingStatusTypeId = 3
