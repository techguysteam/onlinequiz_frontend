CREATE TABLE [dbo].[ExamDetail] (
    [Id]         INT IDENTITY (1, 1) NOT NULL,
    [ExamId]     INT NOT NULL,
    [QuestionId] INT NOT NULL,
    CONSTRAINT [PK_ExamDetail_1] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ExamDetail_Exam] FOREIGN KEY ([ExamId]) REFERENCES [dbo].[Exam] ([Id]),
    CONSTRAINT [FK_ExamDetail_Question] FOREIGN KEY ([QuestionId]) REFERENCES [dbo].[Question] ([Id])
);

