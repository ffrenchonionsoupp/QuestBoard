# QuestBoardWebService

A minimal ASP.NET Core Web API providing the Basic Authentication
verification endpoint required by the QuestBoard project rubric
("Verify User Credentials using a Web Service using the Basic
Authentication schema").

## Why this API doesn't check real passwords

The MAUI app's local SQLite database is the only place that actually has
every registered user's email/password (`UserData.ValidateUserAsync`
checks that). This API has no access to that database and isn't meant
to duplicate it. Its `BasicAuthentication` filter (`Controllers/BasicAuthentication.cs`)
just confirms that a well-formed `Authorization: Basic base64(email:password)`
header was sent - it demonstrates the required Basic Auth technique without
pretending to be a second source of truth for passwords.

`LoginViewModel` in the MAUI app calls both: the local database check first
(the real gate), then this web service (the required technique) - both must
succeed for login to complete.

## Running it

1. Open this project in Visual Studio (or `dotnet run` from this folder).
2. It listens on `http://localhost:5080` (set in `Properties/launchSettings.json`)
   - this must stay in sync with the URL in the MAUI app's
     `DataAccess/AuthWebService.cs`.
3. Leave it running while you test the MAUI app's login screen.
4. You can browse to `http://localhost:5080/swagger` to see/test the
   `GET api/auth/verify` endpoint directly.

## Testing on an Android emulator

The Android emulator can't reach your dev machine via `localhost` - it has
its own loopback address, `10.0.2.2`. `AuthWebService.cs` already handles
this with a compile-time check (`#if ANDROID`), so no changes are needed
there, but the API still needs to be running on your dev machine while the
emulator is testing.

## For your screen captures

Have this project running in one window and the MAUI app running in
another so the login call actually succeeds end-to-end.
