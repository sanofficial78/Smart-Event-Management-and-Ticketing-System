-- =============================================================================
-- SMART EVENT MANAGEMENT AND TICKETING SYSTEM
-- Database Schema Specification (SQL Server)
-- Metropolitan Cultural Council
-- =============================================================================

-- Drop tables in correct order (respecting foreign keys)
IF OBJECT_ID('dbo.Inquiries', 'U') IS NOT NULL DROP TABLE dbo.Inquiries;
IF OBJECT_ID('dbo.Reviews', 'U') IS NOT NULL DROP TABLE dbo.Reviews;
IF OBJECT_ID('dbo.BookingDetails', 'U') IS NOT NULL DROP TABLE dbo.BookingDetails;
IF OBJECT_ID('dbo.Bookings', 'U') IS NOT NULL DROP TABLE dbo.Bookings;
IF OBJECT_ID('dbo.EventSeats', 'U') IS NOT NULL DROP TABLE dbo.EventSeats;
IF OBJECT_ID('dbo.Events', 'U') IS NOT NULL DROP TABLE dbo.Events;
IF OBJECT_ID('dbo.Venues', 'U') IS NOT NULL DROP TABLE dbo.Venues;
IF OBJECT_ID('dbo.Members', 'U') IS NOT NULL DROP TABLE dbo.Members;

-- =============================================================================
-- MEMBERS TABLE
-- Stores registered community members
-- =============================================================================
CREATE TABLE Members (
    MemberID        INT IDENTITY(1,1) PRIMARY KEY,
    Email           NVARCHAR(255) NOT NULL UNIQUE,
    PasswordHash    NVARCHAR(255) NOT NULL,
    FirstName       NVARCHAR(100) NOT NULL,
    LastName        NVARCHAR(100) NOT NULL,
    Phone           NVARCHAR(20),
    Address         NVARCHAR(500),
    PreferredMusic  BIT DEFAULT 0,
    PreferredTheatre BIT DEFAULT 0,
    PreferredSports BIT DEFAULT 0,
    PreferredArts   BIT DEFAULT 0,
    PreferredWorkshops BIT DEFAULT 0,
    PreferredExhibitions BIT DEFAULT 0,
    CreatedAt       DATETIME2 DEFAULT GETDATE(),
    IsActive        BIT DEFAULT 1
);

-- =============================================================================
-- VENUES TABLE
-- Event locations
-- =============================================================================
CREATE TABLE Venues (
    VenueID         INT IDENTITY(1,1) PRIMARY KEY,
    Name            NVARCHAR(200) NOT NULL,
    Address         NVARCHAR(500) NOT NULL,
    City            NVARCHAR(100) NOT NULL,
    Capacity        INT NOT NULL,
    Description     NVARCHAR(MAX)
);

-- =============================================================================
-- EVENTS TABLE
-- Cultural events (concerts, plays, exhibitions, workshops)
-- =============================================================================
CREATE TABLE Events (
    EventID         INT IDENTITY(1,1) PRIMARY KEY,
    Title           NVARCHAR(300) NOT NULL,
    Description     NVARCHAR(MAX),
    Category        NVARCHAR(50) NOT NULL,  -- Music, Theatre, Sports, Arts, Workshop, Exhibition
    EventDate       DATETIME2 NOT NULL,
    VenueID         INT NOT NULL,
    BasePrice       DECIMAL(10,2) NOT NULL,
    ImageUrl        NVARCHAR(500),
    IsActive        BIT DEFAULT 1,
    CreatedAt       DATETIME2 DEFAULT GETDATE(),
    CONSTRAINT FK_Events_Venue FOREIGN KEY (VenueID) REFERENCES Venues(VenueID)
);

