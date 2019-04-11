CREATE TABLE [dbo].[AccountInStage] (
    [Id]         INT      IDENTITY (1, 1) NOT NULL,
    [AccountId]  INT      NOT NULL,
    [ExamId]     INT      NULL,
    [Rank]       INT      NULL,
    [StartTime]  DATETIME NULL,
    [FinishTime] DATETIME NULL,
    [Point]      REAL     NULL,
    CONSTRAINT [PK_AccountInStage_1] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AccountInStage_Account] FOREIGN KEY ([AccountId]) REFERENCES [dbo].[Account] ([Id]),
    CONSTRAINT [FK_AccountInStage_Exam] FOREIGN KEY ([ExamId]) REFERENCES [dbo].[Exam] ([Id]),
    CONSTRAINT [IX_AccountInStage] UNIQUE NONCLUSTERED ([AccountId] ASC, [ExamId] ASC)
);



