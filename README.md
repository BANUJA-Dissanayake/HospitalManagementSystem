# Hospital Management System

CS107.3 – Object Oriented Programming with C# (NSBM) coursework project.

A desktop hospital management application built with **C# / WinForms** on **.NET Framework 4.7.2**, using **Entity Framework Core 3.1** over a self-contained **SQLite** database. Eight forms cover patient, doctor, staff, appointment, medical record and billing management, plus login and a main menu — one module per group member.

## Tech stack

- C# / Windows Forms, .NET Framework 4.7.2
- Entity Framework Core 3.1 (code-first, `Database.EnsureCreated()` — no manual migration step)
- SQLite (`Microsoft.Data.Sqlite`) — a single `HospitalManagement.db` file created automatically next to the `.exe` on first run
- NuGet (`packages.config`) for dependencies

## How to run

1. Open `HospitalManagementSystem.csproj` (or `HospitalManagementSystem.slnx`) in Visual Studio 2022+.
2. Let NuGet restore the packages (right-click the solution → *Restore NuGet Packages*, or just build — restore usually runs automatically).
3. Press **F5** (Debug ▸ Start). The database file is created automatically the first time the app runs — no SQL Server, LocalDB, or manual setup required.
4. Log in with the seeded default account:
   - **Username:** `admin`
   - **Password:** `admin123`

Each teammate can open the project on their own machine and it will "just work" — the SQLite file is local and self-contained, so there's nothing to install or configure for the demo/viva.

## Project structure

```
HospitalManagementSystem/
├── Models/       Person (abstract) → Patient, Doctor, Staff; Appointment; MedicalRecord;
│                 Invoice + IPaymentMethod (Cash/Card/Insurance); User; custom exceptions
├── Data/         HospitalContext (EF Core DbContext), one repository per entity,
│                 RepositoryBase<T> (generics), PasswordHasher, DbInitializer (seeding)
└── Forms/        LoginForm, MainMenuForm, PatientForm, DoctorForm, StaffForm,
                  AppointmentForm, MedicalRecordForm, BillingForm
```

## OOP concepts demonstrated

- **Abstraction & inheritance** — `Person` is an abstract base class; `Patient`, `Doctor`, `Staff` inherit from it.
- **Encapsulation** — private fields exposed through validated properties (e.g. `Name`, `ContactNumber` reject invalid input).
- **Polymorphism** — `PerformDuties()` is overridden per subclass; `IPaymentMethod` (`CashPayment`/`CardPayment`/`InsurancePayment`) is resolved and invoked at runtime without the caller knowing the concrete type.
- **Interfaces** — repository interfaces (`IPatientRepository`, etc.) and `IPaymentMethod`.
- **Generics** — `RepositoryBase<T>` shares CRUD plumbing across all seven repositories.
- **Exception handling** — custom exceptions (`DoctorUnavailableException`, `DuplicateRecordException`, `RecordNotFoundException`, `InvalidCredentialsException`) alongside built-in ones, all caught and shown to the user with a friendly message.
- **Database integration** — EF Core over SQLite with real foreign-key relationships (`Appointment → Patient/Doctor`, `MedicalRecord → Patient`, `Invoice → Patient/Appointment`).
