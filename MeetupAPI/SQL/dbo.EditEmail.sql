CREATE PROCEDURE dbo.EditEmail
	@Email NVARCHAR(MAX),
	@Id int
AS
	UPDATE Users SET Email = @Email WHERE Id = @Id
GO