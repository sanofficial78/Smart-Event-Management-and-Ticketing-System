# Smart Event Management and Ticketing System
## Comprehensive Design and Implementation Report

### 1. Introduction

The Metropolitan Cultural Council required a modern system to manage local cultural events (concerts, plays, exhibitions, workshops) and enable the public to browse and book tickets online. This report documents the design, implementation, and testing of the Smart Event Management and Ticketing System.

### 2. System Requirements

#### 2.1 Home Page
- Overview of the cultural council
- Display of upcoming events
- Entry point for members and guests with login/registration options

#### 2.2 Members
- **Sign In**: Access account
- **Registration**: Personal info and event preferences (music, theatre, sports, arts, workshops, exhibitions)
- **Browse & Search Events**: By category, date, location, price
- **Book Tickets**: Reserve with seat type and quantity
- **Review Submission**: Submit feedback after attending events

#### 2.3 Guests
- Browse basic event info (name, category, date, venue)
- Read reviews left by members
- Restricted search (Available/Full without seat details)
- Membership registration
- Send inquiries (questions about events or membership)

---

### 3. Database Design

#### 3.1 Database Relations Specification (Oracle/SQL Server)

The schema consists of 8 tables:

| Table | Description |
|-------|-------------|
| Members | Registered community members |
| Venues | Event locations |
| Events | Cultural events |
| EventSeats | Seat types and availability per event |
| Bookings | Member ticket reservations |
| BookingDetails | Line items (seat type, quantity, price) |
| Reviews | Member feedback on events |
| Inquiries | Guest questions |

**Primary Keys**: MemberID, VenueID, EventID, EventSeatID, BookingID, BookingDetailID, ReviewID, InquiryID

**Foreign Keys**: Events.VenueID→Venues; EventSeats.EventID→Events; Bookings.MemberID→Members, Bookings.EventID→Events; BookingDetails.BookingID→Bookings; Reviews.MemberID→Members, Reviews.EventID→Events

#### 3.2 ER Diagram

See `Database/ER-Diagram.md` for the Mermaid ER diagram. Key relationships:

- Member 1:N Booking
- Member 1:N Review
- Event 1:N Booking
- Event 1:N EventSeats
- Event 1:N Review
- Venue 1:N Event
- Booking 1:N BookingDetails

#### 3.3 Data Dictionary

See `Database/DataDictionary.md` for full attribute definitions, data types, and constraints.

---

### 4. Database Implementation

#### 4.1 Scripts

- **Schema.sql**: CREATE TABLE statements with PK, FK, indexes
- **SampleData.sql**: INSERT statements for venues, events, seats, members, bookings, reviews, inquiries
- **Queries.sql**: Sample SELECT queries (upcoming events, search by category/date/location/price, bookings, availability, reviews)

#### 4.2 Connection

The ASP.NET application uses:
- **SQL Server**: When `ConnectionStrings:DefaultConnection` is set in appsettings.json
- **SQLite**: Fallback (`events.db`) when no SQL Server connection is configured (development)

---

### 5. Web Application Implementation (ASP.NET MVC)

#### 5.1 Architecture

- **Framework**: ASP.NET Core 8 MVC
- **Data Access**: Entity Framework Core
- **Authentication**: Session-based (MemberID stored in session after login)
- **Pattern**: Services (IEventService, IBookingService, etc.) + Controllers + Views

#### 5.2 Pages and Features

| Page | URL | Description |
|------|-----|-------------|
| Home | / | Council overview, upcoming events, login/register links |
| Events | /Events | Search/browse events (guest vs member view) |
| Event Details | /Events/Details/{id} | Full details, seat options (members), reviews |
| Book Tickets | /Bookings/Book/{id} | Members only; select seat types and quantities |
| My Bookings | /Bookings/MyBookings | Members only; list of bookings |
| Submit Review | /Reviews/Create | Members only; rating and comment |
| Send Inquiry | /Inquiries/Create | Guests can send questions |
| Sign In | /Account/Login | Member login |
| Register | /Account/Register | New member registration |

#### 5.3 Member vs Guest Behavior

| Feature | Guest | Member |
|---------|-------|--------|
| Browse events | Yes (basic info) | Yes (full info) |
| Search events | Yes | Yes |
| Availability | Available/Full only | Full seat details (type, price, qty) |
| Book tickets | No (must register) | Yes |
| Submit reviews | No | Yes |
| Read reviews | Yes | Yes |
| Send inquiry | Yes | Yes |

---

### 6. Testing

#### 6.1 Functional Testing

| Test Case | Expected Result | Status |
|-----------|-----------------|--------|
| Home page loads | Council info and events displayed | ✓ |
| Guest browses events | Sees name, category, date, venue, Available/Full | ✓ |
| Guest reads reviews | Reviews visible on event details | ✓ |
| Guest registers | Can create account with preferences | ✓ |
| Member logs in | Redirected to home, session active | ✓ |
| Member searches events | Filters by category, date, city, price | ✓ |
| Member books tickets | Selects seat types, confirms booking | ✓ |
| Member submits review | Rating 1–5 and optional comment saved | ✓ |
| Guest sends inquiry | Message saved, confirmation shown | ✓ |

#### 6.2 Database Testing

- **Login**: Verify Member lookup by email and password hash (BCrypt)
- **Search**: Verify Event + Venue + EventSeats queries
- **Booking**: Verify Booking + BookingDetails inserts and EventSeats.AvailableSeats decrement
- **Review**: Verify Reviews insert with unique (MemberID, EventID)

---

### 7. Reflection and Conclusion

The Smart Event Management and Ticketing System meets the specified requirements for the Metropolitan Cultural Council. The database schema supports members, events, venues, seat types, bookings, reviews, and inquiries. The ASP.NET MVC application provides a clear separation between guests (limited view, inquiries) and members (full search, booking, reviews).

**Improvements for future work**:
- Payment integration (e.g., Stripe)
- Email notifications for bookings and inquiries
- Admin panel for event/venue management
- QR codes for ticket validation
