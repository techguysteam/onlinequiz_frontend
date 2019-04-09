CREATE TABLE [dbo].[Question] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [Type]     VARCHAR (50)   NULL,
    [Path]     VARCHAR (255)  NULL,
    [Content]  NVARCHAR (MAX) NULL,
    [A]        NVARCHAR (MAX) NULL,
    [B]        NVARCHAR (MAX) NULL,
    [C]        NVARCHAR (MAX) NULL,
    [D]        NVARCHAR (MAX) NULL,
    [Answer]   NVARCHAR (50)  NULL,
    [Point]    REAL           NULL,
    [isActive] BIT            NULL,
    CONSTRAINT [PK_Table_1] PRIMARY KEY CLUSTERED ([Id] ASC)
);

