# QuestBoard

An event RSVP app built with .NET MAUI (tested on the Windows target), plus a small
ASP.NET Core web service used to verify credentials with HTTP Basic Authentication.

## Running it

1. **Start `QuestBoardWebService` first** (Visual Studio, *IIS Express* profile, which serves
   `https://localhost:44372`). That address must match `BaseAddress` in
   `MAUI_QuestBoard/DataAccess/AuthWebService.cs`.
2. Run `MAUI_QuestBoard` with the **Windows Machine** target.

## Accounts for testing

| Account | Email | Password |
|---|---|---|
| Regular user | `fraham5822@students.ecpi.edu` | `Password1` |
| Administrator | `admin@example.com` | `admin123` |

You can also use **Create Account**, or **Continue as Guest** (nothing a guest does is
tied to an account).

## What the app does

- **Login** checks the email/password against the local SQLite database, then calls the web
  service with an `Authorization: Basic base64(email:password)` header. Both must succeed.
- After login a user lands on **My Adventures** (events they are attending). The tabs are
  **Quest Board** (all events), **My Adventures**, and **My Quests** (events they host).
- The administrator lands on the **Admin** page (also reachable from the Admin button on the
  Quest Board, which only the administrator sees). It shows where the SQLite database file is
  (full path, size, last modified), the tables and row counts, the connections the app has
  opened, and every registered account. *Copy Path* and *Open Folder* help find the file.
- **Add Event** uses date/time pickers and rejects events in the past, deadlines after the
  event, and a maximum attendee count below 1.
- **RSVP** is refused after the deadline, when the event is full, or if that email has
  already RSVP'd. Logged-in users get their details prefilled; guests type their own.
- Guests can browse and RSVP but cannot host events, and My Adventures / My Quests show a
  prompt to create an account instead of data.

## Design

The look follows the QuestBoard design document (color palette and typography sections).

| Color | Hex | Used for |
|---|---|---|
| Forest Green | `#344E41` | Primary buttons, navigation bar, headings |
| Moss Green | `#588157` | Secondary buttons, hover states |
| Parchment | `#F2E8CF` | Page background |
| Soft Cream | `#FFFDF5` | Cards and form backgrounds |
| Warm Brown | `#432818` | Text and borders |
| Tavern Gold | `#D4A72C` | Badges and the active tab |
| Muted Burgundy | `#8B3A3A` | Warnings and validation messages |

The app stays in light mode even if Windows is set to dark mode.


## Known simplifications

- Passwords are stored as plain text in the local SQLite database.
- The web service confirms a well-formed Basic Authentication header was sent; the password
  itself is verified against the local database before the service is called.
