# 🌸 FlowerShop - E-Commerce Web Application

A full-featured e-commerce web application for a flower shop built with ASP.NET Core MVC, Entity Framework Core, and SQL Server.

## ✨ Features

- **User Authentication & Authorization**
  - User registration and login with ASP.NET Identity
  - Role-based access control (Admin and Client roles)
  - Secure password management

- **Product Management**
  - Browse flower products
  - Product search and filtering
  - Detailed product views

- **Shopping Cart**
  - Add/remove items from cart
  - Session-based cart management
  - Real-time cart updates

- **Order Management**
  - Secure checkout process
  - Order history tracking
  - Order details view

- **Admin Panel**
  - Product CRUD operations
  - Order management
  - User management

- **Customer Features**
  - User profile management
  - Feedback system
  - Order tracking

- **Security Features**
  - CSRF protection with anti-forgery tokens
  - XSS protection headers
  - Content Security Policy
  - Clickjacking protection
  - Secure cookie handling

## 🛠️ Technologies Used

- **Framework**: ASP.NET Core 10.0 MVC
- **Database**: SQL Server (LocalDB)
- **ORM**: Entity Framework Core 10.0
- **Authentication**: ASP.NET Core Identity
- **Frontend**: Razor Views, HTML, CSS, JavaScript
- **Session Management**: Distributed Memory Cache

## 📋 Prerequisites

Before running this application, ensure you have the following installed:

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) or later
- [SQL Server LocalDB](https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb) or SQL Server
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/)
- Git (for cloning the repository)

## 🚀 Getting Started

### 1. Clone the Repository

```bash
git clone <repository-url>
cd FlowerShop
```

### 2. Configure Database Connection

The application uses SQL Server LocalDB by default. If you want to use a different SQL Server instance, update the connection string in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=FlowerShopDB;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

### 3. Install Dependencies

Navigate to the project directory and restore NuGet packages:

```bash
cd FlowerShop
dotnet restore
```

### 4. Apply Database Migrations

Create and update the database with Entity Framework migrations:

```bash
dotnet ef database update
```

If migrations don't exist, create them first:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### 5. Run the Application

Start the application using:

```bash
dotnet run
```

Or press `F5` in Visual Studio to run with debugging.

### 6. Access the Application

Open your browser and navigate to:
- **HTTPS**: `https://localhost:5001`
- **HTTP**: `http://localhost:5000`

(Port numbers may vary - check the console output)

## 👤 Default Admin Account

The application automatically creates a default admin account on first run:

- **Email**: `admin@flowershop.com`
- **Password**: `Admin123!`

**⚠️ Important**: Change this password immediately after first login in production environments!

## 📁 Project Structure

```
FlowerShop/
├── Controllers/          # MVC Controllers
│   ├── AccountController.cs
│   ├── AdminController.cs
│   ├── CartController.cs
│   ├── ClientController.cs
│   ├── FeedbackController.cs
│   ├── HomeController.cs
│   ├── OrderController.cs
│   └── ShopController.cs
├── Models/              # Data models and view models
│   ├── Client.cs
│   ├── Product.cs
│   ├── Order.cs
│   ├── OrderDetail.cs
│   ├── CartItem.cs
│   └── ...
├── Views/               # Razor views
├── Data/                # Database context
├── Migrations/          # EF Core migrations
├── wwwroot/            # Static files (CSS, JS, images)
├── Helpers/            # Helper classes
├── Attributes/         # Custom attributes
└── Program.cs          # Application entry point
```

## 🔐 User Roles

### Admin
- Full access to admin panel
- Manage products (Create, Read, Update, Delete)
- View and manage all orders
- View customer feedback

### Client (Regular User)
- Browse products
- Add items to cart
- Place orders
- View order history
- Update profile
- Submit feedback

## 🛒 Usage Guide

### For Customers

1. **Register an Account**
   - Click "Register" in the navigation menu
   - Fill in your details (First Name, Email, Password, Age, Gender)
   - Submit the registration form

2. **Browse Products**
   - Navigate to the "Shop" section
   - Browse available flower products
   - Use search and filters to find specific items

3. **Add to Cart**
   - Click on a product to view details
   - Select quantity and click "Add to Cart"
   - View cart by clicking the cart icon

4. **Checkout**
   - Review items in your cart
   - Proceed to checkout
   - Enter delivery information
   - Confirm your order

5. **Track Orders**
   - Go to "My Orders" in your profile
   - View order status and details
   - Check order history

### For Administrators

1. **Login as Admin**
   - Use admin credentials to login
   - Access the Admin Panel from the navigation menu

2. **Manage Products**
   - Add new flower products
   - Edit existing product details
   - Update pricing and inventory
   - Delete discontinued products

3. **Manage Orders**
   - View all customer orders
   - Update order status
   - Process refunds or cancellations

4. **View Feedback**
   - Review customer feedback
   - Respond to customer inquiries

## 🔧 Configuration

### Session Timeout

Default session timeout is 30 minutes. To change it, modify `Program.cs`:

```csharp
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Change this value
});
```

### Password Requirements

Password requirements can be configured in `Program.cs`:

```csharp
builder.Services.AddIdentity<Client, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
});
```

## 🐛 Troubleshooting

### Database Connection Issues

If you encounter database connection errors:
1. Ensure SQL Server LocalDB is installed
2. Check the connection string in `appsettings.json`
3. Run `dotnet ef database update` to ensure migrations are applied

### Migration Errors

If migrations fail:
```bash
# Delete the Migrations folder
# Drop the database
dotnet ef database drop

# Create new migration
dotnet ef migrations add InitialCreate

# Update database
dotnet ef database update
```

### Port Already in Use

If the default ports are in use, modify `Properties/launchSettings.json` to use different ports.

## 📝 Development

### Adding New Features

1. Create necessary models in the `Models` folder
2. Update `FlowerShopContext` if adding new entities
3. Create a new migration: `dotnet ef migrations add <MigrationName>`
4. Update the database: `dotnet ef database update`
5. Implement controllers and views

### Running Tests

```bash
dotnet test
```

## 🤝 Contributing

Contributions are welcome! Please follow these steps:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 📧 Contact

For questions or support, please open an issue in the GitHub repository.

## 🙏 Acknowledgments

- ASP.NET Core team for the excellent framework
- Entity Framework Core for seamless database operations
- Bootstrap for responsive UI components

---

**Happy Shopping! 🌺**
