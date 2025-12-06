# Password Management Implementation

## Overview
Tam password idar?etm? sistemi t?tbiq edildi:
- ? Change Password (Login olmu? istifad?çil?r üçün)
- ? Forgot Password (Email il? token gönd?rm?)
- ? Reset Password (Token il? ?ifr? b?rpas?)
- ? Validate Reset Token (Token yoxlanmas?)

## Endpoints

### 1. Change Password (Authenticated)
**POST** `/api/admin/auth/change-password`

Login olmu? istifad?çi ?ifr?sini d?yi?ir.

**Headers:**
```
Authorization: Bearer YOUR_JWT_TOKEN
Content-Type: application/json
```

**Request Body:**
```json
{
  "currentPassword": "oldPassword123",
  "newPassword": "newPassword456",
  "confirmPassword": "newPassword456"
}
```

**Success Response (200):**
```json
{
  "success": true,
  "message": "?ifr? u?urla d?yi?dirildi"
}
```

**Error Responses:**
```json
{
  "success": false,
  "message": "Cari ?ifr? yanl??d?r"
}
```

```json
{
  "success": false,
  "message": "Yeni ?ifr? cari ?ifr?d?n f?rqli olmal?d?r"
}
```

```json
{
  "success": false,
  "message": "Yeni ?ifr? ?n az? 6 simvol olmal?d?r"
}
```

### 2. Forgot Password (Public)
**POST** `/api/admin/auth/forgot-password`

Email ünvan?na ?ifr? b?rpa linki gönd?rir.

**Request Body:**
```json
{
  "email": "user@example.com"
}
```

**Success Response (200):**
```json
{
  "success": true,
  "message": "?ifr? b?rpa t?limatlar? email ünvan?n?za gönd?rildi"
}
```

**Qeyd:** T?hlük?sizlik üçün, email mövcud olub-olmamas?ndan as?l? olmayaraq eyni mesaj qaytar?l?r.

### 3. Reset Password (Public)
**POST** `/api/admin/auth/reset-password`

Email-d?n al?nan token il? ?ifr? b?rpas?.

**Request Body:**
```json
{
  "email": "user@example.com",
  "token": "CfDJ8N...(long token)...",
  "newPassword": "newPassword123",
  "confirmPassword": "newPassword123"
}
```

**Success Response (200):**
```json
{
  "success": true,
  "message": "?ifr? u?urla b?rpa edildi. ?ndi yeni ?ifr?nizl? daxil ola bil?rsiniz"
}
```

**Error Responses:**
```json
{
  "success": false,
  "message": "Token etibars?zd?r v? ya müdd?ti bitib"
}
```

### 4. Validate Reset Token (Public)
**POST** `/api/admin/auth/validate-reset-token`

Reset token-in etibarl? olub-olmad???n? yoxlay?r.

**Request Body:**
```json
{
  "email": "user@example.com",
  "token": "CfDJ8N...(long token)..."
}
```

**Success Response (200):**
```json
{
  "success": true,
  "message": "Token etibarl?d?r"
}
```

**Error Response (400):**
```json
{
  "success": false,
  "message": "Token etibars?zd?r v? ya müdd?ti bitib"
}
```

## DTOs

### ChangePasswordDTO
```csharp
public class ChangePasswordDTO
{
    [Required(ErrorMessage = "Cari ?ifr? daxil edilm?lidir")]
    public string CurrentPassword { get; set; }

    [Required(ErrorMessage = "Yeni ?ifr? daxil edilm?lidir")]
    [MinLength(6, ErrorMessage = "Yeni ?ifr? ?n az? 6 simvol olmal?d?r")]
    public string NewPassword { get; set; }

    [Required(ErrorMessage = "?ifr? t?sdiqi daxil edilm?lidir")]
    [Compare("NewPassword", ErrorMessage = "Yeni ?ifr? v? t?sdiq ?ifr?si uy?un g?lmir")]
    public string ConfirmPassword { get; set; }
}
```

### ForgotPasswordDTO
```csharp
public class ForgotPasswordDTO
{
    [Required(ErrorMessage = "Email daxil edilm?lidir")]
    [EmailAddress(ErrorMessage = "Email format? düzgün deyil")]
    public string Email { get; set; }
}
```

### ResetPasswordDTO
```csharp
public class ResetPasswordDTO
{
    [Required(ErrorMessage = "Email daxil edilm?lidir")]
    [EmailAddress(ErrorMessage = "Email format? düzgün deyil")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Token daxil edilm?lidir")]
    public string Token { get; set; }

    [Required(ErrorMessage = "Yeni ?ifr? daxil edilm?lidir")]
    [MinLength(6, ErrorMessage = "?ifr? ?n az? 6 simvol olmal?d?r")]
    public string NewPassword { get; set; }

    [Required(ErrorMessage = "?ifr? t?sdiqi daxil edilm?lidir")]
    [Compare("NewPassword", ErrorMessage = "?ifr? v? t?sdiq ?ifr?si uy?un g?lmir")]
    public string ConfirmPassword { get; set; }
}
```

