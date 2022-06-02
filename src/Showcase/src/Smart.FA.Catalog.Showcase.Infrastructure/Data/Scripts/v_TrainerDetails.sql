CREATE VIEW [Cfa].[v_TrainerDetails]
	AS
SELECT Cfa.Trainer.Id
      ,Cfa.Trainer.FirstName
      ,Cfa.Trainer.LastName
      ,Cfa.Trainer.Biography
      ,Cfa.Trainer.Title
      ,Cfa.Trainer.Email
      ,Cfa.Trainer.ProfileImagePath
      ,Cfa.TrainerSocialNetwork.SocialNetworkId
      ,Cfa.TrainerSocialNetwork.UrlToProfile
FROM [Cfa].Trainer
LEFT OUTER JOIN Cfa.TrainerSocialNetwork ON Cfa.Trainer.Id = Cfa.TrainerSocialNetwork.TrainerId
