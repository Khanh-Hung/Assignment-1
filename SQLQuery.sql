CREATE DATABASE SE1725_PRN232_SE170547_G5_BloodDonationSystem;
GO

USE SE1725_PRN232_SE170547_G5_BloodDonationSystem;
GO


-- Tạo bảng tài khoản người dùng
CREATE TABLE [dbo].[System.UserAccount] (
    [UserAccountID] INT IDENTITY(1,1) NOT NULL,
    [UserName] NVARCHAR(50) NOT NULL,
    [Password] NVARCHAR(100) NOT NULL,
    [FullName] NVARCHAR(100) NOT NULL,
    [Email] NVARCHAR(150) NOT NULL,
    [Phone] NVARCHAR(50) NOT NULL,
    [EmployeeCode] NVARCHAR(50) NOT NULL,
    [RoleId] INT NOT NULL,
    [RequestCode] NVARCHAR(50) NULL,
    [CreatedDate] DATETIME NULL,
    [ApplicationCode] NVARCHAR(50) NULL,
    [CreatedBy] NVARCHAR(50) NULL,
    [ModifiedDate] DATETIME NULL,
    [ModifiedBy] NVARCHAR(50) NULL,
    [IsActive] BIT NOT NULL,
    CONSTRAINT [PK_System.UserAccount] PRIMARY KEY CLUSTERED ([UserAccountID])
);
GO

-- Chèn dữ liệu mẫu vào UserAccount
SET IDENTITY_INSERT [dbo].[System.UserAccount] ON;
INSERT INTO [dbo].[System.UserAccount] ([UserAccountID], [UserName], [Password], [FullName], [Email], [Phone], [EmployeeCode], [RoleId], [RequestCode], [CreatedDate], [ApplicationCode], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive])
VALUES 
(1, N'acc', N'@a', N'Accountant', N'Accountant@', N'0913652742', N'000001', 2, NULL, NULL, NULL, NULL, NULL, NULL, 1),
(2, N'auditor', N'@a', N'Internal Auditor', N'InternalAuditor@', N'0972224568', N'000002', 3, NULL, NULL, NULL, NULL, NULL, NULL, 1),
(3, N'chiefacc', N'@a', N'Chief Accountant', N'ChiefAccountant@', N'0902927373', N'000003', 1, NULL, NULL, NULL, NULL, NULL, NULL, 1);
SET IDENTITY_INSERT [dbo].[System.UserAccount] OFF;
GO

-- Bảng nhóm máu
CREATE TABLE BloodTypes (
    BloodTypeID INT IDENTITY PRIMARY KEY,
    ABOGroup NVARCHAR(3),
    RhFactor NVARCHAR(10),
    CanDonateTo NVARCHAR(255),
    CanReceiveFrom NVARCHAR(255),
    PlasmaCompatibility NVARCHAR(255),
    RedCellCompatibility NVARCHAR(255),
    PlateletCompatibility NVARCHAR(255),
    Notes NVARCHAR(255),
    Code NVARCHAR(10),
    IsUniversalDonor BIT
);

-- Ghi chú nhóm máu
CREATE TABLE BloodTypeNotes (
    NoteID INT IDENTITY PRIMARY KEY,
    BloodTypeID INT,
    NoteDate DATETIME,
    NoteContent NVARCHAR(500),
    AddedBy NVARCHAR(100),
    FOREIGN KEY (BloodTypeID) REFERENCES BloodTypes(BloodTypeID)
);

-- Tiền sử bệnh
CREATE TABLE UserMedicalHistories (
    HistoryID INT IDENTITY PRIMARY KEY,
    UserID INT,
    Diagnosis NVARCHAR(255),
    DiagnosisDate DATE,
    IsChronic BIT,
    Notes NVARCHAR(255),
    FOREIGN KEY (UserID) REFERENCES [dbo].[System.UserAccount](UserAccountID)
);

-- Hiến máu
CREATE TABLE Donations (
    DonationID INT IDENTITY PRIMARY KEY,
    UserID INT,
    BloodTypeID INT,
    DonationDate DATETIME,
    DonationType NVARCHAR(50),
    Volume INT,
    Location NVARCHAR(255),
    Staff NVARCHAR(100),
    HealthCheckStatus NVARCHAR(50),
    HemoglobinLevel DECIMAL(4,2),
    BloodPressure NVARCHAR(20),
    Notes NVARCHAR(255),
    Status NVARCHAR(50),
    FOREIGN KEY (UserID) REFERENCES [dbo].[System.UserAccount](UserAccountID),
    FOREIGN KEY (BloodTypeID) REFERENCES BloodTypes(BloodTypeID)
);

-- Xét nghiệm
CREATE TABLE DonationTests (
    TestID INT IDENTITY PRIMARY KEY,
    DonationID INT,
    TestName NVARCHAR(100),
    Result NVARCHAR(100),
    TestDate DATETIME,
    PerformedBy NVARCHAR(100),
    FOREIGN KEY (DonationID) REFERENCES Donations(DonationID)
);

