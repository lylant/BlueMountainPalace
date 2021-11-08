# Blue Mountain Palace

# Introduction

## Project Backgrounds
This project is originally established as an assignment during the study at Western Sydney University. The assignment was a group assignment, three members were in the group. All requirements are introducted as the assignment marking rubric. Several changes are applied after the semester to build this Git repository.

The project aims to build a website for the imaginary **Blue Mountain Palace** that is going to serve the Blue Mountains area, after the COVID-19 pandemic is possibly under control by the end of 2020. This website should allow customers to search an book rooms, and allow administrators to manage bookings and view statistics, et cetera. Basically the website focused on back-end functionalities only, hence, the design of the website will be a simple basic design with Bootstrap.

## Main Tech-Stacks for the Project
* C#
* ASP.NET Core (MVC with Razor Pages)
* Bootstrap
* SQLite


# Architecture

## Software Architecture

<p align="center">
    <img src="/_miscs/architecture_software.PNG">
</p>

## Database Architecture

### Entity-Relationship Diagram

<p align="center">
    <img src="/_miscs/architecture_erd.png">
</p>


### Database Schema

#### Room

Key     | Column  | Datatype | Nullable | Note
--------|---------|----------|----------|---------
primary |ID       |INTEGER   | NO       |
_       |Level    |TEXT      | NO       |
_       |BedCount |INTEGER   | NO       |
_       |Price    |TEXT      | NO       |

#### Customer

Key     | Column    | Datatype | Nullable | Note
--------|-----------|----------|----------|---------
primary |Email      |TEXT      | NO       |
_       |LastName   |TEXT      | NO       |
_       |FirstName  |TEXT      | NO       |
_       |Postcode   |TEXT      | NO       |

#### Booking

Key     | Column       | Datatype | Nullable | Note
--------|--------------|----------|----------|---------------
primary |ID            |INTEGER   | NO       |
_       |CheckIn       |TEXT      | NO       |
_       |CheckOut      |TEXT      | NO       |
_       |Cost          |TEXT      | NO       |
foreign |RoomID        |INTEGER   | NO       | Room.ID
foreign |CustomerEmail |TEXT      | NO       | Customer.Email

#### AspNetUsers

Key     | Column              | Datatype | Nullable | Note
--------|---------------------|----------|----------|---------
primary |Id                   |TEXT      | NO       |
_       |UserName             |TEXT      | YES      |
_       |NormalizedUserName   |TEXT      | YES      |
_       |Email                |TEXT      | YES      |
_       |NormalizedEmail      |TEXT      | YES      |
_       |EmailConfirmed       |INTEGER   | NO       |
_       |PasswordHash         |TEXT      | YES      |
_       |SecurityStamp        |TEXT      | YES      |
_       |ConcurrencyStamp     |TEXT      | YES      |
_       |PhoneNumber          |TEXT      | YES      |
_       |PhoneNumberConfirmed |INTEGER   | NO       |
_       |TwoFactorEnabled     |INTEGER   | NO       |
_       |LockoutEnd           |TEXT      | YES      |
_       |LockoutEnabled       |INTEGER   | NO       |
_       |AccessFailedCount    |INTEGER   | NO       |

#### AspNetUserLogins

Key     | Column             | Datatype | Nullable | Note
--------|--------------------|----------|----------|---------------
primary |LoginProvider       |TEXT      | NO       |
primary |ProviderKey         |TEXT      | NO       |
_       |ProviderDisplayName |TEXT      | YES      |
foreign |UserId              |TEXT      | NO       | AspNetUsers.Id


#### AspNetTokens

Key     | Column       | Datatype | Nullable | Note
--------|--------------|----------|----------|---------------
pri/for |UserId        |TEXT      | NO       | AspNetUsers.Id
primary |LoginProvider |TEXT      | NO       |
primary |Name          |TEXT      | NO       |
_       |Value         |TEXT      | YES      |

#### AspNetUserClaims

Key     | Column       | Datatype | Nullable | Note
--------|--------------|----------|----------|---------------
primary |Id            |INTEGER   | NO       |
foreign |UserId        |TEXT      | NO       | AspNetUsers.Id
_       |ClaimType     |TEXT      | YES      |
_       |ClaimValue    |TEXT      | YES      |

#### AspNetRoles

Key     | Column             | Datatype | Nullable | Note
--------|--------------------|----------|----------|---------------
primary |Id                  |TEXT      | NO       |
_       |Name                |TEXT      | YES      |
_       |NormalizedName      |TEXT      | YES      |
_       |ConcurrencyStamp    |TEXT      | YES      |

#### AspNetRoleClaims

Key     | Column       | Datatype | Nullable | Note
--------|--------------|----------|----------|---------------
primary |Id            |INTEGER   | NO       |
foreign |RoleId        |TEXT      | NO       | AspNetRoles.Id
_       |ClaimType     |TEXT      | YES      |
_       |ClaimValue    |TEXT      | YES      |

#### AspNetUserRoles

Key     | Column       | Datatype | Nullable | Note
--------|--------------|----------|----------|---------------
pri/for |UserId        |TEXT      | NO       | AspNetUsers.Id
pri/for |RoleId        |TEXT      | NO       | AspNetRoles.Id