CREATE PROCEDURE dbo.EditEmail
	@Email NVARCHAR(MAX),
	@Id int NOT NULL
AS
	UPDATE Users SET Email = @Email WHERE Id = @Id
GO