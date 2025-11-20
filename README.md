# DVLD - Driving License Management System

A full-featured desktop application built with C# .NET Framework and Windows Forms for managing a Driving and Vehicle License Department (DVLD). This system streamlines the process of issuing, renewing, and managing driver's licenses, handling various user services, and maintaining person and user records.

## 📖 Project Overview

This project is a complete implementation of the "Project 1" requirements for a Driving License Management system. It automates the core services of a DVLD, including new license issuance, renewals, replacement of lost/damaged licenses, and international license issuance. The system enforces business rules, manages application workflows, and maintains a comprehensive database of people, users, and licenses.

## ✨ Features

### Core Services
- **New Local Driving License Issuance**: Manages the complete process from application to issuance, including vision, written, and practical tests.
- **Retake Test Service**: Allows applicants to schedule retakes for failed tests.
- **License Renewal Service**: Handles the renewal of expired driving licenses.
- **Replacement for Lost License**: Issues a duplicate for a lost license.
- **Replacement for Damaged License**: Issues a duplicate for a damaged license.
- **International Driving Permit Issuance**: Issues international licenses for eligible applicants (Category 3 only).
- **License Detainment & Release**: Manages the detainment and release of licenses.

### System Management
- **People Management**: Add, edit, delete, and search for individuals using various filters such as National Number, ID, Name, etc..
- **Users Management**: Manage system users, link them to people.
- **Applications Management**: Track and filter all service applications by status, applicant, etc.
- **License Classes Management**: Configure license categories, fees, validity, and age requirements.
- **Tests Management**: Update fees for the three test types (Vision, Written, Practical).

## 🛠️ Technologies Used

- **Backend:** C#, .NET Framework
- **Frontend:** Windows Forms (WinForms)
- **Database:** Microsoft SQL Server
- **Version Control:** Git

## 🗄️ Database

The project includes a SQL Server database backup file (`.bak`). This file contains the complete database schema and any initial seed data required for the application to run.

### Restoring the Database
To run this project, you must restore the provided `.bak` file to your SQL Server instance. You can do this via SQL Server Management Studio (SSMS) or using T-SQL commands.

**Using SSMS:**
1. Open SQL Server Management Studio.
2. Connect to your target SQL Server instance.
3. Right-click on the **Databases** node.
4. Select **Restore Database...**.
5. Choose "Device" and browse to select your `.bak` file.
6. Click "OK" to restore the database.

## 🚀 Installation & Setup

Follow these steps to get the project running on your local machine:

1.  **Clone the Repository**
    ```bash
    git clone (https://github.com/rafadhil/DVLD.git)
    cd DVLD
    ```

2.  **Restore the Database**
    - Use the instructions above to restore the `DVLD.bak` file to your SQL Server. Note the name you give the restored database.

3.  **Update Connection String**
    - Open the solution in Visual Studio.
    - Navigate to the `App.config` file.
    - Locate the connection string and update it to match your local SQL Server environment.
    ```xml
	<connectionStrings>
		<add name ="MyDbConnection" connectionString= "Server=.;Database=DVLD;User Id=sa;Password=sa123456;"/>
	</connectionStrings>
    ```

4.  **Build and Run**
    - Build the solution in Visual Studio to restore any NuGet packages.
    - Run the project. The application should start, and you can log in with the default user credentials (if any were set up in the database).

## 📋 License Classes

The system supports 7 license categories:

| Category | Description | Minimum Age | Fees | Validity |
|----------|-------------|-------------|------|----------|
| 1 | Small Motorcycles | 18 | $15 | 5 years |
| 2 | Heavy Motorcycles | 21 | $30 | 5 years |
| 3 | Regular Car License | 18 | $20 | 10 years |
| 4 | Commercial (Taxi/Limousine) | 21 | $200 | 10 years |
| 5 | Agricultural | 21 | $50 | 10 years |
| 6 | Small/Medium Buses | 21 | $250 | 10 years |
| 7 | Trucks & Heavy Vehicles | 21 | $300 | 10 years |

## 🧪 Testing Process

All new license applications require three sequential tests:
1. **Vision Test** - $10 fee
2. **Written Test** - $20 fee  
3. **Practical Test** - $30 fee
The information of these tests can be modified within the application


## 👨‍💻 Developer

- **Rayan Fadhil** - *Initial work* - https://github.com/rafadhil

This project was developed as part of the curriculum from ProgrammingAdvices.com (https://programmingadvices.com).

---
**Note:** This project is for educational and portfolio purposes. The requirements and design are based on the DVLD Project 1 specifications from ProgrammingAdvices.com.
