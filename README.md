# Authentication API

# OAuth Login API

This project is an example of an OAuth login API built with ASP.Net Core MVC. The API authenticates users and issues JWT tokens containing user roles and accessible system regions. 

## Features

- Username and password authentication
- JWT token generation with roles and system regions in claims
- Static data for users, roles, and regions (no database)

## System Roles and Regions

### Regions
1. Board game `b_game` (default for all logged in users)
2. VIP Character modification `vip_chararacter_personalize` (For VIP users)

### Roles
1. Player `player`
2. Administrator `admin`

## Prerequisites

- .NET 6.0 SDK or later

## Getting Started

### Clone the Repository

```sh
git clone https://github.com/yourusername/oauth-login-api.git
cd oauth-login-api


POST /api/auth/login
Request
{
  "username": "exampleUser",
  "password": "examplePassword"
}
Response
json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}

  
//////////////////////////////////////////////////////////////////////////////////////////////

Example Users
Player

Username: player
Password: player123
Role: player
Regions: b_game


VIP Player
Username: admin
Password: admin123
Role: admin
Regions: b_game, vip_chararacter_personalize

  

