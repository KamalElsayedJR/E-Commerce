# E-Commerce Backend API

A robust and scalable RESTful API for an e-commerce platform built with ASP.NET Core 8 following Clean Architecture principles. This API provides comprehensive functionality for managing products, categories, shopping carts, orders, and payments.

## Description

This e-commerce backend API serves as the foundation for online retail operations, providing role-based access control for different user types:

- **Admins**: Full control over categories, products, and user management
- **Sellers**: Manage their product inventory and view orders
- **Customers**: Browse products, manage shopping carts, and place orders

The API implements modern software architecture patterns including Repository Pattern, Specification Pattern, and follows SOLID principles to ensure maintainability, testability, and scalability.

## Features

### Authentication & Authorization
- JWT-based authentication
- Role-based access control (Admin, Seller, Customer)
- Secure user registration and login
- Token-based session management

### Product Management
- CRUD operations for products
- Product search and filtering using Specification Pattern
- Pagination support for product listings
- Image upload and management via Cloudinary
- Product categorization

### Category Management
- Create, update, and delete categories
- Hierarchical category structure
- Category-based product filtering

### Shopping Cart
- Add/remove products to cart
- Update product quantities
- Persistent cart storage
- Cart calculation (subtotal, tax, total)

### Order Management
- Order creation and processing
- Order history and tracking
- Order status management
- Order details retrieval

### Payment Integration
- Stripe payment gateway integration
- Secure payment processing
- Payment confirmation and tracking

### Image Storage
- Cloudinary integration for image uploads
- Optimized image delivery
- Secure image management

## Tech Stack

**Framework**: ASP.NET Core 8  
**Language**: C# 12.0  
**Database**: SQL Server  
**ORM**: Entity Framework Core  
**Authentication**: JWT (JSON Web Tokens)  
**Payment Gateway**: Stripe  
**Image Storage**: Cloudinary  
**Architecture**: Clean Architecture  
**Design Patterns**: Repository Pattern, Specification Pattern, Dependency Injection

## Project Structure

The solution follows Clean Architecture principles with four distinct layers:

```
E-Commerce/
├── E-Commerce.API/              # Presentation Layer
│   ├── Controllers/             # API endpoints
│   └── Program.cs              # Application configuration
│
├── E-Commerce.Application/      # Application Layer
│   ├── DTOs/                   # Data Transfer Objects
│   ├── Interfaces/             # Service interfaces
│   ├── Services/               # Business logic implementation
│   └── Specifications/         # Query specifications
│
├── E-Commerce.Domain/           # Domain Layer
│   ├── Entities/               # Domain entities
│   └── Enums/                  # Domain enumerations
│
└── E-Commerce.Infrastructure/   # Infrastructure Layer
    ├── Data/                   # Database context and configurations
    ├── Repositories/           # Data access implementation
    └── Migrations/             # EF Core migrations
```

### Layer Responsibilities

- **API Layer**: Handles HTTP requests, routing, and response formatting
- **Application Layer**: Contains business logic, DTOs, and service interfaces
- **Domain Layer**: Core business entities and domain logic
- **Infrastructure Layer**: Data access, external services, and cross-cutting concerns

## Getting Started

### Prerequisites

- .NET 8 SDK or later
- SQL Server (LocalDB, Express, or Full)
- Visual Studio 2022 or Visual Studio Code
- Postman or similar API testing tool (optional)

### Installation Steps

1. **Clone the repository**
   ```bash
   git clone https://github.com/KamalElsayedJR/E-Commerce.git
   cd E-Commerce
   ```

2. **Configure the database connection**
   
   Update the connection string in `appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ECommerceDB;Trusted_Connection=true;TrustServerCertificate=true"
   }
   ```

3. **Configure environment variables**
   
   Add the following to `appsettings.json` or user secrets:
   ```json
   {
     "JWT": {
       "SecretKey": "your-secret-key-here",
       "Issuer": "your-issuer",
       "Audience": "your-audience",
       "ExpiryInMinutes": 60
     },
     "Stripe": {
       "SecretKey": "your-stripe-secret-key",
       "PublishableKey": "your-stripe-publishable-key"
     },
     "Cloudinary": {
       "CloudName": "your-cloud-name",
       "ApiKey": "your-api-key",
       "ApiSecret": "your-api-secret"
     }
   }
   ```

