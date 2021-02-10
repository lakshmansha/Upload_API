CREATE PROCEDURE [dbo].[SP_FetchUploadFiles]	
AS
BEGIN

SELECT Id, GUID, FileName, FileSize, UploadDate
FROM Upload

END
