CREATE VIEW [Cfa].[v_TrainerDetails]
AS
SELECT trainer.Id
    ,trainer.FirstName
    ,trainer.LastName
    ,trainer.Biography
    ,trainer.Title
    ,trainer.Email
    ,trainer.ProfileImagePath
    ,socialNetwork.SocialNetworkId
    ,socialNetwork.UrlToProfile
FROM [Cfa].Trainer trainer
    LEFT JOIN Cfa.TrainerSocialNetwork socialNetwork
    ON trainer.Id = socialNetwork.TrainerId
    INNER JOIN Cfa.TrainerAssignment assignment
    ON assignment.TrainerId = trainer.Id
    INNER JOIn Cfa.Training training
    ON training.Id = assignment.TrainingId
    INNER JOIN Cfa.TrainingStatusType trainingStatusType
    ON trainingStatusType.Id = training.TrainingStatusTypeId
WHERE TrainingStatusType.Name = 'Published'
