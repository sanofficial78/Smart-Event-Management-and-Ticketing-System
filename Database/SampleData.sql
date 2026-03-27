-- =============================================================================
-- SAMPLE DATA - Smart Event Management and Ticketing System
-- =============================================================================

-- Venues
INSERT INTO Venues (Name, Address, City, Capacity, Description) VALUES
('Metropolitan Concert Hall', '123 Arts Boulevard', 'Metro City', 2000, 'Premier concert venue with excellent acoustics'),
('Community Theatre', '456 Drama Street', 'Metro City', 500, 'Intimate theatre for plays and performances'),
('City Exhibition Center', '789 Gallery Way', 'Metro City', 1000, 'Spacious galleries for exhibitions'),
('Workshop Studio', '321 Creative Lane', 'Metro City', 50, 'Hands-on workshop space');

-- Events
INSERT INTO Events (Title, Description, Category, EventDate, VenueID, BasePrice, IsActive) VALUES
('Summer Jazz Festival 2025', 'Three days of jazz performances from local and international artists.', 'Music', '2025-07-15 19:00:00', 1, 45.00, 1),
('Romeo and Juliet', 'Classic Shakespeare play performed by the Metropolitan Theatre Company.', 'Theatre', '2025-06-20 20:00:00', 2, 35.00, 1),
('Modern Art Exhibition', 'Contemporary art from emerging artists across the region.', 'Exhibition', '2025-05-01 10:00:00', 3, 15.00, 1),
('Pottery Making Workshop', 'Learn the basics of pottery from professional ceramists.', 'Workshop', '2025-06-10 14:00:00', 4, 75.00, 1),
('Metro City Marathon', 'Annual charity marathon through city center.', 'Sports', '2025-08-15 07:00:00', 1, 25.00, 1),
('Symphony Orchestra Night', 'Beethoven and Mozart performed by the City Symphony.', 'Music', '2025-06-25 19:30:00', 1, 55.00, 1),
('Photography Exhibition', 'Landscape and portrait photography by local artists.', 'Exhibition', '2025-07-01 09:00:00', 3, 10.00, 1);

-- Event Seats (for ticketed events)
INSERT INTO EventSeats (EventID, SeatType, Price, TotalSeats, AvailableSeats) VALUES
(1, 'VIP', 120.00, 100, 85),
(1, 'Standard', 45.00, 1500, 1200),
(1, 'Balcony', 30.00, 400, 350),
(2, 'VIP', 80.00, 50, 40),
(2, 'Standard', 35.00, 350, 280),
(2, 'Balcony', 25.00, 100, 90),
(3, 'General', 15.00, 500, 450),
(4, 'Workshop', 75.00, 30, 22),
(5, 'Participant', 25.00, 5000, 4200),
(6, 'VIP', 100.00, 80, 65),
(6, 'Standard', 55.00, 1500, 1300),
(6, 'Balcony', 40.00, 420, 380),
(7, 'General', 10.00, 300, 275);

-- Members (password: Member123! - hashed with BCrypt)
INSERT INTO Members (Email, PasswordHash, FirstName, LastName, Phone, PreferredMusic, PreferredTheatre, PreferredArts) VALUES
('john.doe@email.com', '$2a$11$XQZ6H5Y8Y3N5R2V1L4K3.OqP9wR2xY5zA6bC7dE8fG9hI0jK1lM2n', 'John', 'Doe', '555-0101', 1, 1, 1),
('jane.smith@email.com', '$2a$11$XQZ6H5Y8Y3N5R2V1L4K3.OqP9wR2xY5zA6bC7dE8fG9hI0jK1lM2n', 'Jane', 'Smith', '555-0102', 1, 0, 1),
('bob.wilson@email.com', '$2a$11$XQZ6H5Y8Y3N5R2V1L4K3.OqP9wR2xY5zA6bC7dE8fG9hI0jK1lM2n', 'Bob', 'Wilson', '555-0103', 0, 1, 0);

-- Bookings
INSERT INTO Bookings (MemberID, EventID, Status, TotalAmount) VALUES
(1, 1, 'Confirmed', 90.00),
(1, 2, 'Confirmed', 70.00),
(2, 1, 'Confirmed', 120.00),
(2, 6, 'Confirmed', 110.00),
(3, 2, 'Completed', 35.00);

-- Booking Details
INSERT INTO BookingDetails (BookingID, SeatType, Quantity, UnitPrice, SubTotal) VALUES
(1, 'Standard', 2, 45.00, 90.00),
(2, 'Standard', 2, 35.00, 70.00),
(3, 'VIP', 1, 120.00, 120.00),
(4, 'Standard', 2, 55.00, 110.00),
(5, 'Standard', 1, 35.00, 35.00);

-- Reviews
INSERT INTO Reviews (MemberID, EventID, Rating, Comment) VALUES
(3, 2, 5, 'Outstanding performance! The actors brought Shakespeare to life.'),
(1, 1, 4, 'Great jazz festival. Would have liked more variety in the lineup.'),
(2, 6, 5, 'The symphony was breathtaking. A must-see for classical music lovers.');

-- Inquiries (from guests)
INSERT INTO Inquiries (GuestName, GuestEmail, Subject, Message, IsResolved) VALUES
('Alice Guest', 'alice@email.com', 'Membership Information', 'How do I register as a member? What benefits do I get?', 1),
('Charlie Visitor', 'charlie@email.com', 'Event Availability', 'Is the Summer Jazz Festival sold out?', 0);
