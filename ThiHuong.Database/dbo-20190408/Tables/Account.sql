CREATE TABLE [dbo].[Account] (
    [Id]           INT             IDENTITY (1, 1) NOT NULL,
    [Username]     VARCHAR (50)    NOT NULL,
    [PasswordHash] VARBINARY (MAX) NOT NULL,
    [PasswordSalt] VARBINARY (MAX) NOT NULL,
    [Fullname]     NVARCHAR (255)  NULL,
    [Phone]        VARCHAR (255)   NULL,
    [Mail]         VARCHAR (255)   NULL,
    [Address]      NVARCHAR (255)  NULL,
    [Birthdate]    DATETIME        NULL,
    [RoleId]       INT             NULL,
    [Deleted]      BIT             NULL,
    CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Account_Role] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Role] ([Id]),
    CONSTRAINT [IX_Account] UNIQUE NONCLUSTERED ([Username] ASC)
);



