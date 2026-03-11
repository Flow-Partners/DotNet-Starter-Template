-- =============================================================================
-- Care Bridge – Database Schema (Reference)
-- =============================================================================
-- This file describes the full database structure for understanding only.
-- The actual database is created automatically when you run the project
-- (EF Core migrations apply on startup).
--
-- Tables are grouped as:
--   1. Identity & Auth (AspNet*, Permissions, UserRoles)
--   2. Dental / Care Bridge (Procedures, Patients, Dentists, Appointments, etc.)
-- =============================================================================

-- -----------------------------------------------------------------------------
-- 1. IDENTITY & AUTH (created by InitialCreate migration)
-- -----------------------------------------------------------------------------

-- AspNetRoles: application roles
-- AspNetUsers: application users (extends Identity)
-- AspNetRoleClaims, AspNetUserClaims, AspNetUserLogins, AspNetUserRoles, AspNetUserTokens
-- Permissions: permission definitions
-- RolePermissions: role–permission mapping
-- UserRoles: user–role mapping (custom)

-- -----------------------------------------------------------------------------
-- 2. DENTAL / CARE BRIDGE (created by AddDentalEntities migration)
-- -----------------------------------------------------------------------------

-- Procedures: catalog of treatments with default pricing (for future use)
CREATE TABLE Procedures (
    Id              INT IDENTITY(1,1) PRIMARY KEY,
    Name            NVARCHAR(200) NOT NULL,
    Code            NVARCHAR(20)  NOT NULL,
    DefaultPrice    DECIMAL(18,2) NOT NULL,
    Category        NVARCHAR(100) NULL,
    Description     NVARCHAR(MAX) NULL,
    IsActive        BIT NOT NULL DEFAULT 1,
    CreatedAt       DATETIME2 NOT NULL
);

-- Patients: patient register (pay later, credit limit for future use)
CREATE TABLE Patients (
    Id                    INT IDENTITY(1,1) PRIMARY KEY,
    FirstName             NVARCHAR(MAX) NOT NULL,
    LastName              NVARCHAR(MAX) NOT NULL,
    Email                 NVARCHAR(MAX) NOT NULL,
    Phone                 NVARCHAR(MAX) NULL,
    DateOfBirth           DATETIME2 NOT NULL,
    Address               NVARCHAR(MAX) NULL,
    MedicalHistory        NVARCHAR(MAX) NULL,
    AllowPayLater         BIT NOT NULL DEFAULT 0,
    CreditLimit           DECIMAL(18,2) NULL,
    EmergencyContactName  NVARCHAR(200) NULL,
    EmergencyContactPhone NVARCHAR(50)  NULL,
    CreatedAt             DATETIME2 NOT NULL,
    UpdatedAt             DATETIME2 NULL
);

-- Dentists: dentist/doctor register
CREATE TABLE Dentists (
    Id             INT IDENTITY(1,1) PRIMARY KEY,
    FirstName      NVARCHAR(MAX) NOT NULL,
    LastName       NVARCHAR(MAX) NOT NULL,
    Email          NVARCHAR(MAX) NOT NULL,
    Phone          NVARCHAR(MAX) NULL,
    Specialization NVARCHAR(MAX) NULL,
    LicenseNumber  NVARCHAR(MAX) NULL,
    IsActive       BIT NOT NULL DEFAULT 1,
    CreatedAt      DATETIME2 NOT NULL,
    UpdatedAt      DATETIME2 NULL
);

-- Appointments: links patient + dentist + optional procedure
CREATE TABLE Appointments (
    Id              INT IDENTITY(1,1) PRIMARY KEY,
    PatientId        INT NOT NULL,
    DentistId        INT NOT NULL,
    AppointmentDate  DATETIME2 NOT NULL,
    ProcedureType    NVARCHAR(MAX) NOT NULL,
    ProcedureId      INT NULL,
    Status           NVARCHAR(MAX) NOT NULL,
    TotalAmount      DECIMAL(18,2) NOT NULL,
    EstimatedCost    DECIMAL(18,2) NOT NULL,
    Notes            NVARCHAR(MAX) NULL,
    CreatedAt        DATETIME2 NOT NULL,
    UpdatedAt        DATETIME2 NULL,
    CONSTRAINT FK_Appointments_Patients  FOREIGN KEY (PatientId)  REFERENCES Patients(Id),
    CONSTRAINT FK_Appointments_Dentists  FOREIGN KEY (DentistId)  REFERENCES Dentists(Id),
    CONSTRAINT FK_Appointments_Procedures FOREIGN KEY (ProcedureId) REFERENCES Procedures(Id) ON DELETE SET NULL
);

