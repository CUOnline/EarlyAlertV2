IF OBJECT_ID(N'__EFMigrationsHistory') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [AspNetRoles] (
    [Id] nvarchar(450) NOT NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [AspNetUsers] (
    [Id] nvarchar(450) NOT NULL,
    [AccessFailedCount] int NOT NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [Email] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [FirstName] nvarchar(max) NULL,
    [LastName] nvarchar(max) NULL,
    [LockoutEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [PasswordHash] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [UserName] nvarchar(256) NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Assignments] (
    [Id] int NOT NULL IDENTITY,
    [DateCreated] datetime2 NOT NULL,
    [LastUpdated] datetime2 NULL,
    CONSTRAINT [PK_Assignments] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Classes] (
    [Id] int NOT NULL IDENTITY,
    [DateCreated] datetime2 NOT NULL,
    [LastUpdated] datetime2 NULL,
    CONSTRAINT [PK_Classes] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Grades] (
    [Id] int NOT NULL IDENTITY,
    [DateCreated] datetime2 NOT NULL,
    [LastUpdated] datetime2 NULL,
    CONSTRAINT [PK_Grades] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Students] (
    [Id] int NOT NULL IDENTITY,
    [DateCreated] datetime2 NOT NULL,
    [LastUpdated] datetime2 NULL,
    CONSTRAINT [PK_Students] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [AspNetRoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    [RoleId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [AspNetUserClaims] (
    [Id] int NOT NULL IDENTITY,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [AspNetUserLogins] (
    [LoginProvider] nvarchar(450) NOT NULL,
    [ProviderKey] nvarchar(450) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [AspNetUserRoles] (
    [UserId] nvarchar(450) NOT NULL,
    [RoleId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [AspNetUserTokens] (
    [UserId] nvarchar(450) NOT NULL,
    [LoginProvider] nvarchar(450) NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

GO

CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);

GO

CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;

GO

CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);

GO

CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);

GO

CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);

GO

CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);

GO

CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20180301164756_initial', N'2.0.1-rtm-125');

GO

CREATE TABLE [Reports] (
    [Id] int NOT NULL IDENTITY,
    [DateCreated] datetime2 NOT NULL,
    [LastUpdated] datetime2 NULL,
    [ReportType] int NOT NULL,
    [Status] nvarchar(max) NULL,
    CONSTRAINT [PK_Reports] PRIMARY KEY ([Id])
);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20180312184942_AddReport', N'2.0.1-rtm-125');

GO

ALTER TABLE [Reports] ADD [Name] nvarchar(max) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20180312185113_AddNameToReport', N'2.0.1-rtm-125');

GO

ALTER TABLE [Reports] ADD [ReportData] nvarchar(max) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20180312192412_AddReportDataToReport', N'2.0.1-rtm-125');

GO

DROP TABLE [Classes];

GO

ALTER TABLE [Students] ADD [CanvasId] int NOT NULL DEFAULT 0;

GO

ALTER TABLE [Students] ADD [Name] nvarchar(max) NULL;

GO

ALTER TABLE [Students] ADD [SISUserId] nvarchar(max) NULL;

GO

ALTER TABLE [Assignments] ADD [CanvasId] int NOT NULL DEFAULT 0;

GO

ALTER TABLE [Assignments] ADD [CourseId] int NOT NULL DEFAULT 0;

GO

ALTER TABLE [Assignments] ADD [Description] nvarchar(max) NULL;

GO

ALTER TABLE [Assignments] ADD [DueAt] datetime2 NULL;

GO

ALTER TABLE [Assignments] ADD [LockAt] datetime2 NULL;

GO

ALTER TABLE [Assignments] ADD [Name] nvarchar(max) NULL;

GO

ALTER TABLE [Assignments] ADD [PointsPossible] float NOT NULL DEFAULT 0E0;

GO

ALTER TABLE [Assignments] ADD [UnlockAt] datetime2 NULL;

GO

CREATE TABLE [Courses] (
    [Id] int NOT NULL IDENTITY,
    [CanvasId] int NOT NULL,
    [CourseCode] nvarchar(max) NULL,
    [DateCreated] datetime2 NOT NULL,
    [EndAt] datetime2 NULL,
    [EnrollmentTermId] int NOT NULL,
    [LastUpdated] datetime2 NULL,
    [Name] nvarchar(max) NULL,
    [StartAt] datetime2 NULL,
    CONSTRAINT [PK_Courses] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [StudentAssignmentSubmissions] (
    [Id] int NOT NULL IDENTITY,
    [AssignmentId] int NOT NULL,
    [CanvasAssignmentId] int NOT NULL,
    [CanvasId] int NOT NULL,
    [CanvasUserId] int NOT NULL,
    [DateCreated] datetime2 NOT NULL,
    [Excused] bit NOT NULL,
    [LastUpdated] datetime2 NULL,
    [Late] bit NOT NULL,
    [Missing] bit NOT NULL,
    [Score] float NULL,
    [StudentId] int NOT NULL,
    [WorkflowState] nvarchar(max) NULL,
    CONSTRAINT [PK_StudentAssignmentSubmissions] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_StudentAssignmentSubmissions_Assignments_AssignmentId] FOREIGN KEY ([AssignmentId]) REFERENCES [Assignments] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_StudentAssignmentSubmissions_Students_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [Students] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [StudentCourse] (
    [StudentId] int NOT NULL,
    [CourseId] int NOT NULL,
    CONSTRAINT [PK_StudentCourse] PRIMARY KEY ([StudentId], [CourseId]),
    CONSTRAINT [FK_StudentCourse_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_StudentCourse_Students_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [Students] ([Id]) ON DELETE CASCADE
);

GO

CREATE INDEX [IX_Assignments_CourseId] ON [Assignments] ([CourseId]);

GO

CREATE INDEX [IX_StudentAssignmentSubmissions_AssignmentId] ON [StudentAssignmentSubmissions] ([AssignmentId]);

GO

CREATE INDEX [IX_StudentAssignmentSubmissions_StudentId] ON [StudentAssignmentSubmissions] ([StudentId]);

GO

CREATE INDEX [IX_StudentCourse_CourseId] ON [StudentCourse] ([CourseId]);

GO

ALTER TABLE [Assignments] ADD CONSTRAINT [FK_Assignments_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([Id]) ON DELETE CASCADE;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20180327214850_UpdateModels', N'2.0.1-rtm-125');

GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'Assignments') AND [c].[name] = N'PointsPossible');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Assignments] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Assignments] ALTER COLUMN [PointsPossible] float NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20180327220828_UpdateAssignmentPointsPossibleNullable', N'2.0.1-rtm-125');

GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'StudentAssignmentSubmissions') AND [c].[name] = N'Excused');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [StudentAssignmentSubmissions] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [StudentAssignmentSubmissions] ALTER COLUMN [Excused] bit NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20180327223339_StudentSubmissionExcusedIsNullable', N'2.0.1-rtm-125');

GO

ALTER TABLE [Assignments] ADD [AssignmentGroupId] int NOT NULL DEFAULT 0;

GO

CREATE TABLE [AssignmentGroups] (
    [Id] int NOT NULL IDENTITY,
    [CanvasId] int NOT NULL,
    [CourseId] int NOT NULL,
    [DateCreated] datetime2 NOT NULL,
    [GroupWeight] float NOT NULL,
    [LastUpdated] datetime2 NULL,
    [Name] nvarchar(max) NULL,
    CONSTRAINT [PK_AssignmentGroups] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AssignmentGroups_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([Id]) ON DELETE CASCADE
);

GO

CREATE INDEX [IX_Assignments_AssignmentGroupId] ON [Assignments] ([AssignmentGroupId]);

GO

CREATE INDEX [IX_AssignmentGroups_CourseId] ON [AssignmentGroups] ([CourseId]);

GO

ALTER TABLE [Assignments] ADD CONSTRAINT [FK_Assignments_AssignmentGroups_AssignmentGroupId] FOREIGN KEY ([AssignmentGroupId]) REFERENCES [AssignmentGroups] ([Id]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20180329163115_AddAssignmentGroup', N'2.0.1-rtm-125');

GO

ALTER TABLE [Grades] ADD [CourseId] int NOT NULL DEFAULT 0;

GO

ALTER TABLE [Grades] ADD [StudentId] int NOT NULL DEFAULT 0;

GO

ALTER TABLE [Grades] ADD [Value] float NOT NULL DEFAULT 0E0;

GO

CREATE INDEX [IX_Grades_CourseId] ON [Grades] ([CourseId]);

GO

CREATE INDEX [IX_Grades_StudentId] ON [Grades] ([StudentId]);

GO

ALTER TABLE [Grades] ADD CONSTRAINT [FK_Grades_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([Id]) ON DELETE CASCADE;

GO

ALTER TABLE [Grades] ADD CONSTRAINT [FK_Grades_Students_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [Students] ([Id]) ON DELETE CASCADE;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20180406231154_UpdateGrades', N'2.0.1-rtm-125');

GO

CREATE TABLE [ReportSettings] (
    [Id] int NOT NULL IDENTITY,
    [ActivityWeight] float NOT NULL,
    [CommunicationWeight] float NOT NULL,
    [DateCreated] datetime2 NOT NULL,
    [GradeWeight] float NOT NULL,
    [LastUpdated] datetime2 NULL,
    [LateAssignmentsWeight] float NOT NULL,
    [MissedAssignmentsWeight] float NOT NULL,
    [NumberOfActiveCoursesWeight] float NOT NULL,
    [PageViewsWeight] float NOT NULL,
    [ParticipationWeight] float NOT NULL,
    CONSTRAINT [PK_ReportSettings] PRIMARY KEY ([Id])
);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20180416201620_AddReportSettings', N'2.0.1-rtm-125');

GO

ALTER TABLE [Students] ADD [LatestActivity] datetime2 NULL;

GO

ALTER TABLE [ReportSettings] ADD [ActivityTimeMax] int NOT NULL DEFAULT 0;

GO

ALTER TABLE [ReportSettings] ADD [ActivityTimeMin] int NOT NULL DEFAULT 0;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20180521204950_AddLatestActivityToStudents', N'2.0.1-rtm-125');

GO

