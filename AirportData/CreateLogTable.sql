CREATE TABLE [dbo].[MyLogs](
    [ID] [int] IDENTITY(1,1) NOT NULL,
    [Date] [datetime] NOT NULL,    
    [Level] [varchar](20) NOT NULL,    
    [Message] [varchar](4000) NOT NULL
) ON [PRIMARY]