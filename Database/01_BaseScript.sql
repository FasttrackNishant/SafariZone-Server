CREATE DATABASE SafariZone

CREATE TABLE Roles (
    RoleId INT IDENTITY PRIMARY KEY,
    RoleName NVARCHAR(50) NOT NULL UNIQUE, -- Admin, Employee, Tourist
    Description NVARCHAR(255)
);

CREATE TABLE Users (
  UserId INT PRIMARY KEY IDENTITY(1,1),
  Email VARCHAR(100) NOT NULL UNIQUE,
  PasswordHash VARCHAR(255) NOT NULL,
  RoleId INT NOT NULL FOREIGN KEY REFERENCES Roles(RoleId),
  IsActive BIT DEFAULT 1,
  LastLogin DATETIME,
  CreatedAt DATETIME DEFAULT GETDATE(),
  UpdatedAt DATETIME DEFAULT GETDATE()
);


ALTER TABLE Users
alter column AadId NVARCHAR(256) NULL


CREATE TABLE GenderTypes (
    GenderId INT PRIMARY KEY IDENTITY(1,1),
    GenderName VARCHAR(50) UNIQUE NOT NULL
);

CREATE TABLE Countries (
    CountryId INT PRIMARY KEY IDENTITY(1,1),
    CountryName VARCHAR(100) NOT NULL,
    CountryCode VARCHAR(10) NOT NULL
);

CREATE TABLE Profiles (
    ProfileId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT UNIQUE NOT NULL,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    PhoneNumber NVARCHAR(20),
    DateOfBirth DATE,
    GenderId INT,
    Street NVARCHAR(255),
    City NVARCHAR(100),
    State NVARCHAR(100),
    CountryId INT,
    ZipCode NVARCHAR(20),
    ProfilePictureUrl NVARCHAR(255) NULL,
    IsProfileComplete BIT DEFAULT 0,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Profiles_Users FOREIGN KEY (UserId) REFERENCES Users(UserId),
    CONSTRAINT FK_Profiles_Gender FOREIGN KEY (GenderId) REFERENCES GenderTypes(GenderId),
    CONSTRAINT FK_Profiles_Country FOREIGN KEY (CountryId) REFERENCES Countries(CountryId)
);
