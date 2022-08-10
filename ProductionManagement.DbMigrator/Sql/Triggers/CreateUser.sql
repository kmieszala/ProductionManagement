CREATE OR ALTER TRIGGER CreateUser  
ON dbo.Users
AFTER INSERT   
AS   		
BEGIN
	insert into dbo.Log([LogCode], [Description], [Date])
	select 0, 'Add new User: ' + i.FirstName + ' ' + i.LastName, GETDATE() from inserted i
END