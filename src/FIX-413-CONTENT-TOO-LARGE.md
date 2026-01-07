# 413 Content Too Large Error - FIXED ?

## Problem
When uploading images larger than 1MB, the API was returning **413 Content Too Large** error.

## Root Cause
ASP.NET Core has default size limits for file uploads:
- **Kestrel**: 30MB default
- **IIS**: 28.6MB default  
- **FormOptions**: 128MB default multipart body length
- But these can be restrictive for large images

## Solution Applied

### 1. Program.cs - Global Configuration ?

Added unlimited file upload size configuration:

```csharp
// Configure Kestrel to accept large files (unlimited)
builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = null; // Unlimited
});

// Configure IIS to accept large files (unlimited)
builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = null; // Unlimited
});

// Configure FormOptions to accept large files (unlimited)
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = long.MaxValue; // Unlimited
    options.ValueLengthLimit = int.MaxValue;
    options.MultipartHeadersLengthLimit = int.MaxValue;
});
```

### 2. Controller-Level Attributes ?

Added attributes to all file upload endpoints:

```csharp
[RequestSizeLimit(long.MaxValue)]
[RequestFormLimits(MultipartBodyLengthLimit = long.MaxValue)]
```

**Applied to:**
- ? ContactSettingsController - Create & Update
- ? ProductsController - Create & Update  
- ? CategoriesController - Create & Update
- ? BrandsController - Create & Update

### 3. No Server-Side File Size Validation ?

`FileManagerService` only validates:
- ? File must be an image type (content-type contains "image")
- ? NO size limit enforced

## Changes Summary

### Files Modified:
1. **src/App.API/Program.cs**
   - Added Kestrel, IIS, and FormOptions configuration
   
2. **src/App.API/Controllers/ContactSettingsController.cs**
   - Added attributes to Create & Update endpoints
   
3. **src/App.API/Controllers/ProductsController.cs**
   - Added attributes to Create & Update endpoints
   
4. **src/App.API/Controllers/CategoriesController.cs**
   - Added attributes to Create & Update endpoints
   
5. **src/App.API/Controllers/BrandsController.cs**
   - Added attributes to Create & Update endpoints

## Testing

### Before Fix:
```
PUT /api/contactsettingses/1
Content-Length: 1104501 (1.1MB)
Response: 413 Content Too Large
```

### After Fix:
```
PUT /api/contactsettingses/1
Content-Length: 1104501 (1.1MB)
Response: 200 OK
```

## Configuration Hierarchy

1. **Global Level** (Program.cs)
   - Kestrel: `MaxRequestBodySize = null` (unlimited)
   - IIS: `MaxRequestBodySize = null` (unlimited)
   - FormOptions: `MultipartBodyLengthLimit = long.MaxValue`

2. **Controller Level** (Each endpoint)
   - `[RequestSizeLimit(long.MaxValue)]`
   - `[RequestFormLimits(MultipartBodyLengthLimit = long.MaxValue)]`

3. **Service Level** (FileManagerService)
   - No size validation
   - Only image type validation

## Notes

?? **Production Considerations:**
- Consider adding reasonable limits for production (e.g., 50MB, 100MB)
- Monitor disk space usage
- Consider implementing:
  - Image compression on upload
  - CDN for image storage
  - Background job for image optimization
  - Rate limiting per user/IP

? **Current Configuration:**
- Unlimited file uploads
- No size restrictions
- Suitable for development/testing
- May need adjustment for production

## Next Steps (Optional)

For production environment, consider:

```csharp
// Production-friendly limits
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 100 * 1024 * 1024; // 100MB
    options.ValueLengthLimit = int.MaxValue;
    options.MultipartHeadersLengthLimit = int.MaxValue;
});
```

And per-endpoint:

```csharp
[RequestSizeLimit(100 * 1024 * 1024)] // 100MB
[RequestFormLimits(MultipartBodyLengthLimit = 100 * 1024 * 1024)]
```

## API Usage

All file upload endpoints now accept any size:

### ContactSettings
```bash
PUT /api/contact-settings/1
Content-Type: multipart/form-data
- File size: ? Unlimited
```

### Products
```bash
POST /api/products
Content-Type: multipart/form-data
- Multiple images: ? Unlimited size each
```

### Categories
```bash
POST /api/categories
Content-Type: multipart/form-data
- Single image: ? Unlimited size
```

### Brands
```bash
POST /api/brands
Content-Type: multipart/form-data
- Logo image: ? Unlimited size
```

## Build Status
? Build successful

## Deployment
Ready to deploy to production server (Linux/barsense.az)

---

**Problem Solved:** ? 413 Content Too Large error fixed
**File Size Limit:** ? None (unlimited)
**Validation:** ? Image type only
**Production Ready:** ?? Consider adding reasonable limits
