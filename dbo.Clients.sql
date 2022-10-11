CREATE TABLE [dbo].[Clients] (
    [Id]       INT        IDENTITY (1, 1) NOT NULL,
    [Login]    VARCHAR(50) NULL,
    [Haslo]    VARCHAR(50) NULL,
    [Imie]     VARCHAR(50) NULL,
    [Nazwisko] VARCHAR(50) NULL,
    [Email]    VARCHAR(50) NULL,
    [Active]   BIT        NULL,
    [Blocked]  BIT        NULL,
    CONSTRAINT [PK_Clients] PRIMARY KEY CLUSTERED ([Id] ASC)
);

