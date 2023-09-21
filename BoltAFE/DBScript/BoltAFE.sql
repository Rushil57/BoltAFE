USE [master]
GO
/****** Object:  Database [BoltAFE]    Script Date: 21-09-2023 10:39:24 AM ******/
CREATE DATABASE [BoltAFE]
 GO

USE [BoltAFE]
GO
/****** Object:  Table [dbo].[Afe_aprvl_hist_dtl]    Script Date: 21-09-2023 10:39:24 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Afe_aprvl_hist_dtl](
	[Aprvl_hist_dtl_id] [int] IDENTITY(1,1) NOT NULL,
	[Afe_hdr_id] [int] NULL,
	[Approver_user_id] [int] NULL,
	[Approved_date] [datetime] NULL,
 CONSTRAINT [PK_Afe_aprvl_hist_dtl] PRIMARY KEY CLUSTERED 
(
	[Aprvl_hist_dtl_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Afe_category]    Script Date: 21-09-2023 10:39:24 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Afe_category](
	[Afe_category_id] [int] IDENTITY(1,1) NOT NULL,
	[Category] [nvarchar](1000) NULL,
 CONSTRAINT [PK_Afe_category] PRIMARY KEY CLUSTERED 
(
	[Afe_category_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Afe_comments]    Script Date: 21-09-2023 10:39:24 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Afe_comments](
	[Afe_comments_id] [int] IDENTITY(1,1) NOT NULL,
	[Afe_hdr_id] [int] NULL,
	[User_id] [int] NULL,
	[Timestamp] [datetime] NULL,
	[Message] [nvarchar](max) NULL,
 CONSTRAINT [PK_Afe_comments] PRIMARY KEY CLUSTERED 
(
	[Afe_comments_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Afe_docs]    Script Date: 21-09-2023 10:39:24 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Afe_docs](
	[Afe_doc_id] [int] IDENTITY(1,1) NOT NULL,
	[Afe_hdr_id] [int] NULL,
	[User_id] [int] NULL,
	[Doc_path] [nvarchar](max) NULL,
	[Doc_description] [nvarchar](max) NULL,
	[Doc_order] [int] NULL,
	[Upload_timestamp] [datetime] NULL,
 CONSTRAINT [PK_Afe_docs] PRIMARY KEY CLUSTERED 
(
	[Afe_doc_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Afe_econ_dtl]    Script Date: 21-09-2023 10:39:24 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Afe_econ_dtl](
	[Afe_econ_dtl_id] [int] IDENTITY(1,1) NOT NULL,
	[Afe_hdr_id] [int] NULL,
	[Description] [nvarchar](max) NULL,
	[Gross_afe] [decimal](18, 2) NULL,
	[Wi] [decimal](18, 2) NULL,
	[Nri] [decimal](18, 2) NULL,
	[Roy] [decimal](18, 2) NULL,
	[Net_afe] [decimal](18, 2) NULL,
	[Oil] [decimal](18, 2) NULL,
	[Gas] [decimal](18, 2) NULL,
	[Ngl] [decimal](18, 2) NULL,
	[Boe] [decimal](18, 2) NULL,
	[Und_po] [decimal](18, 2) NULL,
	[Pv10] [decimal](18, 2) NULL,
	[F_and_d] [decimal](18, 2) NULL,
	[Ror] [decimal](18, 2) NULL,
	[Mroi] [decimal](18, 2) NULL,
	[Changed_by_user_id] [int] NULL,
	[Changed_date] [datetime] NULL,
 CONSTRAINT [PK_Afe_econ_dtl] PRIMARY KEY CLUSTERED 
(
	[Afe_econ_dtl_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Afe_hdr]    Script Date: 21-09-2023 10:39:24 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Afe_hdr](
	[Afe_hdr_id] [int] IDENTITY(1,1) NOT NULL,
	[Afe_name] [nvarchar](500) NULL,
	[Afe_type_id] [int] NULL,
	[Afe_category_id] [int] NULL,
	[Afe_num] [nvarchar](255) NULL,
	[Created_date] [datetime] NULL,
	[Created_By] [int] NULL,
	[Inbox_user_id] [int] NULL,
 CONSTRAINT [PK_Afe_hdr] PRIMARY KEY CLUSTERED 
(
	[Afe_hdr_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UK_Table_1] UNIQUE NONCLUSTERED 
(
	[Afe_num] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Afe_type]    Script Date: 21-09-2023 10:39:24 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Afe_type](
	[Afe_type_id] [int] IDENTITY(1,1) NOT NULL,
	[Type] [nvarchar](500) NULL,
	[Include_gross_afe] [bit] NULL,
	[Include_wi] [bit] NULL,
	[Include_nri] [bit] NULL,
	[Include_roy] [bit] NULL,
	[Include_net_afe] [bit] NULL,
	[Include_oil] [bit] NULL,
	[Include_gas] [bit] NULL,
	[Include_ngl] [bit] NULL,
	[Include_boe] [bit] NULL,
	[Include_po] [bit] NULL,
	[Include_pv10] [bit] NULL,
	[Include_f_and_d] [bit] NULL,
	[Include_ror] [bit] NULL,
	[Include_mroi] [bit] NULL,
	[Afe_num_code] [nvarchar](50) NULL,
 CONSTRAINT [PK_Afe_type] PRIMARY KEY CLUSTERED 
(
	[Afe_type_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TransactionLog]    Script Date: 21-09-2023 10:39:24 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TransactionLog](
	[TableName] [varchar](50) NULL,
	[LogDateTime] [datetime] NULL,
	[Note] [varchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserDetail]    Script Date: 21-09-2023 10:39:24 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserDetail](
	[User_ID] [int] IDENTITY(1,1) NOT NULL,
	[User_email] [nvarchar](255) NOT NULL,
	[Password] [nvarchar](500) NOT NULL,
	[Reset_password] [bit] NOT NULL,
	[Approver_amount] [decimal](18, 2) NULL,
 CONSTRAINT [PK_UserDetail] PRIMARY KEY CLUSTERED 
(
	[User_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Afe_aprvl_hist_dtl] ADD  CONSTRAINT [DF_Afe_aprvl_hist_dtl_Approved_date]  DEFAULT (getdate()) FOR [Approved_date]
GO
ALTER TABLE [dbo].[Afe_comments] ADD  CONSTRAINT [DF_Afe_comments_Timestamp]  DEFAULT (getdate()) FOR [Timestamp]
GO
ALTER TABLE [dbo].[Afe_docs] ADD  CONSTRAINT [DF_Afe_docs_Upload_timestamp]  DEFAULT (getdate()) FOR [Upload_timestamp]
GO
ALTER TABLE [dbo].[Afe_econ_dtl] ADD  CONSTRAINT [DF_Afe_econ_dtl_Changed_date]  DEFAULT (getdate()) FOR [Changed_date]
GO
ALTER TABLE [dbo].[Afe_hdr] ADD  CONSTRAINT [DF_Afe_hdr_Created_date]  DEFAULT (getdate()) FOR [Created_date]
GO
ALTER TABLE [dbo].[Afe_type] ADD  CONSTRAINT [DF_Afe_type_Include_gross_afe]  DEFAULT ((0)) FOR [Include_gross_afe]
GO
ALTER TABLE [dbo].[Afe_type] ADD  CONSTRAINT [DF_Afe_type_Include_wi]  DEFAULT ((0)) FOR [Include_wi]
GO
ALTER TABLE [dbo].[Afe_type] ADD  CONSTRAINT [DF_Afe_type_Include_nri]  DEFAULT ((0)) FOR [Include_nri]
GO
ALTER TABLE [dbo].[Afe_type] ADD  CONSTRAINT [DF_Afe_type_Include_roy]  DEFAULT ((0)) FOR [Include_roy]
GO
ALTER TABLE [dbo].[Afe_type] ADD  CONSTRAINT [DF_Afe_type_Include_net_afe]  DEFAULT ((0)) FOR [Include_net_afe]
GO
ALTER TABLE [dbo].[Afe_type] ADD  CONSTRAINT [DF_Afe_type_Include_oil]  DEFAULT ((0)) FOR [Include_oil]
GO
ALTER TABLE [dbo].[Afe_type] ADD  CONSTRAINT [DF_Afe_type_Include_gas]  DEFAULT ((0)) FOR [Include_gas]
GO
ALTER TABLE [dbo].[Afe_type] ADD  CONSTRAINT [DF_Afe_type_Include_ngl]  DEFAULT ((0)) FOR [Include_ngl]
GO
ALTER TABLE [dbo].[Afe_type] ADD  CONSTRAINT [DF_Afe_type_Include_boe]  DEFAULT ((0)) FOR [Include_boe]
GO
ALTER TABLE [dbo].[Afe_type] ADD  CONSTRAINT [DF_Afe_type_Include_po]  DEFAULT ((0)) FOR [Include_po]
GO
ALTER TABLE [dbo].[Afe_type] ADD  CONSTRAINT [DF_Afe_type_Include_pv10]  DEFAULT ((0)) FOR [Include_pv10]
GO
ALTER TABLE [dbo].[Afe_type] ADD  CONSTRAINT [DF_Afe_type_Include_f_and_d]  DEFAULT ((0)) FOR [Include_f_and_d]
GO
ALTER TABLE [dbo].[Afe_type] ADD  CONSTRAINT [DF_Afe_type_Include_ror]  DEFAULT ((0)) FOR [Include_ror]
GO
ALTER TABLE [dbo].[Afe_type] ADD  CONSTRAINT [DF_Afe_type_Include_mroi]  DEFAULT ((0)) FOR [Include_mroi]
GO
ALTER TABLE [dbo].[UserDetail] ADD  CONSTRAINT [DF_UserDetail_Reset_password]  DEFAULT ((1)) FOR [Reset_password]
GO
ALTER TABLE [dbo].[UserDetail] ADD  CONSTRAINT [DF_UserDetail_Approver_amount]  DEFAULT ((0)) FOR [Approver_amount]
GO
ALTER TABLE [dbo].[Afe_aprvl_hist_dtl]  WITH CHECK ADD  CONSTRAINT [FK_Afe_aprvl_hist_dtl_Afe_hdr] FOREIGN KEY([Afe_hdr_id])
REFERENCES [dbo].[Afe_hdr] ([Afe_hdr_id])
GO
ALTER TABLE [dbo].[Afe_aprvl_hist_dtl] CHECK CONSTRAINT [FK_Afe_aprvl_hist_dtl_Afe_hdr]
GO
ALTER TABLE [dbo].[Afe_aprvl_hist_dtl]  WITH CHECK ADD  CONSTRAINT [FK_Afe_aprvl_hist_dtl_UserDetail] FOREIGN KEY([Approver_user_id])
REFERENCES [dbo].[UserDetail] ([User_ID])
GO
ALTER TABLE [dbo].[Afe_aprvl_hist_dtl] CHECK CONSTRAINT [FK_Afe_aprvl_hist_dtl_UserDetail]
GO
ALTER TABLE [dbo].[Afe_comments]  WITH CHECK ADD  CONSTRAINT [FK_Afe_comments_UserDetail] FOREIGN KEY([User_id])
REFERENCES [dbo].[UserDetail] ([User_ID])
GO
ALTER TABLE [dbo].[Afe_comments] CHECK CONSTRAINT [FK_Afe_comments_UserDetail]
GO
ALTER TABLE [dbo].[Afe_docs]  WITH CHECK ADD  CONSTRAINT [FK_Afe_docs_UserDetail] FOREIGN KEY([User_id])
REFERENCES [dbo].[UserDetail] ([User_ID])
GO
ALTER TABLE [dbo].[Afe_docs] CHECK CONSTRAINT [FK_Afe_docs_UserDetail]
GO
ALTER TABLE [dbo].[Afe_econ_dtl]  WITH CHECK ADD  CONSTRAINT [FK_Afe_econ_dtl_Afe_hdr] FOREIGN KEY([Afe_hdr_id])
REFERENCES [dbo].[Afe_hdr] ([Afe_hdr_id])
GO
ALTER TABLE [dbo].[Afe_econ_dtl] CHECK CONSTRAINT [FK_Afe_econ_dtl_Afe_hdr]
GO
ALTER TABLE [dbo].[Afe_econ_dtl]  WITH CHECK ADD  CONSTRAINT [FK_Afe_econ_dtl_UserDetail] FOREIGN KEY([Changed_by_user_id])
REFERENCES [dbo].[UserDetail] ([User_ID])
GO
ALTER TABLE [dbo].[Afe_econ_dtl] CHECK CONSTRAINT [FK_Afe_econ_dtl_UserDetail]
GO
ALTER TABLE [dbo].[Afe_hdr]  WITH CHECK ADD  CONSTRAINT [FK_Afe_hdr_Afe_category] FOREIGN KEY([Afe_category_id])
REFERENCES [dbo].[Afe_category] ([Afe_category_id])
GO
ALTER TABLE [dbo].[Afe_hdr] CHECK CONSTRAINT [FK_Afe_hdr_Afe_category]
GO
ALTER TABLE [dbo].[Afe_hdr]  WITH CHECK ADD  CONSTRAINT [FK_Afe_hdr_Afe_type] FOREIGN KEY([Afe_type_id])
REFERENCES [dbo].[Afe_type] ([Afe_type_id])
GO
ALTER TABLE [dbo].[Afe_hdr] CHECK CONSTRAINT [FK_Afe_hdr_Afe_type]
GO
USE [master]
GO
ALTER DATABASE [BoltAFE] SET  READ_WRITE 
GO
