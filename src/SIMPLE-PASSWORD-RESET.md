# Simple Password Reset - No Token Required

## Overview
Token-based password reset ?v?zin?, sad? admin ?ifr? reset funksiyas? t?tbiq edildi. Email t?sdiql?m? olmad??? üçün admin birba?a istifad?çinin ?ifr?sini d?yi?? bil?r.

## D?yi?iklikl?r

### Silin?n Funksiyalar (Token-based)
- ? Forgot Password (email gönd?rm?)
- ? Reset Password with Token
- ? Validate Reset Token

### ?lav? Edil?n Funksiya
- ? Admin Reset Password (Token yoxdur, sad?c? email v? yeni ?ifr?)

## Endpoint

### Admin Reset Password
**POST** `/api/admin/auth/admin/reset-password`

Admin v? ya SuperAdmin istifad?çini email-? gör? ?ifr?ni d?yi?? bil?r.

**Authorization:** Admin v? ya SuperAdmin role t?l?b olunur

**Headers:**
```
Authorization: Bearer YOUR_ADMIN_TOKEN
Content-Type: application/json
```

**Request Body:**
```json
{
  "email": "user@example.com",
  "newPassword": "newPassword123",
  "confirmPassword": "newPassword123"
}
```

**Success Response (200):**
```json
{
  "success": true,
  "message": "user@example.com istifad?çisinin ?ifr?si u?urla d?yi?dirildi"
}
```

**Error Responses:**

**?stifad?çi tap?lmad?:**
```json
{
  "success": false,
  "message": "?stifad?çi tap?lmad?"
}
```

**?ifr? çox q?sa:**
```json
{
  "success": false,
  "message": "?ifr? ?n az? 6 simvol olmal?d?r"
}
```

**?ifr? t?sdiqi uy?un deyil:**
```json
{
  "success": false,
  "message": "?ifr? v? t?sdiq ?ifr?si uy?un g?lmir"
}
```

**Unauthorized (401):**
```json
{
  "success": false,
  "message": "Unauthorized"
}
```

**Forbidden (403) - Admin deyilsinizs?:**
```json
{
  "success": false,
  "message": "Forbidden"
}
```

## DTO

### AdminResetPasswordDTO
```csharp
public class AdminResetPasswordDTO
{
    [Required(ErrorMessage = "Email daxil edilm?lidir")]
    [EmailAddress(ErrorMessage = "Email format? düzgün deyil")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Yeni ?ifr? daxil edilm?lidir")]
    [MinLength(6, ErrorMessage = "?ifr? ?n az? 6 simvol olmal?d?r")]
    public string NewPassword { get; set; }

    [Required(ErrorMessage = "?ifr? t?sdiqi daxil edilm?lidir")]
    [Compare("NewPassword", ErrorMessage = "?ifr? v? t?sdiq ?ifr?si uy?un g?lmir")]
    public string ConfirmPassword { get; set; }
}
```

## Business Logic

### AdminResetPasswordAsync Flow

1. ? Email il? istifad?çi tap?l?r
2. ? Yeni ?ifr? validation-dan keçir (min 6 simvol)
3. ? Köhn? ?ifr? silinir (`RemovePasswordAsync`)
4. ? Yeni ?ifr? t?yin edilir (`AddPasswordAsync`)
5. ? Success mesaj? qaytar?l?r

**Qeyd:** Köhn? ?ifr? t?l?b olunmur, admin birba?a d?yi?ir.

## Kod Nümun?si

### Service Implementation
```csharp
public async Task<ServiceResult> AdminResetPasswordAsync(string email, string newPassword)
{
    var user = await _userManager.FindByEmailAsync(email);
    
    if (user == null)
    {
        return ServiceResult.FailureResult("?stifad?çi tap?lmad?");
    }

    if (string.IsNullOrWhiteSpace(newPassword))
    {
        return ServiceResult.FailureResult("Yeni ?ifr? daxil edilm?lidir");
    }

    if (newPassword.Length < 6)
    {
        return ServiceResult.FailureResult("?ifr? ?n az? 6 simvol olmal?d?r");
    }

    // Remove current password
    var removePasswordResult = await _userManager.RemovePasswordAsync(user);
    if (!removePasswordResult.Succeeded)
    {
        return ServiceResult.FailureResult("?ifr? yenil?nm?di");
    }

    // Add new password
    var addPasswordResult = await _userManager.AddPasswordAsync(user, newPassword);
    if (!addPasswordResult.Succeeded)
    {
        var errors = string.Join(", ", addPasswordResult.Errors.Select(e => e.Description));
        return ServiceResult.FailureResult($"?ifr? t?yin edilm?di: {errors}");
    }

    return ServiceResult.SuccessResult($"{user.Email} istifad?çisinin ?ifr?si u?urla d?yi?dirildi");
}
```

