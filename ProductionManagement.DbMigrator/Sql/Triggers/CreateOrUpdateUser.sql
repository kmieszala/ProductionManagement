CREATE OR ALTER TRIGGER CreateUser  
ON dbo.Users
AFTER INSERT   
AS   		
BEGIN
	select * from inserted i
		insert into dbo.Log(LogCode, [Description])
		select 0, 'Add new User: ' + i.FirstName + ' ' + i.LastName from inserted i
END