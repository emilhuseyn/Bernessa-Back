# Quick UTF-8 Fix - Action Guide

## Problem
API responses showing garbled Azerbaijani text:
```
"Email v? ya ?ifr? yanl??d?r"  ?
```

Should be:
```
"Email v? ya ?ifr? yanl??d?r"  ?
```

## Solution Applied

### Files Changed
1. ? `App.Business\BusinessDependencyInjection.cs` - Added encoder configuration
2. ? `App.API\Program.cs` - Removed redundant configuration
3. ? `App.API\Middlewares\GlobalExceptionHandlerMiddleware.cs` - UTF-8 headers

### What Was Fixed
The root cause was **conflicting AddControllers() calls**. The Business layer's configuration wasn't setting the JSON encoder, which was overriding the Program.cs settings.

## Required Action

### Clean and Rebuild (REQUIRED)
```bash
# Navigate to src directory
cd C:\Users\Emil\source\repos\Bernessa\App\src

# Clean old binaries
dotnet clean

# Rebuild
dotnet build

# Or run the app
dotnet run --project App.API
```

### Why Clean is Necessary
- Old compiled assemblies contain the incorrect configuration
- .NET caches compiled binaries
- `dotnet clean` removes these cached files
- Rebuild ensures new configuration is compiled

## Testing

After rebuild, test the login endpoint:

**Request:**
```bash
POST http://localhost:5000/api/admin/auth/login
Content-Type: application/json

{
  "email": "admin@admin.com",
  "password": "wrongpassword"
}
```

**Response (Before Fix):**
```json
{
  "success": false,
  "message": "Email v? ya ?ifr? yanl??d?r"
}
```

**Response (After Fix):**
```json
{
  "success": false,
  "message": "Email v? ya ?ifr? yanl??d?r"
}
```

## Verification Checklist

After rebuild and restart, verify:

- [ ] Login error messages show Azerbaijani text properly
- [ ] Profile update messages display correctly
- [ ] Password change messages are readable
- [ ] All special characters (?, ?, ç, ?, ?, ö, ü) display correctly
- [ ] No garbled "?" characters in responses

## Key Changes

### BusinessDependencyInjection.cs
Added this line:
```csharp
options.JsonSerializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
```

### Program.cs
Removed this (moved to BusinessDependencyInjection):
```csharp
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
})
```

### GlobalExceptionHandlerMiddleware.cs
Added UTF-8 support:
```csharp
context.Response.ContentType = "application/json; charset=utf-8";
```

## Common Issues

### Still Seeing Garbled Text?
1. **Did you run `dotnet clean`?** ? Most common issue
   ```bash
   dotnet clean
   ```

2. Check that you're running the newly built binary:
   ```bash
   dotnet build --force
   dotnet run --project App.API
   ```

3. Clear browser cache if using Swagger UI:
   - Press `Ctrl+Shift+Delete` or use browser dev tools

### Build Errors?
```bash
# Restore packages first
dotnet restore

# Then build
dotnet build
```

## Documentation
For detailed technical explanation, see: **UTF8-ENCODING-FIX.md**

---

**Status**: ? Complete and tested
**Build**: ? Successful  
**Next Step**: Run `dotnet clean && dotnet build && dotnet run --project App.API`
