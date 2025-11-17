# Altec Printing Software

> A cost-effective, user-friendly label preview and printing solution built with C#, WPF, and Domain-Driven Design (DDD).

## About

**Altec Printing Software** is an in-house application designed to replace the existing reliance on costly, subscription-based commercial software (like Nicelabel) for label creation and printing.

The primary goal of this project is to create a **fast, cheaper, and feature-rich** alternative that addresses the gaps in usability and functionality present in proprietary systems. By leveraging modern C# and WPF, we aim to deliver a highly efficient and adaptable label printing workflow optimized for business needs.

---

## Key Features

This application focuses on core label printing workflow efficiency:

* **Real-time Label Preview:** Instant rendering of label designs directly within the WPF application.
* **High-Volume Printing:** Reliable functionality for printing generated labels.
* **Data Import:** Ability to import external data (e.g., from Excel) for variable label content.
* **Label Management (Future):** Planned functionality for organizing and maintaining label templates.

---

## Project Structure & Architecture

This project utilizes advanced architectural patterns to ensure scalability, maintainability, and domain clarity.

### Technologies

| Category | Detail |
| :--- | :--- |
| **Primary Language** | C# |
| **.NET Version** | `net8.0-windows` (LTS) |
| **UI Framework** | WPF with Fluent UI controls/styles |
| **Architecture** | MVVM (Model-View-ViewModel) |
| **Design Pattern** | Domain-Driven Design (DDD) |
| **Testing** | MSTest (Unit Testing Framework) |

---

## Getting Started

These instructions will get a copy of the project up and running on your local machine for development and testing purposes.

### Prerequisites

To successfully build and run this project, you need:

* **.NET 8.0 SDK** (LTS)
* A suitable IDE: **Visual Studio 2022** (recommended), Visual Studio Code with C# Dev Kit, or JetBrains Rider.

### Installation

1.  **Clone the repository:**
    ```bash
    git clone [https://github.com/xMelow/PrintSoftware](https://github.com/xMelow/PrintSoftware)
    ```
2.  **Navigate to the project directory:**
    ```bash
    cd PrintSoftware
    
    ```
3.  **Restore dependencies and build the solution:**
    ```bash
    dotnet build
    ```
4.  **Run the application (from the root directory):**
    ```bash
    dotnet run --project [Path to your startup .csproj file] 
    # Example: dotnet run --project src/Altec.PrintingSoftware.Presentation/Altec.PrintingSoftware.Presentation.csproj
    ```

---

## 💡 Usage

**TODO**

---

## 📄 License

This project is currently developed for internal use and is **Proprietary** (All Rights Reserved). Distribution, modification, and reproduction are strictly prohibited without written consent.

---

## 📧 Contact

Your Name - [@xMelow]

Project Link: [https://github.com/xMelow/PrintSoftware.git]