## Frontend Integration

### React Admin Panel

```jsx
import { useState } from 'react';

function AdminResetPasswordForm() {
  const [formData, setFormData] = useState({
    email: '',
    newPassword: '',
    confirmPassword: ''
  });
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    setSuccess('');
    setLoading(true);

    try {
      const response = await fetch('/api/admin/auth/admin/reset-password', {
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
        setFormData({ email: '', newPassword: '', confirmPassword: '' });
      } else {
        setError(result.message);
      }
    } catch (err) {
      setError('X?ta ba? verdi');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="admin-reset-password">
      <h2>?stifad?çi ?ifr?sini S?f?rla</h2>
      
      <form onSubmit={handleSubmit}>
        {error && <div className="error-message">{error}</div>}
        {success && <div className="success-message">{success}</div>}

        <div className="form-group">
          <label>?stifad?çi Email</label>
          <input
            type="email"
            placeholder="user@example.com"
            value={formData.email}
            onChange={e => setFormData({...formData, email: e.target.value})}
            required
          />
        </div>

        <div className="form-group">
          <label>Yeni ?ifr?</label>
          <input
            type="password"
            placeholder="Minimum 6 simvol"
            value={formData.newPassword}
            onChange={e => setFormData({...formData, newPassword: e.target.value})}
            required
            minLength={6}
          />
        </div>

        <div className="form-group">
          <label>?ifr? T?sdiqi</label>
          <input
            type="password"
            placeholder="?ifr?ni t?krar daxil edin"
            value={formData.confirmPassword}
            onChange={e => setFormData({...formData, confirmPassword: e.target.value})}
            required
          />
        </div>

        <button type="submit" disabled={loading}>
          {loading ? '?ifr? d?yi?dirilir...' : '?ifr?ni S?f?rla'}
        </button>
      </form>
    </div>
  );
}

export default AdminResetPasswordForm;
```

### User Management Page

