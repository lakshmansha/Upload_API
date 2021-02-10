CREATE TABLE [dbo].[Upload]
(
    [Id] int NOT NULL PRIMARY KEY IDENTITY,
	[GUID] VARCHAR(MAX) NOT NULL, 
    [FileName] VARCHAR(250) NULL, 
    [FileSize] DECIMAL(18, 2) NULL, 
    [UploadDate] DATETIME NULL, 
    [FileBinary] VARBINARY(MAX) NULL, 
    [FilePath] VARCHAR(MAX) NULL, 
    [LocalFileName] VARCHAR(MAX) NULL
)