-- Yêu cầu máu
CREATE TABLE Requests (
    RequestID INT IDENTITY PRIMARY KEY,
    RequestorID INT,
    BloodTypeID INT,
    ComponentType NVARCHAR(50),
    Volume INT,
    Location NVARCHAR(255),
    Hospital NVARCHAR(255),
    RequestDate DATETIME,
    RequiredDate DATETIME,
    Status NVARCHAR(50),
    ContactPerson NVARCHAR(100),
    ContactPhone NVARCHAR(15),
    Notes NVARCHAR(255),
    FOREIGN KEY (RequestorID) REFERENCES [dbo].[System.UserAccount](UserAccountID),
    FOREIGN KEY (BloodTypeID) REFERENCES BloodTypes(BloodTypeID)
);

-- Nhật ký yêu cầu
CREATE TABLE RequestLogs (
    LogID INT IDENTITY PRIMARY KEY,
    RequestID INT,
    Action NVARCHAR(100),
    PerformedAt DATETIME,
    PerformedBy NVARCHAR(100),
    Notes NVARCHAR(255),
    FOREIGN KEY (RequestID) REFERENCES Requests(RequestID)
);

-- Đơn vị máu
CREATE TABLE BloodUnits (
    BloodUnitID INT IDENTITY PRIMARY KEY,
    BloodTypeID INT,
    Volume INT,
    ComponentType NVARCHAR(50),
    ExpirationDate DATE,
    StorageLocation NVARCHAR(100),
    BatchCode NVARCHAR(50),
    Source NVARCHAR(100),
    Status NVARCHAR(50),
    Tested BIT,
    TestResult NVARCHAR(50),
    StorageTemperature NVARCHAR(10),
    FOREIGN KEY (BloodTypeID) REFERENCES BloodTypes(BloodTypeID)
);

-- Di chuyển đơn vị máu
CREATE TABLE BloodUnitMovements (
    MovementID INT IDENTITY PRIMARY KEY,
    BloodUnitID INT,
    MovedDate DATETIME,
    FromLocation NVARCHAR(100),
    ToLocation NVARCHAR(100),
    Reason NVARCHAR(255),
    HandledBy NVARCHAR(100),
    FOREIGN KEY (BloodUnitID) REFERENCES BloodUnits(BloodUnitID)
);

-- Phục hồi
CREATE TABLE Recoveries (
    RecoveryID INT IDENTITY PRIMARY KEY,
    UserID INT,
    LastDonationDate DATETIME,
    NextEligibleDate DATETIME,
    RecoveryDays INT,
    DonationCount INT,
    WeightAtLastDonation DECIMAL(5,2),
    RestRecommendation NVARCHAR(255),
    DoctorName NVARCHAR(100),
    Notes NVARCHAR(255),
    Status NVARCHAR(50),
    FOREIGN KEY (UserID) REFERENCES [dbo].[System.UserAccount](UserAccountID)
);

-- Ghi chú phục hồi
CREATE TABLE RecoveryNotes (
    NoteID INT IDENTITY PRIMARY KEY,
    RecoveryID INT,
    NoteDate DATETIME,
    DoctorName NVARCHAR(100),
    Observation NVARCHAR(255),
    FOREIGN KEY (RecoveryID) REFERENCES Recoveries(RecoveryID)
);

-- Yêu cầu khẩn
CREATE TABLE EmergencyRequests (
    EmergencyID INT IDENTITY PRIMARY KEY,
    RequestID INT,
    CreatedBy INT,
    EmergencyLevel NVARCHAR(50),
    AlertTime DATETIME,
    ResponseDeadline DATETIME,
    NumberOfUnitsNeeded INT,
    ResponderCount INT,
    BroadcastLocation NVARCHAR(255),
    Channel NVARCHAR(100),
    Status NVARCHAR(50),
    FOREIGN KEY (RequestID) REFERENCES Requests(RequestID),
    FOREIGN KEY (CreatedBy) REFERENCES [dbo].[System.UserAccount](UserAccountID)
);

-- Phản hồi khẩn
CREATE TABLE EmergencyResponses (
    ResponseID INT IDENTITY PRIMARY KEY,
    EmergencyID INT,
    ResponderID INT,
    ResponseTime DATETIME,
    Confirmed BIT,
    ResponseChannel NVARCHAR(50),
    FOREIGN KEY (EmergencyID) REFERENCES EmergencyRequests(EmergencyID),
    FOREIGN KEY (ResponderID) REFERENCES [dbo].[System.UserAccount](UserAccountID)
);

