# Room-Scheduling-Software

## Author
[Juan Pablo Gómez Haro Cabrera](https://github.com/JuanPabloGHC)

## Description
This software simulates a place to play like [ARENA](https://gamersarena.com.mx/). You have many rooms to play different games or consoles.

* Categories
  * Computers
  * Consoles
  * Virtual Reality...

In that way you can create one o more Room for each category and define its people capacity, price per hour, availability, etc.

In addition, to book a room, you have to create a user. In this MVP version, the user information is just the name, email and the number o visits, in the future you can add payment methods, discounts, etc.

## Technologies
* [C#](https://learn.microsoft.com/en-us/dotnet/csharp/)
* [.NET MAUI](https://dotnet.microsoft.com/en-us/apps/maui)
* [Entity Framework](https://learn.microsoft.com/es-es/ef/)
* [SQL SERVER](https://www.microsoft.com/es-MX/sql-server)

## Tools
[Colors Generator](https://uicolors.app/create)

* Blue: #3228c3
* Brown: #c8401e
* Purple: #6e28c3

[.NET Community Toolkit](https://learn.microsoft.com/en-us/dotnet/communitytoolkit/introduction)
* Popup

## Entities

### User

* SQL
  
|          Id         |         Name         |        Email         |   Number_Visits  |
|---------------------|----------------------|----------------------|------------------|
| 🔑INTEGER NOT NULL | VARCHAR(50) NOT NULL | VARCHAR(50) NOT NULL | INTEGER NOT NULL |

* Code

```C#
Class User {
  [Key]
  int Id { get; set; }
  string Name { get; set; }
  string Email { get; set; }
  int Number_Visits { get; set; }
}
```

### Category

* SQL
  
|          Id         |         Name         |           Photo         |
|---------------------|----------------------|-------------------------|
| 🔑INTEGER NOT NULL | VARCHAR(20) NOT NULL | VARBINARY(MAX) NOT NULL |

* Code

```C#
Class Category {
  [Key]
  int Id { get; set; }
  string Name { get; set; }
  byte[] Photo { get; set; }
  ICollection<Room> Rooms { get; set; }
}
```

### Room

* SQL
  
|          Id         |         Name         |     Capacity     |       Hourly_Price     |    IsFree    |      CategoryId     |
|---------------------|----------------------|------------------|------------------------|--------------|---------------------|
| 🔑INTEGER NOT NULL | VARCHAR(20) NOT NULL | INTEGER NOT NULL | DECIMAL(6, 2) NOT NULL | BIT NOT NULL | FK INTEGER NOT NULL |

* Code

```C#
Class Room {
  [Key]
  int Id { get; set; }
  string Name { get; set; }
  int Capacity { get; set; }
  decimal Hourly_Price { get; set; }
  bool IsFree { get; set; } = true;
  int CategoryId { get; set; }
  [ForeignKey("CategoryId")]
  Category Category { get; set; }
}
```

### Appointment

* SQL
  
|          Id         |        UserId       |        RoomId       |      Start_Hour   |      End_Hour     |          Price         |
|---------------------|---------------------|---------------------|-------------------|-------------------|------------------------|
| 🔑INTEGER NOT NULL | FK INTEGER NOT NULL | FK INTEGER NOT NULL | DATETIME NOT NULL | DATETIME NOT NULL | DECIMAL(6, 2) NOT NULL |

* Code

```C#
Class Appointment {
  [Key]
  int Id { get; set; }
  int UserId { get; set; }
  [ForeignKey("UserId")]
  User User { get; set; }
  int RoomId { get; set; }
  [ForeignKey("RoomId")]
  Room Room { get; set; }
  DateTime Start_Hour { get; set; }
  DateTime End_Hour { get; set; }
  decimal Price { get; set; }
}
```

## SQL commands

### Create DB
```SQL
CREATE DATABASE <DATABASE_AME>;
```

### Create Users Table
```SQL
CREATE TABLE Users (
	Id INTEGER PRIMARY KEY IDENTITY(1, 1) not null,
	Name VARCHAR(50) not null,
	Email VARCHAR(20) not null,
	Number_Visits INTEGER DEFAULT 0 not null
)
```

### Create Categories Table
```SQL
CREATE TABLE Categories (
	Id INTEGER PRIMARY KEY IDENTITY(1, 1) not null,
	Name VARCHAR(20) not null,
	Photo VARBINARY(MAX) not null
)
```

### Create Rooms Table
```SQL
CREATE TABLE Rooms (
	Id INTEGER PRIMARY KEY IDENTITY(1, 1) not null,
	Name VARCHAR(20) not null,
	Capacity INTEGER not null,
	Hourly_Price DECIMAL(6, 2) not null,
	IsFree BIT not null,
	CategoryId INTEGER not null,

	FOREIGN KEY(CategoryId) REFERENCES Categories(Id)
)
```

### Create Appointments Table
```SQL
CREATE TABLE Appointments (
	Id INTEGER PRIMARY KEY IDENTITY(1, 1) not null,
	UserId INTEGER not null,
	RoomId INTEGER not null,
	Start_Hour DATETIME not null,
	End_Hour DATETIME not null,
	Price DECIMAL(6, 2) not null

	FOREIGN KEY(UserId) REFERENCES Users(Id),
	FOREIGN KEY(RoomId) REFERENCES Rooms(Id)
)
```

## Connect Entity Framework to the DB
```C#
class Context : DbContext {
    public DbSet<User> Users { get; set; }

    public DbSet<Room> Rooms { get; set; }

    public DbSet<Category> Categories { get; set; }

    public DbSet<Appointment> Appointments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=<HOST>;Database=<BASEDATOS>;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true");
    }
}
```

## Architecture

```plain
└──📁/Room-Scheduling-Fostware
   ├──📁/Data
   │   ├──📁/Entities
   │   │  ├──📄Appointment.cs
   │   │  │──📄Category.cs
   │   │  │──📄Room.cs
   │   │  └──📄User.cs
   │   └──📄Context.cs
   │
   ├──📁/Pages
   │  ├──📄Admin.xaml
   │  |   └──📄Admin.xaml.cs
   │  └──📄Home.xaml
   │      └──📄Home.xaml.cs
   |
   ├──📁/Views
   │  ├──📄NewAppointment.xaml
   │  |   └──📄NewAppointment.xaml.cs
   │  ├──📄NewCategory.xaml
   │  |   └──📄NewCategory.xaml.cs
   │  ├──📄NewRoom.xaml
   │  |   └──📄NewRoom.xaml.cs
   │  └──📄NewUser.xaml
   │      └──📄NewUser.xaml.cs
   |
   ├──📄App.xaml
   |   └──📄App.xaml.cs
   ├──📄AppShell.xaml
   |   └──📄AppShell.xaml.cs
   ├──📄MainPage.xaml
   |   └──📄MainPage.xaml.cs
   └──📄MauiProgram.cs
```

* Folder Data: Save the structures of the tables and the connection to the database.
* Folder Pages: Page Home to manage the Appointments and Admin to manage the Rooms and Categories.
* Folder Views: To create the Popups views.
* App: To charge the sources.
* AppShell: To create the tabbed page.
* MainPage: It is by default, you can add the LogIn part to add the authentification component.
* MauiProgram: To build the app, including Maui framework, community toolkit component and connection to the database.

* .xaml: The page structure.
* .xaml.cs: The page logic .
