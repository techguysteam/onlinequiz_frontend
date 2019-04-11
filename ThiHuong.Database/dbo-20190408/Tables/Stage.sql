CREATE TABLE [dbo].[Stage] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [name]      NVARCHAR (255) NULL,
    [startDate] DATETIME       CONSTRAINT [DF_Stage_startDate] DEFAULT (getdate()) NULL,
    [endDate]   DATETIME       NULL,
    [status]    VARCHAR (50)   CONSTRAINT [DF_Stage_status] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_Stage] PRIMARY KEY CLUSTERED ([Id] ASC)
);



