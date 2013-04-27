/****** Object:  Table [dbo].[Languages]    Script Date: 12/27/2010 16:56:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Languages]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Languages](
	[ID] [int] NOT NULL,
	[CultureCode] [char](2) NOT NULL,
	[Default] [bit] NOT NULL,
 CONSTRAINT [PK_Languages] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/****** Object:  Table [dbo].[Resources]    Script Date: 12/27/2010 16:58:28 ******/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Resources]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Resources](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[LanguageID] [int] NOT NULL,
	[Route] [nvarchar](50) NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
	[Value] [nvarchar](4000) NOT NULL,
	[ResourceType] [int] NOT NULL,
	[CreationDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

SET ANSI_PADDING OFF
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Resources_Languages]') AND parent_object_id = OBJECT_ID(N'[dbo].[Resources]'))
ALTER TABLE [dbo].[Resources]  WITH CHECK ADD  CONSTRAINT [FK_Resources_Languages] FOREIGN KEY([LanguageID])
REFERENCES [dbo].[Languages] ([ID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Resources_Languages]') AND parent_object_id = OBJECT_ID(N'[dbo].[Resources]'))
ALTER TABLE [dbo].[Resources] CHECK CONSTRAINT [FK_Resources_Languages]
GO

ALTER TABLE [dbo].[Resources] ADD  CONSTRAINT [DF_Resources_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO

/****** Object:  Table [dbo].[Resources]    Script Date: 01/31/2012 15:56:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

