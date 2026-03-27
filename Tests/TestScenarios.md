# Testing Scenarios - Smart Event Management and Ticketing System

## 1. Login Testing

| Step | Action | Expected Result |
|------|--------|-----------------|
| 1 | Navigate to /Account/Login | Login form displayed |
| 2 | Enter valid email and password | Redirect to home, session active |
| 3 | Enter invalid credentials | Error message "Invalid email or password" |
| 4 | Sign Out | Redirect to home, session cleared |

## 2. Registration Testing

| Step | Action | Expected Result |
|------|--------|-----------------|
| 1 | Navigate to /Account/Register | Registration form with preferences |
| 2 | Submit with mismatched passwords | Validation error on ConfirmPassword |
| 3 | Submit with existing email | "Email already registered" message |
| 4 | Submit valid data | Redirect to login, success message |

## 3. Event Search Testing

| Step | Action | Expected Result |
|------|--------|-----------------|
| 1 | As guest, go to /Events | Table with name, category, date, venue, price, Availability |
| 2 | Filter by category "Music" | Only Music events shown |
| 3 | Filter by date range | Events within range |
| 4 | Filter by city | Events at venues in that city |
| 5 | Filter by price range | Events within min/max price |

## 4. Booking Testing

| Step | Action | Expected Result |
|------|--------|-----------------|
| 1 | As guest, click Book on event | Redirect to login |
| 2 | As member, go to /Bookings/Book/1 | Form with seat types and quantity inputs |
| 3 | Select quantities and confirm | Booking created, redirect to My Bookings |
| 4 | Verify My Bookings | New booking listed with details |
| 5 | Attempt overbooking | Error "Not enough X seats available" |

## 5. Review Testing

| Step | Action | Expected Result |
|------|--------|-----------------|
| 1 | As guest, try /Reviews/Create?eventId=1 | Redirect to login |
| 2 | As member, submit review (rating 1-5) | Review saved, redirect to event details |
| 3 | Submit second review for same event | "You have already reviewed" message |
| 4 | As guest, view event details | Reviews visible |

## 6. Inquiry Testing

| Step | Action | Expected Result |
|------|--------|-----------------|
| 1 | Go to /Inquiries/Create | Form for name, email, subject, message |
| 2 | Submit valid inquiry | Success message, form cleared |
| 3 | Submit with empty required fields | Validation errors |

## 7. Database Queries (SQL)

Run `Database/Queries.sql` to verify:

- Upcoming events
- Events by category/date/location/price
- Member bookings
- Ticket availability (guest view)
- Event reviews
- Average ratings