```jsx
function UserManagementPage() {
  const [users, setUsers] = useState([]);
  const [selectedUser, setSelectedUser] = useState(null);
  const [showResetModal, setShowResetModal] = useState(false);
  const [newPassword, setNewPassword] = useState('');

  const handleResetPassword = async () => {
    if (!selectedUser) return;

    const response = await fetch('/api/admin/auth/admin/reset-password', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${token}`
      },
      body: JSON.stringify({
        email: selectedUser.email,
        newPassword: newPassword,
        confirmPassword: newPassword
      })
    });

    const result = await response.json();
    
    if (result.success) {
      alert(result.message);
      setShowResetModal(false);
      setNewPassword('');
    } else {
      alert(result.message);
    }
  };

  return (
    <div className="user-management">
      <h1>?stifad?çi ?dar?etm?si</h1>
      
      <table>
        <thead>
          <tr>
            <th>Email</th>
            <th>Ad Soyad</th>
            <th>Rol</th>
            <th>?m?liyyatlar</th>
          </tr>
        </thead>
        <tbody>
          {users.map(user => (
            <tr key={user.id}>
              <td>{user.email}</td>
              <td>{user.firstName} {user.lastName}</td>
              <td>{user.role}</td>
              <td>
                <button onClick={() => {
                  setSelectedUser(user);
                  setShowResetModal(true);
                }}>
                  ?ifr?ni S?f?rla
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>

      {showResetModal && (
        <div className="modal">
          <div className="modal-content">
            <h3>?ifr?ni S?f?rla: {selectedUser?.email}</h3>
            <input
              type="password"
              placeholder="Yeni ?ifr? (min 6 simvol)"
              value={newPassword}
              onChange={e => setNewPassword(e.target.value)}
              minLength={6}
            />
            <div className="modal-actions">
              <button onClick={handleResetPassword}>T?sdiq Et</button>
              <button onClick={() => setShowResetModal(false)}>L??v Et</button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
```

## Security

### Authorization
- ? Yaln?z Admin v? SuperAdmin reset ed? bil?r
- ? JWT token yoxlan?l?r
- ? Role-based authorization

### Password Requirements
- ? Minimum 6 simvol
- ? ConfirmPassword match olmal?
- ? ASP.NET Identity validations

### Audit
?st?y? gör? audit log ?lav? edil? bil?r:

```csharp
public async Task<ServiceResult> AdminResetPasswordAsync(string email, string newPassword)
{
    // ... password reset logic ...

    // Log the action
    await _auditLogService.LogAsync(new AuditLog
    {
        Action = "Password Reset",
        UserId = user.Id,
        PerformedBy = _claimService.GetUserId(),
        Timestamp = DateTime.UtcNow,
        Details = $"Password reset for {email}"
    });

    return ServiceResult.SuccessResult($"{user.Email} istifad?çisinin ?ifr?si u?urla d?yi?dirildi");
}
```

## Comparison: Token vs No Token

| Xüsusiyy?t | Token-based | No Token (Admin) |
|------------|-------------|------------------|
| **Email T?sdiql?m?** | Laz?md?r | Laz?m deyil |
| **Token Yaratma** | Var | Yoxdur |
| **Token Expiration** | Var | Yoxdur |
| **Email Gönd?rm?** | Laz?md?r | Laz?m deyil |
| **Security** | ?stifad?çi özü reset edir | Admin reset edir |
| **Complexity** | Yüks?k | A?a?? |
| **Use Case** | Public reset | Admin panel |

## Use Cases

### Admin Panel
1. ? ?stifad?çi ?ifr?sini unutub
2. ? ?stifad?çi daxil ola bilmir
3. ? ?lk ?ifr? t?yini
4. ? T?cili ?ifr? d?yi?ikliyi

### User Self-Service
?stifad?çi özü ?ifr?sini d?yi?m?k ist?yirs?:
- ? Login olmal?
- ? Change Password endpoint istifad? etm?li
- ? Cari ?ifr?ni bilm?lidir

## Testing

### Test with cURL

```bash
# Login as Admin first
curl -X POST http://localhost:5000/api/admin/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@test.com","password":"Admin@123"}'

# Get token from response, then reset password
curl -X POST http://localhost:5000/api/admin/auth/admin/reset-password \
  -H "Authorization: Bearer YOUR_ADMIN_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "email":"user@test.com",
    "newPassword":"NewPass@123",
    "confirmPassword":"NewPass@123"
  }'
```

### Test Scenarios

**? Success:**
```json
{
  "email": "user@test.com",
  "newPassword": "NewPass123",
  "confirmPassword": "NewPass123"
}
// Response: "user@test.com istifad?çisinin ?ifr?si u?urla d?yi?dirildi"
```

**? User Not Found:**
```json
{
  "email": "nonexistent@test.com",
  "newPassword": "NewPass123",
  "confirmPassword": "NewPass123"
}
// Response: "?stifad?çi tap?lmad?"
```

**? Password Too Short:**
```json
{
  "email": "user@test.com",
  "newPassword": "123",
  "confirmPassword": "123"
}
// Response: "?ifr? ?n az? 6 simvol olmal?d?r"
```

**? Password Mismatch:**
```json
{
  "email": "user@test.com",
  "newPassword": "NewPass123",
  "confirmPassword": "Different123"
}
// Response: "?ifr? v? t?sdiq ?ifr?si uy?un g?lmir"
```

**? Not Admin:**
```
Authorization: Bearer USER_TOKEN (not admin)
// Response: 403 Forbidden
```

## Summary

? **Simple** - Token-based flow yoxdur
? **Admin Only** - Yaln?z admin/superadmin
? **No Email** - Email gönd?rm? t?l?b olunmur
? **Direct Reset** - Birba?a ?ifr? d?yi?ir
? **Secure** - Role-based authorization
? **Validated** - Tam input validation

Bu h?ll email t?sdiql?m? olmayan admin panel üçün ideald?r. ?stifad?çil?r öz ?ifr?l?rini d?yi?m?k ist?yirl?rs?, **Change Password** endpoint-d?n istifad? ed? bil?rl?r (cari ?ifr? il?).
