CREATE TABLE [dbo].[ResultDetail] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [AccountId] INT           NOT NULL,
    [Choice]    NVARCHAR (50) NULL,
    [IsCorrect] BIT           NULL,
    [Point]     REAL          NULL,
    [Answer]    NVARCHAR (50) NULL,
    CONSTRAINT [PK_ResultDetail_1] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ResultDetail_AccountInStage] FOREIGN KEY ([AccountId]) REFERENCES [dbo].[AccountInStage] ([Id])
);

