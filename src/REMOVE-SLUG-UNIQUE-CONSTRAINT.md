# Remove Slug Unique Constraint

## Problem
Kateqoriya slug-lar? unique idi v? eyni slug-dan bir neç? d?f? istifad? etm?k mümkün deyildi.

## H?ll
Database-d?n slug unique constraint-i silindi. ?ndi eyni slug-dan bir neç? kateqoriya üçün istifad? ed? bil?rsiniz.

## D?yi?iklikl?r

### 1. CategoryConfiguration
**Fayl**: `App.DAL/Configurations/CategoryConfiguration.cs`

**?vv?l**:
```csharp
builder.HasIndex(c => c.Slug)
    .IsUnique();
```

**?ndi**:
```csharp
builder.HasIndex(c => c.Slug);
```

- `.IsUnique()` metodu silindi
- Index qald? (performans üçün)
- Art?q slug unique deyil

### 2. Migration
**Fayl**: `App.DAL/Migrations/20251208224947_RemoveSlugUniqueConstraint.cs`

Migration a?a??dak? ?m?liyyatlar? yerin? yetirir:
1. Köhn? unique index-i silir
2. Yeni non-unique index yarad?r

## N?tic?

### ? ?ndi Mümkündür
```json
// Kateqoriya 1
{
  "name": "Parfüm Ki?i",
  "slug": "parfum"
}

// Kateqoriya 2 (eyni slug)
{
  "name": "Parfüm Qad?n", 
  "slug": "parfum"
}
```

### ?? Diqq?t
- Eyni slug-a malik çox kateqoriya ola bil?r
- Slug h?l? d? r?q?m ola bilm?z (NotNumeric validation aktiv)
- GetBySlug endpoint ilk tap?lan kateqoriyan? qaytaracaq

## Database Update
```bash
cd App.DAL
dotnet ef migrations add RemoveSlugUniqueConstraint --startup-project ../App.API
dotnet ef database update --startup-project ../App.API
```

## T?sdiql?ndi ?
- Migration yarad?ld?: `20251208224947_RemoveSlugUniqueConstraint.cs`
- Database update olundu
- Unique constraint silindi
- Index qald? (performans üçün)
