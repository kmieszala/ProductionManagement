CREATE OR ALTER TRIGGER UpdateUser  
ON dbo.Users
AFTER INSERT   
AS   
IF ( UPDATE (STATUS) OR UPDATE (Email) )  
BEGIN  
	SET NOCOUNT ON;
	if UPDATE (STATUS)
	BEGIN
		insert into dbo.Log([LogCode], [Description], [Date])
		select 0, 'Change User Status from ' + i.Status + ' to ' + d.Status, GETDATE() 
			from inserted i
			join deleted d on i.Id = d.Id
	end
	else
	BEGIN
		insert into dbo.Log([LogCode], [Description], [Date])
		select 0, 'Change User Email from ' + i.Email + ' to ' + d.Email, GETDATE() 
			from inserted i
			join deleted d on i.Id = d.Id	
		end
END;  
GO