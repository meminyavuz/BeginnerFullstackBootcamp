# Library Management System

A Library Management System built using ASP.NET Core MVC, Entity Framework Core, and a SQLite database. The system manages users, books, and borrowing operations in a library.

## Features

- **User Management:**
  - Add, update, and delete users.
  - Display list of users with details.
  - Error handling when trying to delete users with borrowed books.
  
- **Book Management:**
  - Add, update, and delete books.
  - Error handling when trying to delete or update books that are currently borrowed.

- **Borrowing System:**
  - Users can borrow available books.
  - Users can return borrowed books.
  - Display a list of books borrowed by each user.
  
- **Database:**
  - Entity Framework Core used for database interaction.
  - SQLite is used as the database.
  - Relationships between `Users`, `Books`, and `Borrows` tables to manage borrowing transactions.

## Prerequisites

- .NET 6.0 or later
- SQLite (for database)
- Visual Studio or Visual Studio Code

## Example Screenshots

<img width="1710" alt="Ekran Resmi 2025-02-05 05 49 18" src="https://github.com/user-attachments/assets/8d8c015e-c765-4f2e-9e70-89f722e8fc50" />

<img width="1710" alt="Ekran Resmi 2025-02-05 05 44 25" src="https://github.com/user-attachments/assets/8dda23a3-4c5d-474e-947e-02614888a0e2" />

<img width="1710" alt="Ekran Resmi 2025-02-05 05 50 41" src="https://github.com/user-attachments/assets/527eeae1-1ed6-4a8a-b2ec-c4f950511e1b" />

<img width="1710" alt="Ekran Resmi 2025-02-05 05 45 10" src="https://github.com/user-attachments/assets/44013a78-f1d7-405b-ad06-1ba82d426629" />

<img width="1710" alt="Ekran Resmi 2025-02-05 05 45 29" src="https://github.com/user-attachments/assets/72535a51-e55b-43bb-93d1-b83251223c76" />

<img width="1710" alt="Ekran Resmi 2025-02-05 05 44 51" src="https://github.com/user-attachments/assets/468a49e7-c630-4606-bf00-3fff80a387be" />


