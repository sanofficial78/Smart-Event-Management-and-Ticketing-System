-- =============================================================================
-- SAMPLE QUERIES - Smart Event Management and Ticketing System
-- =============================================================================

-- 1. Upcoming events (next 90 days)
SELECT e.Title, e.Category, e.EventDate, v.Name AS VenueName, v.City, e.BasePrice
FROM Events e
JOIN Venues v ON e.VenueID = v.VenueID
WHERE e.EventDate >= GETDATE() 
  AND e.EventDate <= DATEADD(DAY, 90, GETDATE())
  AND e.IsActive = 1
ORDER BY e.EventDate;

-- 2. Events by category
SELECT e.Title, e.EventDate, v.Name AS Venue
FROM Events e
JOIN Venues v ON e.VenueID = v.VenueID
WHERE e.Category = 'Music'
  AND e.IsActive = 1
ORDER BY e.EventDate;

-- 3. Events by date range
SELECT e.Title, e.Category, e.EventDate, v.Name AS Venue
FROM Events e
JOIN Venues v ON e.VenueID = v.VenueID
WHERE e.EventDate BETWEEN '2025-06-01' AND '2025-06-30'
  AND e.IsActive = 1;

-- 4. Events by location (city)
SELECT e.Title, e.Category, e.EventDate
FROM Events e
JOIN Venues v ON e.VenueID = v.VenueID
WHERE v.City = 'Metro City'
  AND e.IsActive = 1;

-- 5. Events by price range
SELECT e.Title, e.Category, e.BasePrice, e.EventDate
FROM Events e
WHERE e.BasePrice BETWEEN 20 AND 50
  AND e.IsActive = 1
ORDER BY e.BasePrice;

-- 6. Booked tickets for a member
SELECT b.BookingID, e.Title, b.BookingDate, b.Status, b.TotalAmount,
       bd.SeatType, bd.Quantity, bd.SubTotal
FROM Bookings b
JOIN Events e ON b.EventID = e.EventID
JOIN BookingDetails bd ON b.BookingID = bd.BookingID
WHERE b.MemberID = 1
ORDER BY b.BookingDate DESC;

-- 7. Ticket availability (for guests - Available/Full view)
SELECT e.Title, es.SeatType, 
       CASE WHEN es.AvailableSeats > 0 THEN 'Available' ELSE 'Full' END AS Availability,
       es.AvailableSeats
FROM Events e
JOIN EventSeats es ON e.EventID = es.EventID
WHERE e.IsActive = 1
  AND e.EventDate >= GETDATE();

-- 8. Event reviews (guests can read)
SELECT e.Title, m.FirstName + ' ' + m.LastName AS Reviewer, r.Rating, r.Comment, r.ReviewDate
FROM Reviews r
JOIN Events e ON r.EventID = e.EventID
JOIN Members m ON r.MemberID = m.MemberID
WHERE e.EventID = 2
ORDER BY r.ReviewDate DESC;

-- 9. Average rating per event
SELECT e.Title, AVG(r.Rating) AS AvgRating, COUNT(r.ReviewID) AS ReviewCount
FROM Events e
LEFT JOIN Reviews r ON e.EventID = r.EventID
WHERE e.IsActive = 1
GROUP BY e.EventID, e.Title;

-- 10. Member's event preferences
SELECT m.FirstName, m.LastName, m.Email,
       CASE WHEN m.PreferredMusic = 1 THEN 'Music ' ELSE '' END +
       CASE WHEN m.PreferredTheatre = 1 THEN 'Theatre ' ELSE '' END +
       CASE WHEN m.PreferredSports = 1 THEN 'Sports ' ELSE '' END +
       CASE WHEN m.PreferredArts = 1 THEN 'Arts ' ELSE '' END +
       CASE WHEN m.PreferredWorkshops = 1 THEN 'Workshops ' ELSE '' END +
       CASE WHEN m.PreferredExhibitions = 1 THEN 'Exhibitions' ELSE '' END AS Preferences
FROM Members m;
