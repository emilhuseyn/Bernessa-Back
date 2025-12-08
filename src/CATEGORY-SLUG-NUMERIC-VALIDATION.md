# Category Slug Numeric Validation Fix

## Problem
Category slugs could be just numbers (e.g., "123"), which creates routing conflicts with the `/api/categories/{id}` endpoint that expects an integer ID.

## Solution
Implemented validation to prevent slugs from being purely numeric values at both the DTO and service layer.

## Changes Made

### 1. Created Custom Validation Attribute
**File**: `App.Business/ValidationAttributes/NotNumericAttribute.cs`
- New validation attribute that checks if a string value is purely numeric
- Returns validation error with Azerbaijani message: "Slug sad?c? r?q?m ola bilm?z"

### 2. Updated CreateCategoryDTO
**File**: `App.Business/DTOs/Categories/CreateCategoryDTO.cs`
- Added `[NotNumeric]` attribute to `Slug` property
- Provides client-side validation through model state

### 3. Updated CategoryService
**File**: `App.Business/Services/Implementations/CategoryService.cs`
- Added validation in `CreateCategoryAsync()` method
- Added validation in `UpdateCategoryAsync()` method
- Both methods check if slug is numeric and throw exception with message: "Slug sad?c? r?q?m ola bilm?z"

## How It Works

1. **DTO Level**: The `[NotNumeric]` attribute validates the slug when the model is bound
2. **Service Level**: Additional validation ensures no numeric slugs are created even if DTO validation is bypassed

## Examples

### ? Invalid Slugs (Will be rejected)
- "123"
- "456789"
- "0"

### ? Valid Slugs (Will be accepted)
- "parfum-123"
- "category-1"
- "new-arrivals"
- "123-special-offer"

## Testing

After creating or updating a category with a numeric slug, you will receive:
```json
{
  "success": false,
  "message": "Slug sad?c? r?q?m ola bilm?z"
}
```

## Benefits

1. **Prevents Routing Conflicts**: Ensures `/api/categories/{slug}` and `/api/categories/{id}` endpoints work correctly
2. **Better SEO**: Forces descriptive slugs instead of just numbers
3. **User-Friendly**: Clear error messages in Azerbaijani
4. **Multi-Layer Protection**: Validation at both DTO and service layers