-- =============================================================================
-- EVENT_SEATS TABLE
-- Seat availability by type (e.g., VIP, Standard, Balcony)
-- =============================================================================
CREATE TABLE EventSeats (
    EventSeatID     INT IDENTITY(1,1) PRIMARY KEY,
    EventID         INT NOT NULL,
    SeatType        NVARCHAR(50) NOT NULL,  -- VIP, Standard, Balcony, etc.
    Price           DECIMAL(10,2) NOT NULL,
    TotalSeats      INT NOT NULL,
    AvailableSeats  INT NOT NULL,
    CONSTRAINT FK_EventSeats_Event FOREIGN KEY (EventID) REFERENCES Events(EventID) ON DELETE CASCADE
);

-- =============================================================================
-- BOOKINGS TABLE
-- Member ticket reservations
-- =============================================================================
CREATE TABLE Bookings (
    BookingID       INT IDENTITY(1,1) PRIMARY KEY,
    MemberID        INT NOT NULL,
    EventID         INT NOT NULL,
    BookingDate     DATETIME2 DEFAULT GETDATE(),
    Status          NVARCHAR(20) DEFAULT 'Confirmed',  -- Confirmed, Cancelled, Completed
    TotalAmount     DECIMAL(10,2) NOT NULL,
    CONSTRAINT FK_Bookings_Member FOREIGN KEY (MemberID) REFERENCES Members(MemberID),
    CONSTRAINT FK_Bookings_Event FOREIGN KEY (EventID) REFERENCES Events(EventID)
);

-- =============================================================================
-- BOOKING_DETAILS TABLE
-- Individual ticket lines (seat type, quantity)
-- =============================================================================
CREATE TABLE BookingDetails (
    BookingDetailID INT IDENTITY(1,1) PRIMARY KEY,
    BookingID       INT NOT NULL,
    SeatType        NVARCHAR(50) NOT NULL,
    Quantity        INT NOT NULL,
    UnitPrice       DECIMAL(10,2) NOT NULL,
    SubTotal        DECIMAL(10,2) NOT NULL,
    CONSTRAINT FK_BookingDetails_Booking FOREIGN KEY (BookingID) REFERENCES Bookings(BookingID) ON DELETE CASCADE
);

-- =============================================================================
-- REVIEWS TABLE
-- Member feedback after attending events
-- =============================================================================
CREATE TABLE Reviews (
    ReviewID        INT IDENTITY(1,1) PRIMARY KEY,
    MemberID        INT NOT NULL,
    EventID         INT NOT NULL,
    Rating          INT NOT NULL CHECK (Rating BETWEEN 1 AND 5),
    Comment         NVARCHAR(MAX),
    ReviewDate      DATETIME2 DEFAULT GETDATE(),
    CONSTRAINT FK_Reviews_Member FOREIGN KEY (MemberID) REFERENCES Members(MemberID),
    CONSTRAINT FK_Reviews_Event FOREIGN KEY (EventID) REFERENCES Events(EventID),
    CONSTRAINT UQ_Member_Event_Review UNIQUE (MemberID, EventID)
);

-- =============================================================================
-- INQUIRIES TABLE
-- Guest questions (about events or membership)
-- =============================================================================
CREATE TABLE Inquiries (
    InquiryID       INT IDENTITY(1,1) PRIMARY KEY,
    GuestName       NVARCHAR(200) NOT NULL,
    GuestEmail      NVARCHAR(255) NOT NULL,
    Subject         NVARCHAR(300) NOT NULL,
    Message         NVARCHAR(MAX) NOT NULL,
    InquiryDate     DATETIME2 DEFAULT GETDATE(),
    IsResolved      BIT DEFAULT 0
);

-- Indexes for performance
CREATE INDEX IX_Events_Category ON Events(Category);
CREATE INDEX IX_Events_EventDate ON Events(EventDate);
CREATE INDEX IX_Events_VenueID ON Events(VenueID);
CREATE INDEX IX_Bookings_MemberID ON Bookings(MemberID);
CREATE INDEX IX_Bookings_EventID ON Bookings(EventID);
CREATE INDEX IX_Reviews_EventID ON Reviews(EventID);
CREATE INDEX IX_EventSeats_EventID ON EventSeats(EventID);