### ValidateTokenDTO
```csharp
public class ValidateTokenDTO
{
    [Required(ErrorMessage = "Email daxil edilm?lidir")]
    [EmailAddress(ErrorMessage = "Email format? düzgün deyil")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Token daxil edilm?lidir")]
    public string Token { get; set; }
}
```

## Business Logic

### Change Password Flow

1. ? ?stifad?çi tap?l?r
2. ? Hesab aktiv olub-olmad??? yoxlan?l?r
3. ? Cari ?ifr? yoxlan?l?r
4. ? Yeni ?ifr?nin uzunlu?u yoxlan?l?r (min 6)
5. ? Yeni ?ifr? cari ?ifr?d?n f?rqli olmal?d?r
6. ? Identity password validations t?tbiq olunur
7. ? ?ifr? d?yi?dirilir

### Forgot Password Flow

1. ? Email ünvan? yoxlan?l?r
2. ? ?stifad?çi tap?l?rsa, reset token yarad?l?r
3. ? Email gönd?rilir (TODO: Email service integration)
4. ? T?hlük?sizlik üçün h?mi?? success response qaytar?l?r

### Reset Password Flow

1. ? Email v? token yoxlan?l?r
2. ? ?stifad?çi tap?l?r
3. ? Hesab aktiv olub-olmad??? yoxlan?l?r
4. ? Token etibarl? olub-olmad??? yoxlan?l?r
5. ? ?ifr? Identity t?r?find?n reset edilir

## Validations

### Password Requirements
- Minimum uzunluq: 6 simvol
- Yeni ?ifr? cari ?ifr?d?n f?rqli olmal?d?r (Change Password)
- ConfirmPassword NewPassword il? eyni olmal?d?r

### Token Validation
- Token ASP.NET Identity t?r?find?n yarad?l?r
- Token mü?yy?n müdd?t sonra expire olur
- H?r istifad?çi üçün unikal token yarad?l?r

## Frontend Integration Examples

### Change Password Form (React)

```jsx
import { useState } from 'react';

function ChangePasswordForm() {
  const [formData, setFormData] = useState({
    currentPassword: '',
    newPassword: '',
    confirmPassword: ''
  });
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    setSuccess('');

    try {
      const response = await fetch('/api/admin/auth/change-password', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${localStorage.getItem('token')}`
        },
        body: JSON.stringify(formData)
      });

      const result = await response.json();

      if (result.success) {
        setSuccess(result.message);
        setFormData({ currentPassword: '', newPassword: '', confirmPassword: '' });
      } else {
        setError(result.message);
      }
    } catch (err) {
      setError('X?ta ba? verdi');
    }
  };

  return (
    <form onSubmit={handleSubmit}>
      {error && <div className="error">{error}</div>}
      {success && <div className="success">{success}</div>}

      <input
        type="password"
        placeholder="Cari ?ifr?"
        value={formData.currentPassword}
        onChange={e => setFormData({...formData, currentPassword: e.target.value})}
        required
      />

      <input
        type="password"
        placeholder="Yeni ?ifr?"
        value={formData.newPassword}
        onChange={e => setFormData({...formData, newPassword: e.target.value})}
        required
        minLength={6}
      />

      <input
        type="password"
        placeholder="?ifr? t?sdiqi"
        value={formData.confirmPassword}
        onChange={e => setFormData({...formData, confirmPassword: e.target.value})}
        required
      />

      <button type="submit">?ifr?ni D?yi?</button>
    </form>
  );
}
```

### Forgot Password Form

```jsx
function ForgotPasswordForm() {
  const [email, setEmail] = useState('');
  const [message, setMessage] = useState('');

  const handleSubmit = async (e) => {
    e.preventDefault();

    const response = await fetch('/api/admin/auth/forgot-password', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ email })
    });

    const result = await response.json();
    setMessage(result.message);
  };

  return (
    <form onSubmit={handleSubmit}>
      {message && <div className="info">{message}</div>}
      
      <input
        type="email"
        placeholder="Email ünvan?n?z"
        value={email}
        onChange={e => setEmail(e.target.value)}
        required
      />

      <button type="submit">?ifr? B?rpas? Gönd?r</button>
    </form>
  );
}
```

### Reset Password Form

```jsx
import { useSearchParams } from 'react-router-dom';

