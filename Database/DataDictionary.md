# Data Dictionary - Smart Event Management and Ticketing System

## Table: Members

| Attribute | Data Type | Constraints | Description |
|-----------|-----------|-------------|-------------|
| MemberID | INT | PK, IDENTITY(1,1) | Unique identifier for each member |
| Email | NVARCHAR(255) | NOT NULL, UNIQUE | Member's login email |
| PasswordHash | NVARCHAR(255) | NOT NULL | Hashed password for authentication |
| FirstName | NVARCHAR(100) | NOT NULL | Member's first name |
| LastName | NVARCHAR(100) | NOT NULL | Member's last name |
| Phone | NVARCHAR(20) | | Contact phone number |
| Address | NVARCHAR(500) | | Physical address |
| PreferredMusic | BIT | DEFAULT 0 | Event preference: Music |
| PreferredTheatre | BIT | DEFAULT 0 | Event preference: Theatre |
| PreferredSports | BIT | DEFAULT 0 | Event preference: Sports |
| PreferredArts | BIT | DEFAULT 0 | Event preference: Arts |
| PreferredWorkshops | BIT | DEFAULT 0 | Event preference: Workshops |
| PreferredExhibitions | BIT | DEFAULT 0 | Event preference: Exhibitions |
| CreatedAt | DATETIME2 | DEFAULT GETDATE() | Registration timestamp |
| IsActive | BIT | DEFAULT 1 | Account status |

---

## Table: Venues

| Attribute | Data Type | Constraints | Description |
|-----------|-----------|-------------|-------------|
| VenueID | INT | PK, IDENTITY(1,1) | Unique identifier for each venue |
| Name | NVARCHAR(200) | NOT NULL | Venue name |
| Address | NVARCHAR(500) | NOT NULL | Full address |
| City | NVARCHAR(100) | NOT NULL | City location |
| Capacity | INT | NOT NULL | Maximum capacity |
| Description | NVARCHAR(MAX) | | Venue details |

---

## Table: Events

| Attribute | Data Type | Constraints | Description |
|-----------|-----------|-------------|-------------|
| EventID | INT | PK, IDENTITY(1,1) | Unique identifier for each event |
| Title | NVARCHAR(300) | NOT NULL | Event title |
| Description | NVARCHAR(MAX) | | Full event description |
| Category | NVARCHAR(50) | NOT NULL | Music, Theatre, Sports, Arts, Workshop, Exhibition |
| EventDate | DATETIME2 | NOT NULL | Date and time of event |
| VenueID | INT | NOT NULL, FK→Venues | Venue where event occurs |
| BasePrice | DECIMAL(10,2) | NOT NULL | Starting price |
| ImageUrl | NVARCHAR(500) | | Event image path |
| IsActive | BIT | DEFAULT 1 | Event availability |
| CreatedAt | DATETIME2 | DEFAULT GETDATE() | Creation timestamp |

---

## Table: EventSeats

| Attribute | Data Type | Constraints | Description |
|-----------|-----------|-------------|-------------|
| EventSeatID | INT | PK, IDENTITY(1,1) | Unique identifier |
| EventID | INT | NOT NULL, FK→Events | Associated event |
| SeatType | NVARCHAR(50) | NOT NULL | VIP, Standard, Balcony |
| Price | DECIMAL(10,2) | NOT NULL | Price for this seat type |
| TotalSeats | INT | NOT NULL | Total seats of this type |
| AvailableSeats | INT | NOT NULL | Remaining seats |

---

## Table: Bookings

| Attribute | Data Type | Constraints | Description |
|-----------|-----------|-------------|-------------|
| BookingID | INT | PK, IDENTITY(1,1) | Unique identifier |
| MemberID | INT | NOT NULL, FK→Members | Member who made booking |
| EventID | INT | NOT NULL, FK→Events | Event booked |
| BookingDate | DATETIME2 | DEFAULT GETDATE() | When booking was made |
| Status | NVARCHAR(20) | DEFAULT 'Confirmed' | Confirmed, Cancelled, Completed |
| TotalAmount | DECIMAL(10,2) | NOT NULL | Total booking cost |

---

## Table: BookingDetails

| Attribute | Data Type | Constraints | Description |
|-----------|-----------|-------------|-------------|
| BookingDetailID | INT | PK, IDENTITY(1,1) | Unique identifier |
| BookingID | INT | NOT NULL, FK→Bookings | Parent booking |
| SeatType | NVARCHAR(50) | NOT NULL | Type of seat booked |
| Quantity | INT | NOT NULL | Number of tickets |
| UnitPrice | DECIMAL(10,2) | NOT NULL | Price per ticket |
| SubTotal | DECIMAL(10,2) | NOT NULL | Line total |

---

## Table: Reviews

| Attribute | Data Type | Constraints | Description |
|-----------|-----------|-------------|-------------|
| ReviewID | INT | PK, IDENTITY(1,1) | Unique identifier |
| MemberID | INT | NOT NULL, FK→Members | Reviewer |
| EventID | INT | NOT NULL, FK→Events | Event reviewed |
| Rating | INT | NOT NULL, CHECK(1-5) | Star rating 1-5 |
| Comment | NVARCHAR(MAX) | | Review text |
| ReviewDate | DATETIME2 | DEFAULT GETDATE() | When review was posted |

**Unique Constraint:** One review per member per event (MemberID, EventID)

---

## Table: Inquiries

| Attribute | Data Type | Constraints | Description |
|-----------|-----------|-------------|-------------|
| InquiryID | INT | PK, IDENTITY(1,1) | Unique identifier |
| GuestName | NVARCHAR(200) | NOT NULL | Sender's name |
| GuestEmail | NVARCHAR(255) | NOT NULL | Sender's email |
| Subject | NVARCHAR(300) | NOT NULL | Inquiry subject |
| Message | NVARCHAR(MAX) | NOT NULL | Inquiry content |
| InquiryDate | DATETIME2 | DEFAULT GETDATE() | Submission date |
| IsResolved | BIT | DEFAULT 0 | Resolution status |

---

## Entity Relationships

- **Member** 1:N **Booking** (One member can have many bookings)
- **Member** 1:N **Review** (One member can write many reviews)
- **Event** 1:N **Booking** (One event can have many bookings)
- **Event** 1:N **EventSeats** (One event has multiple seat types)
- **Event** 1:N **Review** (One event can have many reviews)
- **Venue** 1:N **Event** (One venue hosts many events)
- **Booking** 1:N **BookingDetails** (One booking has multiple ticket lines)
