CREATE TABLE [dbo].[Exam] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [Duration]      DATETIME       NULL,
    [TotalQuestion] INT            NULL,
    [Status]        VARCHAR (50)   NULL,
    [OpenTime]      DATETIME       NULL,
    [StageId]       INT            NULL,
    [Year]          INT            NULL,
    [Name]          NVARCHAR (255) NULL,
    [Code]          VARCHAR (50)   NULL,
    CONSTRAINT [PK_Exam] PRIMARY KEY CLUSTERED ([Id] ASC)
);

