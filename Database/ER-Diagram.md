# ER Diagram - Smart Event Management and Ticketing System

## Entity Relationship Diagram (Mermaid)

```mermaid
erDiagram
    Members ||--o{ Bookings : "makes"
    Members ||--o{ Reviews : "submits"
    Events ||--o{ Bookings : "has"
    Events ||--o{ EventSeats : "has"
    Events ||--o{ Reviews : "receives"
    Venues ||--o{ Events : "hosts"
    Bookings ||--o{ BookingDetails : "contains"

    Members {
        int MemberID PK
        nvarchar Email UK
        nvarchar PasswordHash
        nvarchar FirstName
        nvarchar LastName
        nvarchar Phone
        nvarchar Address
        bit PreferredMusic
        bit PreferredTheatre
        bit PreferredSports
        bit PreferredArts
        bit PreferredWorkshops
        bit PreferredExhibitions
        datetime CreatedAt
        bit IsActive
    }

    Venues {
        int VenueID PK
        nvarchar Name
        nvarchar Address
        nvarchar City
        int Capacity
        nvarchar Description
    }

    Events {
        int EventID PK
        nvarchar Title
        nvarchar Description
        nvarchar Category
        datetime EventDate
        int VenueID FK
        decimal BasePrice
        nvarchar ImageUrl
        bit IsActive
        datetime CreatedAt
    }

    EventSeats {
        int EventSeatID PK
        int EventID FK
        nvarchar SeatType
        decimal Price
        int TotalSeats
        int AvailableSeats
    }

    Bookings {
        int BookingID PK
        int MemberID FK
        int EventID FK
        datetime BookingDate
        nvarchar Status
        decimal TotalAmount
    }

    BookingDetails {
        int BookingDetailID PK
        int BookingID FK
        nvarchar SeatType
        int Quantity
        decimal UnitPrice
        decimal SubTotal
    }

    Reviews {
        int ReviewID PK
        int MemberID FK
        int EventID FK
        int Rating
        nvarchar Comment
        datetime ReviewDate
    }

    Inquiries {
        int InquiryID PK
        nvarchar GuestName
        nvarchar GuestEmail
        nvarchar Subject
        nvarchar Message
        datetime InquiryDate
        bit IsResolved
    }
```

## Relationship Summary

| Relationship | Cardinality | Description |
|--------------|-------------|-------------|
| Member - Booking | 1:N | A member can make many bookings |
| Member - Review | 1:N | A member can submit many reviews |
| Event - Booking | 1:N | An event can have many bookings |
| Event - EventSeats | 1:N | An event has multiple seat types |
| Event - Review | 1:N | An event can receive many reviews |
| Venue - Event | 1:N | A venue hosts many events |
| Booking - BookingDetails | 1:N | A booking contains multiple ticket lines |

## SQL Developer Data Modeler Notes

To convert this design in Oracle SQL Developer Data Modeler:

1. **Create Logical Model**: Create entities for Members, Venues, Events, EventSeats, Bookings, BookingDetails, Reviews, and Inquiries
2. **Define Attributes**: Use the Data Dictionary for exact attribute definitions
3. **Create Relationships**: Draw relationships with correct cardinality (1:N)
4. **Generate Physical Model**: Choose SQL Server as target
5. **Generate DDL**: Export to SQL Server compatible scripts
