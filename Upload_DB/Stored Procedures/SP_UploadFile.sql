CREATE PROCEDURE [dbo].[SP_UploadFile]
	@GUID varchar(max),
	@FileName varchar(250),
	@FileSize decimal(18,2),
	@FileBinary varbinary(max),
	@FilePath varchar(max),
	@LocalFileName varchar(max),
	@ReturnId int OUTPUT
AS
BEGIN

Insert Into Upload ([GUID], [FileName], [FileSize], [FileBinary], [FilePath], [UploadDate]) 
		Values (@GUID, @FileName, @FileSize, @FileBinary, @FilePath, GETDATE())

SELECT @ReturnId = SCOPE_IDENTITY()

END