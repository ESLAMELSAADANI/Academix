# 🚀 Academix  

Academix is a modern, centralized management system designed to streamline the operations of educational institutions. It provides efficient management for Departments, Students, Courses, Users, and Roles in a user-friendly web interface.

---

## 🧾 Overview  

The **Academix Management System** allows administrators to manage users, roles, courses, departments, and students with full authentication and role-based authorization features.  
It’s a perfect project for learning **ASP.NET Core MVC**, **Entity Framework Core**, and **Bootstrap integration**.  

---

## 🚀 Features  

✅ User Registration & Login  
✅ Secure Authentication using Cookies  
✅ Role-based Authorization (Admin / User / Student / Supervisor)  
✅ CRUD Operations for Departments, Students, Courses, Users, and Roles  
✅ Client-side & Server-side Validation  
✅ Modern, Responsive UI with Bootstrap  
✅ Entity Framework Core Integration  
✅ SQL Server Database  

---

## 🛠️ Tech Stack  

| Category | Technologies |
|-----------|--------------|
| **Backend** | ASP.NET Core MVC, C# |
| **Database** | Entity Framework Core, SQL Server |
| **Frontend** | HTML5, CSS3, Bootstrap 5 |
| **Authentication** | Cookie-based Authentication |
| **IDE** | Visual Studio 2022 Community |

---

## 📸 Screenshots  
<img width="1902" height="892" alt="image" src="https://github.com/user-attachments/assets/d385cc45-607d-4663-a99b-3733d73425c6" />
<img width="1903" height="895" alt="image" src="https://github.com/user-attachments/assets/6fce6f88-27b5-49ba-9e7f-574d417c8b24" />
<img width="1902" height="893" alt="image" src="https://github.com/user-attachments/assets/346949a8-b985-445b-a6a0-4d626b3fc9e8" />
<img width="1902" height="892" alt="image" src="https://github.com/user-attachments/assets/43f37d2f-2096-4332-8516-071acd23c5aa" />
<img width="1901" height="890" alt="image" src="https://github.com/user-attachments/assets/2848bc1a-5df5-4cc6-9981-138982fc31fe" />
<img width="1900" height="893" alt="image" src="https://github.com/user-attachments/assets/e535ce06-765c-4b37-98b0-e7ee8df61c14" />
<img width="1901" height="888" alt="image" src="https://github.com/user-attachments/assets/b82d59a6-9cd5-48d7-8436-e11f557b9337" />
<img width="1916" height="891" alt="image" src="https://github.com/user-attachments/assets/97e81fe3-64d0-44a9-87ad-20ef117aa830" />
<img width="1903" height="888" alt="image" src="https://github.com/user-attachments/assets/df62de15-c0d8-40b1-a1e1-5dc6e6936ecc" />
<img width="1919" height="883" alt="image" src="https://github.com/user-attachments/assets/257cb148-3d7e-405f-ada1-d93be078a098" />
<img width="1915" height="886" alt="image" src="https://github.com/user-attachments/assets/f7bea995-ecd9-47d5-a33b-9ff6f8eda9ef" />
<img width="1918" height="886" alt="image" src="https://github.com/user-attachments/assets/da5b0162-1463-475d-b906-f982bc0486bc" />
<img width="1903" height="885" alt="image" src="https://github.com/user-attachments/assets/626fbc50-ecdf-4ca5-b3a6-036aedeaf16a" />
<img width="1920" height="884" alt="image" src="https://github.com/user-attachments/assets/b97af15f-8596-4473-9aea-c89d6059bfa3" />
<img width="1904" height="897" alt="image" src="https://github.com/user-attachments/assets/c2757f26-6052-4952-8726-2bbdb2bb0f2a" />
<img width="1900" height="891" alt="image" src="https://github.com/user-attachments/assets/73c44254-8da3-4564-8fcc-980c9c0c3e01" />
<img width="1915" height="890" alt="image" src="https://github.com/user-attachments/assets/c3701cbc-d7c6-4377-8420-5e232aeeddd5" />
<img width="1901" height="891" alt="image" src="https://github.com/user-attachments/assets/d27217b7-7a82-4e6b-8278-38fe0d00ee79" />







---

## ⚙️ Installation & Setup  

Follow these steps to run the project locally 👇  

### 1️⃣ Clone the Repository  
```bash
git clone https://github.com/ESLAMELSAADANI/Academix.git
```

### 2️⃣ Open in Visual Studio 2022
Open the .sln file in Visual Studio.

### 3️⃣ Set up the Database
1- Update your connection string in appsettings.json.

2- Run the following commands in the Package Manager Console:
```bash
add-migration InitialCreate
update-database
```

### 4️⃣ Run the Project

Press Ctrl + F5 or click Run Without Debugging.
The app will open in your browser (default: https://localhost:xxxx).

## 🧩 Project Structure

Academix/
│
├── Controllers/
│   ├── AccountController.cs
│   ├── UserController.cs
│   ├── RoleController.cs
│   ├── DepartmentController.cs
│   ├── StudentController.cs
│   └── CourseController.cs
│
├── Models/
│   ├── User.cs
│   ├── Role.cs
│   ├── Department.cs
│   ├── Student.cs
│   ├── Course.cs
│   └── ViewModels/
│
├── Views/
│   ├── Account/
│   ├── User/
│   ├── Role/
│   ├── Department/
│   ├── Student/
│   ├── Course/
│   └── Shared/
│
├── Data/
│   └── ApplicationDbContext.cs
│
├── wwwroot/
│   ├── css/
│   ├── js/
│   └── lib/
│
└── appsettings.json

## 🧠 Learning Goals

This project was built to practice and understand:

✅ ASP.NET Core MVC architecture
✅ Authentication & Authorization
✅ Role-based Access Control
✅ Entity Framework Core and Migrations
✅ Razor Views and Model Binding
✅ Form Validation (Client & Server)
✅ Bootstrap Integration for Modern UI


### ⭐ If you found this project helpful, don’t forget to star the repo! 🌟
