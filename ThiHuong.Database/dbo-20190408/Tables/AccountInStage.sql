CREATE TABLE [dbo].[AccountInStage] (
    [Id]         INT      IDENTITY (1, 1) NOT NULL,
    [StageId]    INT      NOT NULL,
    [AccountId]  INT      NOT NULL,
    [StartTime]  DATETIME NULL,
    [FinishTime] DATETIME NULL,
    [Point]      REAL     NULL,
    [isTalent]   BIT      NULL,
    [ExamId]     INT      NULL,
    CONSTRAINT [PK_AccountInStage_1] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AccountInStage_Account] FOREIGN KEY ([AccountId]) REFERENCES [dbo].[Account] ([Id]),
    CONSTRAINT [FK_AccountInStage_Stage] FOREIGN KEY ([StageId]) REFERENCES [dbo].[Stage] ([Id])
);

