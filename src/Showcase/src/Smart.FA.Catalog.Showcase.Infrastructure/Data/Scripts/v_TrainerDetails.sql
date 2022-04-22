CREATE VIEW [dbo].[v_TrainerDetails]
	AS
SELECT dbo.Trainer.Id
      ,dbo.Trainer.FirstName
      ,dbo.Trainer.LastName
      ,dbo.Trainer.Biography
      ,dbo.Trainer.Title
      ,dbo.TrainerPersonalNetwork.SocialNetwork
      ,dbo.TrainerPersonalNetwork.UrlToProfile
FROM dbo.Trainer
LEFT OUTER JOIN dbo.TrainerPersonalNetwork ON dbo.Trainer.Id = dbo.TrainerPersonalNetwork.TrainerId