-- Ghép nối người hiến
CREATE TABLE Matchings (
    MatchingID INT IDENTITY PRIMARY KEY,
    RequestID INT,
    DonorID INT,
    MatchDate DATETIME,
    MatchType NVARCHAR(50),
    DistanceKM DECIMAL(6,2),
    IsConfirmed BIT,
    ConfirmationTime DATETIME,
    ContactMethod NVARCHAR(50),
    Notes NVARCHAR(255),
    Status NVARCHAR(50),
    FOREIGN KEY (RequestID) REFERENCES Requests(RequestID),
    FOREIGN KEY (DonorID) REFERENCES [dbo].[System.UserAccount](UserAccountID)
);

-- Trạng thái ghép nối
CREATE TABLE MatchingStatuses (
    StatusID INT IDENTITY PRIMARY KEY,
    MatchingID INT,
    Status NVARCHAR(50),
    UpdatedAt DATETIME,
    UpdatedBy NVARCHAR(100),
    FOREIGN KEY (MatchingID) REFERENCES Matchings(MatchingID)
);

-- Nhắc nhở hiến máu
CREATE TABLE DonorReminders (
    ReminderID INT IDENTITY PRIMARY KEY,
    UserID INT,
    NextDonationDate DATE,
    ReminderDate DATE,
    Method NVARCHAR(50),
    Message NVARCHAR(255),
    Status NVARCHAR(50),
    ResentCount INT,
    ResponseStatus NVARCHAR(50),
    SentBy NVARCHAR(100),
    FOREIGN KEY (UserID) REFERENCES [dbo].[System.UserAccount](UserAccountID)
);

-- Nhật ký nhắc nhở
CREATE TABLE ReminderLogs (
    LogID INT IDENTITY PRIMARY KEY,
    ReminderID INT,
    LogDate DATETIME,
    Action NVARCHAR(100),
    PerformedBy NVARCHAR(100),
    FOREIGN KEY (ReminderID) REFERENCES DonorReminders(ReminderID)
);

-- 📌 Bảng loại báo cáo
CREATE TABLE ReportTypesHungHK (
    ReportTypeHungHKID INT IDENTITY PRIMARY KEY,
    TypeName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255)
);

-- 📌 Bảng báo cáo
CREATE TABLE ReportsHungHK (
    ReportHungHKID INT IDENTITY PRIMARY KEY,
    ReportDate DATE,
    ReportTypeID INT,
    TotalUsers INT,
    TotalDonors INT,
    TotalRecipients INT,
    TotalBloodUnits INT,
    TotalRequests INT,
    TotalDonations INT,
    MostNeededBloodType NVARCHAR(10),
	Status BIT,
    GeneratedBy INT,
    FOREIGN KEY (ReportTypeID) REFERENCES ReportTypesHungHK(ReportTypeHungHKID),
    FOREIGN KEY (GeneratedBy) REFERENCES [dbo].[System.UserAccount](UserAccountID)
);

-- Seed một số loại báo cáo
INSERT INTO ReportTypesHungHK (TypeName, Description)
VALUES 
(N'Tổng Quan', N'Báo cáo tổng hợp toàn hệ thống'),
(N'Theo Tháng', N'Số liệu theo tháng'),
(N'Khẩn Cấp', N'Phân tích báo cáo khẩn');

INSERT INTO ReportsHungHK (
    ReportDate,
    ReportTypeID,
    TotalUsers,
    TotalDonors,
    TotalRecipients,
    TotalBloodUnits,
    TotalRequests,
    TotalDonations,
    MostNeededBloodType,
    Status,
    GeneratedBy
)
VALUES
('2025-01-15', 1, 1500, 800, 700, 320, 220, 210, 'O+', 1, 1),
('2025-06-30', 2, 320, 180, 140, 65, 50, 48, 'A+', 1, 2),
('2025-07-05', 3, 230, 128, 126, 342, 30, 18, 'B-', 1, 3),
('2025-01-15', 1, 500, 250, 250, 100, 80, 75, 'O+', 1, 1),
('2025-02-15', 1, 520, 270, 250, 110, 85, 80, 'A+', 1, 1),
('2025-03-15', 2, 540, 280, 260, 115, 90, 85, 'B+', 1, 2),
('2025-04-15', 2, 560, 300, 260, 120, 95, 90, 'O-', 1, 2),
('2025-05-15', 2, 580, 310, 270, 130, 100, 95, 'A-', 1, 3),
('2025-06-15', 2, 600, 320, 280, 135, 110, 100, 'AB+', 1, 3),
('2025-07-15', 3, 620, 330, 290, 140, 115, 110, 'O+', 1, 1),
('2025-08-15', 3, 640, 340, 300, 150, 120, 115, 'B-', 1, 1),
('2025-09-15', 3, 660, 350, 310, 160, 125, 120, 'A+', 1, 2),
('2025-10-15', 1, 680, 360, 320, 170, 130, 125, 'O-', 1, 2),
('2025-11-15', 1, 700, 370, 330, 180, 135, 130, 'AB-', 1, 3),
('2025-12-15', 1, 720, 380, 340, 190, 140, 135, 'A+', 1, 3);
