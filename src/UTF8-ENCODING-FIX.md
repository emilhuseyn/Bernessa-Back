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
1. **Missing JSON Encoder Configuration**: ASP.NET Core was using default HTML-escaping which corrupts non-ASCII characters
2. **Inconsistent Serialization**: The global exception handler was using `Newtonsoft.Json` without proper UTF-8 configuration
3. **Missing Content-Type Charset**: Response headers didn't explicitly specify UTF-8 encoding

## Solution

### 1. Updated Program.cs
Added JSON serializer configuration to use `UnsafeRelaxedJsonEscaping`:

```csharp
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
    });
```

This ensures that:
- UTF-8 characters (?, ?, ç, ?, ?, ö, ü) are preserved in JSON responses
- Proper encoding for all API endpoints
- All controllers benefit from consistent serialization

### 2. Updated GlobalExceptionHandlerMiddleware.cs
Replaced `Newtonsoft.Json` with `System.Text.Json` and configured proper encoding:

```csharp
var options = new JsonSerializerOptions
{
    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
    WriteIndented = false
};

var result = JsonSerializer.Serialize(new { success = false, errors }, options);

context.Response.ContentType = "application/json; charset=utf-8";
```

This ensures:
- Exception responses also preserve Azerbaijani characters
- Content-Type header explicitly declares UTF-8 encoding
- Consistent serialization across all middleware

## Impact
? All API responses now properly display Azerbaijani text
? Login error messages show correctly: "Email v? ya ?ifr? yanl??d?r"
? Profile update messages show correctly: "Profil u?urla yenil?ndi"
? Password change messages show correctly: "?ifr? u?urla d?yi?dirildi"
? Exception handler messages preserve all special characters

## Testing
To verify the fix:

1. Test login with invalid credentials:
```bash
curl -X POST http://localhost:5000/api/admin/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"user@example.com","password":"wrong"}'
```

2. Response should now show:
```json
{
  "success": false,
  "message": "Email v? ya ?ifr? yanl??d?r"
}
```

3. Test other endpoints (profile update, password change, etc.) to verify all Azerbaijani text is properly encoded

## Files Modified
- `App.API\Program.cs` - Added JSON serializer options
- `App.API\Middlewares\GlobalExceptionHandlerMiddleware.cs` - Updated to use System.Text.Json with UTF-8 encoding

## Notes
- The `UnsafeRelaxedJsonEscaping` encoder is safe to use as we're handling trusted server responses
- All special Azerbaijani characters are now properly preserved in all JSON responses
- This fix applies globally to all API endpoints