-- AppointmentImages: before/after images per appointment (in use)
CREATE TABLE AppointmentImages (
    Id            INT IDENTITY(1,1) PRIMARY KEY,
    AppointmentId INT NOT NULL,
    ImageType    NVARCHAR(MAX) NOT NULL,
    ImageUrl     NVARCHAR(MAX) NOT NULL,
    Description  NVARCHAR(MAX) NULL,
    UploadedAt   DATETIME2 NOT NULL,
    CONSTRAINT FK_AppointmentImages_Appointments FOREIGN KEY (AppointmentId) REFERENCES Appointments(Id) ON DELETE CASCADE
);

-- Invoices: one per appointment; total, paid, remaining (for future use)
CREATE TABLE Invoices (
    Id              INT IDENTITY(1,1) PRIMARY KEY,
    AppointmentId   INT NOT NULL UNIQUE,
    PatientId       INT NOT NULL,
    DentistId       INT NOT NULL,
    TotalAmount     DECIMAL(18,2) NOT NULL,
    PaidAmount      DECIMAL(18,2) NOT NULL,
    RemainingAmount DECIMAL(18,2) NOT NULL,
    Status          NVARCHAR(20) NOT NULL,
    DueDate         DATETIME2 NULL,
    Notes           NVARCHAR(MAX) NULL,
    CreatedAt       DATETIME2 NOT NULL,
    UpdatedAt       DATETIME2 NULL,
    CONSTRAINT FK_Invoices_Appointments FOREIGN KEY (AppointmentId) REFERENCES Appointments(Id),
    CONSTRAINT FK_Invoices_Patients     FOREIGN KEY (PatientId)     REFERENCES Patients(Id),
    CONSTRAINT FK_Invoices_Dentists    FOREIGN KEY (DentistId)     REFERENCES Dentists(Id)
);

-- Payments: payments against an invoice (for future use)
CREATE TABLE Payments (
    Id              INT IDENTITY(1,1) PRIMARY KEY,
    InvoiceId       INT NOT NULL,
    Amount          DECIMAL(18,2) NOT NULL,
    PaymentDate     DATETIME2 NOT NULL,
    Method          NVARCHAR(50) NOT NULL,
    ReferenceNumber NVARCHAR(100) NULL,
    Notes           NVARCHAR(MAX) NULL,
    CreatedAt       DATETIME2 NOT NULL,
    CONSTRAINT FK_Payments_Invoices FOREIGN KEY (InvoiceId) REFERENCES Invoices(Id) ON DELETE CASCADE
);

-- Prescriptions: per appointment (for future use)
CREATE TABLE Prescriptions (
    Id            INT IDENTITY(1,1) PRIMARY KEY,
    AppointmentId INT NOT NULL,
    PatientId     INT NOT NULL,
    DentistId     INT NOT NULL,
    PrescribedAt  DATETIME2 NOT NULL,
    Notes         NVARCHAR(MAX) NULL,
    CreatedAt     DATETIME2 NOT NULL,
    CONSTRAINT FK_Prescriptions_Appointments FOREIGN KEY (AppointmentId) REFERENCES Appointments(Id),
    CONSTRAINT FK_Prescriptions_Patients     FOREIGN KEY (PatientId)     REFERENCES Patients(Id),
    CONSTRAINT FK_Prescriptions_Dentists    FOREIGN KEY (DentistId)     REFERENCES Dentists(Id)
);

-- PrescriptionItems: line items for a prescription (for future use)
CREATE TABLE PrescriptionItems (
    Id              INT IDENTITY(1,1) PRIMARY KEY,
    PrescriptionId  INT NOT NULL,
    MedicationName  NVARCHAR(200) NOT NULL,
    Dosage          NVARCHAR(100) NULL,
    Frequency       NVARCHAR(100) NULL,
    Duration        NVARCHAR(100) NULL,
    Instructions    NVARCHAR(MAX) NULL,
    CONSTRAINT FK_PrescriptionItems_Prescriptions FOREIGN KEY (PrescriptionId) REFERENCES Prescriptions(Id) ON DELETE CASCADE
);

-- AppointmentReminders: SMS/Email reminders (for future use)
CREATE TABLE AppointmentReminders (
    Id            INT IDENTITY(1,1) PRIMARY KEY,
    AppointmentId INT NOT NULL,
    ReminderType NVARCHAR(20) NOT NULL,
    ScheduledAt  DATETIME2 NOT NULL,
    SentAt       DATETIME2 NULL,
    Status       NVARCHAR(20) NOT NULL,
    Notes        NVARCHAR(MAX) NULL,
    CreatedAt    DATETIME2 NOT NULL,
    CONSTRAINT FK_AppointmentReminders_Appointments FOREIGN KEY (AppointmentId) REFERENCES Appointments(Id) ON DELETE CASCADE
);

-- =============================================================================
-- Summary
-- =============================================================================
-- In use now:  Patients, Dentists, Appointments, AppointmentImages
-- For future:  Procedures, Invoices, Payments, Prescriptions, PrescriptionItems, AppointmentReminders
-- =============================================================================
