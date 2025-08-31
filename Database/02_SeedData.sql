INSERT INTO Roles (RoleName, Description)
VALUES
  ( 'Admin', 'System administrator with full access'),
  ( 'Employee', 'Park employees who manage park operations'),
  ( 'Tourist', 'Tourist users who can explore parks and book safaris');


  
INSERT INTO GenderTypes (GenderName) VALUES 
('Male'),
('Female'),
('Other');

-- -----------------------------
-- 6️⃣ Insert Countries
-- -----------------------------
INSERT INTO Countries (CountryName, CountryCode) VALUES
('India', 'IN'),
('United States', 'US'),
('United Kingdom', 'UK');


INSERT INTO Profiles 
(UserId, FirstName, LastName, PhoneNumber, DateOfBirth, GenderId, Street, City, State, CountryId, ZipCode, ProfilePictureUrl, IsProfileComplete, CreatedAt, UpdatedAt)
VALUES
(1, 'Tourist', 'User', '9998887777', '1995-01-15', 1, '123 Safari Lane', 'Pune', 'Maharashtra', 1, '411001', NULL, 1, GETDATE(), GETDATE()),
(5, 'Admin', 'Patil', '8887776666', '1988-05-10', 1, '45 Admin Street', 'Mumbai', 'Maharashtra', 1, '400001', NULL, 1, GETDATE(), GETDATE()),
(6, 'Mike', 'Groom', '7776665555', '1990-09-20', 1, '78 Groom Avenue', 'Pune', 'Maharashtra', 1, '411007', NULL, 1, GETDATE(), GETDATE());