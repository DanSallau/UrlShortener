/****** Object:  Table [dbo].[Urls]    Script Date: 07/21/2015 04:20:16 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

IF db_id('UrlShortener') IS NULL 
    CREATE DATABASE UrlShortener
    
GO

CREATE TABLE [dbo].[Urls](
	[UrlId] [int] IDENTITY(1,1) NOT NULL,
	[OriginalUrl] [varchar](max) NOT NULL,
	[UrlCode] [varchar](30) NULL,
	[PostedDate] [datetime] NULL,
	[IpAddress] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[UrlId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


