# Az?rbaycanca Error Mesajlar?

## T?tbiq Edildi

Bütün Identity error mesajlar? Az?rbaycancaya t?rcüm? edildi! `AzerbaijaniIdentityErrorDescriber` vasit?sil? sistem avtomatik olaraq Az?rbaycan dilind? error qaytar?r.

## ?ifr? X?talar?

| ?ngilis Mesaj | Az?rbaycan Mesaj? |
|--------------|-------------------|
| Passwords must be at least 8 characters | ?ifr? ?n az? 8 simvol olmal?d?r |
| Passwords must have at least one digit | ?ifr? ?n az? bir r?q?m (0-9) ehtiva etm?lidir |
| Passwords must have at least one lowercase | ?ifr? ?n az? bir kiçik h?rf (a-z) ehtiva etm?lidir |
| Passwords must have at least one uppercase | ?ifr? ?n az? bir böyük h?rf (A-Z) ehtiva etm?lidir |
| Passwords must have at least one non alphanumeric | ?ifr? ?n az? bir xüsusi simvol (!@#$%^&* v? s.) ehtiva etm?lidir |
| Passwords must use at least 1 different characters | ?ifr? ?n az? 1 f?rqli simvol ehtiva etm?lidir |
| Incorrect password | ?ifr?l?r uy?un g?lmir |

## ?stifad?çi X?talar?

| ?ngilis Mesaj | Az?rbaycan Mesaj? |
|--------------|-------------------|
| Email 'xxx' is already taken | xxx art?q istifad? olunur |
| Username 'xxx' is already taken | xxx art?q istifad? olunur |
| Email 'xxx' is invalid | xxx etibars?z email ünvan?d?r |
| Username 'xxx' is invalid | xxx etibars?zd?r. ?stifad?çi ad? yaln?z h?rf v? r?q?ml?rd?n ibar?t ola bil?r |
| User already has a password set | ?stifad?çinin art?q ?ifr?si mövcuddur |
| Lockout is not enabled for this user | Bu istifad?çi üçün bloklanma aktivl??dirilm?yib |

## Rol X?talar?

| ?ngilis Mesaj | Az?rbaycan Mesaj? |
|--------------|-------------------|
| User already in role 'Admin' | ?stifad?çi art?q 'Admin' roluna malikdir |
| User is not in role 'Admin' | ?stifad?çi 'Admin' roluna malik deyil |
| Role name 'xxx' is already taken | 'xxx' rolu art?q mövcuddur |
| Role name 'xxx' is invalid | 'xxx' etibars?z rol ad?d?r |

## Token X?talar?

| ?ngilis Mesaj | Az?rbaycan Mesaj? |
|--------------|-------------------|
| Invalid token | Token etibars?zd?r |
| Invalid recovery code | B?rpa kodu etibars?zd?r |

## Dig?r X?talar

| ?ngilis Mesaj | Az?rbaycan Mesaj? |
|--------------|-------------------|
| Optimistic concurrency failure | Optimistik yoxlama x?tas?, obyekt d?yi?dirilib |
| An unknown failure has occurred | X?ta ba? verdi |
| A user with this login already exists | Bu login art?q ba?qa istifad?çiy? ba?l?d?r |

## Test Nümun?l?ri

### ?ifr? çox q?sa
```bash
POST /api/admin/auth/admin/reset-password
{
  "email": "test@test.com",
  "newPassword": "123",
  "confirmPassword": "123"
}

# Response:
{
  "success": false,
  "message": "?ifr? t?yin edilm?di: ?ifr? ?n az? 8 simvol olmal?d?r"
}
```

### Böyük h?rf yoxdur
```bash
{
  "newPassword": "test@123",
  "confirmPassword": "test@123"
}

# Response:
{
  "success": false,
  "message": "?ifr? t?yin edilm?di: ?ifr? ?n az? bir böyük h?rf (A-Z) ehtiva etm?lidir"
}
```

### R?q?m yoxdur
```bash
{
  "newPassword": "Test@abc",
  "confirmPassword": "Test@abc"
}

# Response:
{
  "success": false,
  "message": "?ifr? t?yin edilm?di: ?ifr? ?n az? bir r?q?m (0-9) ehtiva etm?lidir"
}
```

### Xüsusi simvol yoxdur
```bash
{
  "newPassword": "Test1234",
  "confirmPassword": "Test1234"
}

# Response:
{
  "success": false,
  "message": "?ifr? t?yin edilm?di: ?ifr? ?n az? bir xüsusi simvol (!@#$%^&* v? s.) ehtiva etm?lidir"
}
```

### Email art?q mövcuddur
```bash
POST /api/admin/users/create
{
  "email": "admin@test.com",  // Already exists
  "password": "Admin@123"
}

# Response:
{
  "success": false,
  "message": "admin@test.com art?q istifad? olunur"
}
```

## Konfiqurasiya

`App.DAL/DALDependencyInjection.cs`:

```csharp
services.AddIdentity<User, IdentityRole>(...)
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders()
    .AddErrorDescriber<AzerbaijaniIdentityErrorDescriber>(); // ? Az?rbaycanca
```

## Kod Strukturu

```
App.DAL/
  ??? Localization/
      ??? AzerbaijaniIdentityErrorDescriber.cs  ?
```

T?rcüm? edilmi? metodlar:
- ? `PasswordTooShort`
- ? `PasswordRequiresDigit`
- ? `PasswordRequiresLower`
- ? `PasswordRequiresUpper`
- ? `PasswordRequiresNonAlphanumeric`
- ? `PasswordRequiresUniqueChars`
- ? `PasswordMismatch`
- ? `DuplicateEmail`
- ? `DuplicateUserName`
- ? `InvalidEmail`
- ? `InvalidUserName`
- ? `UserAlreadyHasPassword`
- ? `UserAlreadyInRole`
- ? `UserNotInRole`
- ? `UserLockoutNotEnabled`
- ? `DuplicateRoleName`
- ? `InvalidRoleName`
- ? `InvalidToken`
- ? `RecoveryCodeRedemptionFailed`
- ? `ConcurrencyFailure`
- ? `DefaultError`
- ? `LoginAlreadyAssociated`

## Art?q ??l?yir!

Bütün Identity ?m?liyyatlar?nda (login, change password, reset password, create user v? s.) avtomatik olaraq Az?rbaycanca error mesajlar? göst?rilir.

### Change Password
```json
{
  "currentPassword": "wrong",
  "newPassword": "Test@123",
  "confirmPassword": "Test@123"
}

// Old: "Incorrect password"
// New: "?ifr?l?r uy?un g?lmir" ?
```

### Admin Reset Password
```json
{
  "email": "user@test.com",
  "newPassword": "weak",
  "confirmPassword": "weak"
}

// Old: "Passwords must be at least 8 characters"
// New: "?ifr? ?n az? 8 simvol olmal?d?r" ?
```

Build u?urla tamamland?! ?ndi bütün error mesajlar? Az?rbaycancad?r! ?????
