# 🍔 FoodExpress - Food Delivery Desktop App

A modern, responsive desktop food delivery and order management system built with **WPF (Windows Presentation Foundation)** and **C# (.NET Framework 4.7.2)** using **SQLite** for local database persistence.

---

## 🚀 Features

- **User Authentication**: Role-based access control with Customer and Admin portals.
- **Dynamic Category & Product Browsing**: Explore Burgers, Pizzas, Wings, Salads, Pasta, Milkshakes, and more with customized sizes/variations.
- **Custom Data Structures**: Uses custom Linked List and Queue implementations for fast in-memory cart management and order fulfillment.
- **Shopping Cart & Checkout**: Real-time badge counter, quantity adjustment, and order submission.
- **Admin Dashboard**: Real-time FIFO order queue management for order processing.
- **Local SQLite Database**: Auto-initializes tables and seeds initial menu and sample accounts on first run.

---

## 🔑 Default Login Credentials

| Role | Username | Password | Notes |
| :--- | :--- | :--- | :--- |
| **Customer** | `customer1` | `password1` | Browse menu, customize items, place orders |
| **Admin** | `admin` | `admin123` | View order queue and process orders |

*You can also click **Sign Up** on the login screen to register a new customer account.*

---

## 🛠️ How to Run

### Method 1: Run Pre-built Executable
1. Navigate to:
   ```
   FoodDeliveryApp\bin\Debug\FoodDeliveryApp.exe
   ```
2. Double-click `FoodDeliveryApp.exe` to launch the application.
3. *(Optional)* Use the `FoodDeliveryApp.lnk` shortcut in the project root folder.

### Method 2: Open in Visual Studio
1. Open `FoodDeliveryApp.sln` in **Visual Studio** (2019 / 2022 / 2026).
2. If prompted, restore NuGet packages (or run `nuget.exe restore FoodDeliveryApp.sln`).
3. Set the build configuration to **Debug** and platform to **Any CPU** (or x64/x86).
4. Press **F5** or click the **Start** button to run.

---

## 📁 Project Architecture

```
FoodDeliveryApp/
├── FoodDeliveryApp/
│   ├── Assets/             # App icons, logos (app.png, app.ico), and 50+ food pictures
│   ├── Converters/         # WPF value converters (e.g. ImagePathConverter)
│   ├── Data/               # SQLite DatabaseHelper and schema seeding
│   ├── DataStructures/     # Custom LinkedList & Custom Queue implementations
│   ├── Models/             # User, Product, Variation, CartItem, Order entities
│   ├── Services/           # AuthService, ProductService, CartService, OrderService
│   ├── Utils/              # Helper utilities
│   ├── Views/              # XAML Windows & Pages (Login, SignUp, MainWindow, etc.)
│   ├── App.xaml / .cs      # Application startup and shared global resource dictionary
│   └── FoodDeliveryApp.csproj
├── packages/               # External assemblies (System.Data.SQLite, EntityFramework)
├── nuget.exe               # NuGet command line tool
└── FoodDeliveryApp.sln     # Visual Studio Solution
```

---

## 🧰 Tech Stack
- **Framework**: .NET Framework 4.7.2
- **UI**: WPF (XAML)
- **Language**: C#
- **Database**: SQLite (`System.Data.SQLite`)