4. **Restore dependencies**
   ```bash
   dotnet restore
   ```

5. **Apply database migrations**
   ```bash
   dotnet ef database update --project E-Commerce.Infrastructure --startup-project E-Commerce.API
   ```

6. **Run the application**
   ```bash
   dotnet run --project E-Commerce.API
   ```

The API will be available at `https://localhost:5001` or `http://localhost:5000`.

## API Authentication

This API uses JWT (JSON Web Tokens) for authentication. To access protected endpoints:

1. **Register a new user** via `/Auth/register` endpoint
2. **Login** via `/Auth/login` endpoint to receive a JWT token
3. **Include the token** in subsequent requests using the Authorization header:
   ```
   Authorization: Bearer <your-jwt-token>
   ```

Tokens expire after the configured duration (default: 60 minutes). Upon expiration, users must re-authenticate to obtain a new token.

## Environment Variables

Create an `appsettings.json` or use user secrets for sensitive configuration:

| Variable | Description | Required |
|----------|-------------|----------|
| `ConnectionStrings:DefaultConnection` | SQL Server connection string | Yes |
| `JWT:SecretKey` | Secret key for JWT token generation | Yes |
| `JWT:Issuer` | Token issuer identifier | Yes |
| `JWT:Audience` | Token audience identifier | Yes |
| `JWT:ExpiryInMinutes` | Token expiration time in minutes | Yes |
| `Stripe:SecretKey` | Stripe API secret key | Yes |
| `Stripe:PublishableKey` | Stripe publishable key | Yes |
| `Cloudinary:CloudName` | Cloudinary cloud name | Yes |
| `Cloudinary:ApiKey` | Cloudinary API key | Yes |
| `Cloudinary:ApiSecret` | Cloudinary API secret | Yes |

## Payment Integration

This project integrates Stripe for secure payment processing:

- **Payment Methods**: Credit cards, debit cards, and other Stripe-supported methods
- **Security**: PCI-compliant payment processing
- **Flow**: 
  1. Customer adds products to cart
  2. Proceeds to checkout
  3. Payment processed through Stripe
  4. Order confirmed upon successful payment

For testing, use Stripe's test card numbers:
- Success: `4242 4242 4242 4242`
- Decline: `4000 0000 0000 0002`

## API Endpoints

### Authentication
- `POST /Auth/register` - Register new user
- `POST /Auth/login` - User login

### Categories
- `GET /Category/Categories` - Get all categories
- `GET /Category/{categoryId}` - Get category by ID
- `POST /Category` - Create category (Admin)
- `PUT /Category/{categoryId}` - Update category (Admin)
- `DELETE /Category/{categoryId}` - Delete category (Admin)

### Products
- `GET /Product/Products` - Get all products with pagination
- `GET /Product/{productId}` - Get product by ID
- `POST /Product` - Create product (Admin/Seller)
- `PUT /Product/{productId}` - Update product (Admin/Seller)
- `DELETE /Product/{productId}` - Delete product (Admin/Seller)

### Shopping Cart
- `GET /Cart` - Get user's cart
- `POST /Cart` - Add item to cart
- `PUT /Cart` - Update cart item
- `DELETE /Cart/{itemId}` - Remove item from cart

### Orders
- `GET /Order` - Get user's orders
- `GET /Order/{orderId}` - Get order details
- `POST /Order` - Create new order

## Future Improvements

- [ ] Implement product reviews and ratings
- [ ] Add wishlist functionality
- [ ] Implement real-time notifications (SignalR)
- [ ] Add product inventory management
- [ ] Implement discount and coupon system
- [ ] Add email notifications for order confirmations
- [ ] Implement product search with Elasticsearch
- [ ] Add API rate limiting
- [ ] Implement caching with Redis
- [ ] Create admin dashboard
- [ ] Add multi-language support
- [ ] Implement advanced analytics and reporting

## Author

**Kamal Elsayed**  
GitHub: [@KamalElsayedJR](https://github.com/KamalElsayedJR)


**Note**: This is a backend API project. A separate frontend application is required to provide a complete user interface for the e-commerce platform.
