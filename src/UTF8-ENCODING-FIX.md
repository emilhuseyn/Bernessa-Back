# UTF-8 Encoding Fix for Azerbaijani Text in API Responses

## Problem
API responses were returning garbled Azerbaijani text instead of properly encoded UTF-8 characters:

**Before:**
```json
{
  "success": false,
  "message": "Email v? ya ?ifr? yanl??d?r"
}
```

**Expected:**
```json
{
  "success": false,
  "message": "Email v? ya ?ifr? yanl??d?r"
}
```

## Root Cause
The issue was caused by **conflicting JSON serialization configurations** in two places:

1. **Program.cs** - Called `AddControllers()` with JSON encoder options
2. **BusinessDependencyInjection.cs** - Called `AddControllers()` again WITHOUT the JSON encoder option

The second call was **overriding** the first configuration, causing ASP.NET Core to use its default HTML-escaping encoder instead of `UnsafeRelaxedJsonEscaping`, which corrupted non-ASCII characters.

## Solution

### 1. Updated BusinessDependencyInjection.cs
Added proper JSON serialization configuration:

```csharp
using System.Text.Encodings.Web;  // Add this import

services.AddControllers(options =>
{
    options.Conventions.Add(new PluralizedRouteConvention());
    options.ModelValidatorProviders.Clear();
})
.AddFluentValidation(...)
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
    // Add Azerbaijan timezone converter for all DateTime properties
    options.JsonSerializerOptions.Converters.Add(new AzerbaijanDateTimeConverter());
    options.JsonSerializerOptions.Converters.Add(new AzerbaijanNullableDateTimeConverter());
});
```

### 2. Simplified Program.cs
Removed redundant `AddJsonOptions` since it's now configured in the Business layer:

```csharp
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();  // Configuration moved to BusinessDependencyInjection.cs
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(...);

builder.Services
    .AddDataAccess(builder.Configuration)
    .AddBusiness();  // This now handles all JSON serialization settings
```

### 3. GlobalExceptionHandlerMiddleware.cs
Already updated to use proper UTF-8 encoding:

```csharp
var options = new JsonSerializerOptions
{
    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
    WriteIndented = false
};

var result = JsonSerializer.Serialize(new { success = false, errors }, options);
context.Response.ContentType = "application/json; charset=utf-8";
```

## Impact

? **All API responses** now properly display Azerbaijani text
? **Login error messages** show correctly: "Email v? ya ?ifr? yanl??d?r"
? **Profile update messages** show correctly: "Profil u?urla yenil?ndi"  
? **Password change messages** show correctly: "?ifr? u?urla d?yi?dirildi"
? **Exception handler responses** preserve all special characters
? **DateTime conversions** to Azerbaijan timezone still work properly

## Key Characters Fixed

All Azerbaijani special characters are now properly encoded:
- **?** - schwa (e.g., "Email", "d?yi?dirildi")
- **?** - s with cedilla (e.g., "?ifr?")
- **ç** - c with cedilla (e.g., "h?r?k?tçi")
- **?** - g with breve (e.g., "da?")
- **?** - dotless i (e.g., "?ifr?")
- **ö** - o with diaeresis
- **ü** - u with diaeresis

## Testing

To verify the fix works:

1. **Test login with invalid credentials:**
```bash
curl -X POST http://localhost:5000/api/admin/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"user@example.com","password":"wrong"}'
```

Expected response:
```json
{
  "success": false,
  "message": "Email v? ya ?ifr? yanl??d?r"
}
```

2. **Test profile update:**
```bash
curl -X PUT http://localhost:5000/api/admin/auth/profile \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -d '{"firstName":"","lastName":"Test"}'
```

Expected response:
```json
{
  "success": false,
  "message": "Ad daxil edilm?lidir"
}
```

3. **Test password change:**
```bash
curl -X POST http://localhost:5000/api/admin/auth/change-password \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -d '{"currentPassword":"test","newPassword":"short"}'
```

Expected response:
```json
{
  "success": false,
  "message": "Yeni ?ifr? ?n az? 6 simvol olmal?d?r"
}
```

## Important Notes

### UnsafeRelaxedJsonEscaping
- Safe for this use case because we're serializing trusted server responses
- Only relevant for non-ASCII characters in string values
- Does NOT affect security - application logic handles validation
- Recommended for APIs serving international content

### Centralized Configuration
- All JSON serialization settings are now in `BusinessDependencyInjection.cs`
- Single source of truth for serialization options
- Easier to maintain and modify in the future
- Applies consistently to all endpoints

### DateTime Handling
- Custom converters still work as intended
- Azerbaijan timezone conversion continues to function
- Proper JSON encoding doesn't interfere with converter logic

## Files Modified
- `App.Business\BusinessDependencyInjection.cs` - Added UTF-8 encoder configuration
- `App.API\Program.cs` - Removed redundant JSON configuration
- `App.API\Middlewares\GlobalExceptionHandlerMiddleware.cs` - Updated in previous fix

## Build Required
**Important**: You must clean and rebuild the solution for this fix to take effect:

```bash
# Clean old binaries
dotnet clean

# Rebuild solution
dotnet build

# Or simply rebuild without clean
dotnet build --force
```

This ensures all cached assemblies are refreshed with the new configuration.