function ResetPasswordForm() {
  const [searchParams] = useSearchParams();
  const email = searchParams.get('email');
  const token = searchParams.get('token');

  const [formData, setFormData] = useState({
    email: email || '',
    token: token || '',
    newPassword: '',
    confirmPassword: ''
  });
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');

  // Validate token on mount
  useEffect(() => {
    if (email && token) {
      validateToken();
    }
  }, []);

  const validateToken = async () => {
    const response = await fetch('/api/admin/auth/validate-reset-token', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ email, token })
    });

    const result = await response.json();
    if (!result.success) {
      setError(result.message);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    const response = await fetch('/api/admin/auth/reset-password', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(formData)
    });

    const result = await response.json();

    if (result.success) {
      setSuccess(result.message);
      // Redirect to login after 2 seconds
      setTimeout(() => {
        window.location.href = '/login';
      }, 2000);
    } else {
      setError(result.message);
    }
  };

  return (
    <form onSubmit={handleSubmit}>
      {error && <div className="error">{error}</div>}
      {success && <div className="success">{success}</div>}

      <input
        type="password"
        placeholder="Yeni ?ifr?"
        value={formData.newPassword}
        onChange={e => setFormData({...formData, newPassword: e.target.value})}
        required
        minLength={6}
      />

      <input
        type="password"
        placeholder="?ifr? t?sdiqi"
        value={formData.confirmPassword}
        onChange={e => setFormData({...formData, confirmPassword: e.target.value})}
        required
      />

      <button type="submit">?ifr?ni B?rpa Et</button>
    </form>
  );
}
```

## Email Integration (TODO)

Haz?rda email gönd?rm? funksiyas? implement edilm?yib. Production üçün email service ?lav? etm?k laz?md?r:

```csharp
// IEmailService interface
public interface IEmailService
{
    Task SendPasswordResetEmailAsync(string email, string resetLink);
}

// ForgotPasswordAsync metodunda:
var resetLink = $"https://yourapp.com/reset-password?email={email}&token={Uri.EscapeDataString(resetToken)}";
await _emailService.SendPasswordResetEmailAsync(user.Email, resetLink);
```

## Security Features

? **Password Validation**
- Minimum length enforcement
- Cannot reuse current password
- Confirm password matching

? **Token Security**
- Tokens are time-limited
- Tokens are user-specific
- Tokens can only be used once

? **Email Enumeration Prevention**
- Same success message regardless of email existence
- Prevents attackers from discovering valid emails

? **Account Security**
- Disabled accounts cannot reset password
- Current password required for change
- Token validation before reset

## Testing

### Test Change Password
```bash
# Login first
curl -X POST http://localhost:5000/api/admin/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@test.com","password":"Admin@123"}'

# Get token from response, then:
curl -X POST http://localhost:5000/api/admin/auth/change-password \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "currentPassword":"Admin@123",
    "newPassword":"NewPass@123",
    "confirmPassword":"NewPass@123"
  }'
```

### Test Forgot Password
```bash
curl -X POST http://localhost:5000/api/admin/auth/forgot-password \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@test.com"}'
```

### Test Reset Password
```bash
curl -X POST http://localhost:5000/api/admin/auth/reset-password \
  -H "Content-Type: application/json" \
  -d '{
    "email":"admin@test.com",
    "token":"TOKEN_FROM_EMAIL",
    "newPassword":"NewPass@123",
    "confirmPassword":"NewPass@123"
  }'
```

## Error Messages (Azerbaijani)

- "Cari ?ifr? daxil edilm?lidir"
- "Yeni ?ifr? daxil edilm?lidir"
- "Yeni ?ifr? ?n az? 6 simvol olmal?d?r"
- "?ifr? t?sdiqi daxil edilm?lidir"
- "Yeni ?ifr? v? t?sdiq ?ifr?si uy?un g?lmir"
- "?stifad?çi tap?lmad?"
- "Hesab?n?z deaktiv edilib"
- "Cari ?ifr? yanl??d?r"
- "Yeni ?ifr? cari ?ifr?d?n f?rqli olmal?d?r"
- "Token etibars?zd?r v? ya müdd?ti bitib"
- "?ifr? u?urla d?yi?dirildi"
- "?ifr? u?urla b?rpa edildi"
- "Token etibarl?d?r"

## Summary

? **Change Password** - Login olmu? istifad?çil?r üçün
? **Forgot Password** - Email il? token gönd?rm?
? **Reset Password** - Token il? ?ifr? b?rpas?
? **Token Validation** - Token yoxlanmas?
? **Full Validation** - Bütün input-lar validasiya olunur
? **Security** - T?hlük?sizlik t?dbirl?ri t?tbiq edilib
? **Error Handling** - Ayd?n error mesajlar?
? **DTO Separation** - T?miz kod strukturu

?? **TODO**: Email service integration
