# Azerbaijan Timezone (UTC+4) Implementation

## Problemin T?sviri

?vv?lc? tarixl?r UTC olaraq saxlan?l?rd?, lakin Az?rbaycan saat qur?a?? UTC+4 oldu?u üçün:
- Hal-haz?rda saat **17:55** (Azerbaijan) olduqda
- Veril?nl?r bazas?nda **13:55 UTC** kimi saxlan?l?rd?
- API response-da is? **13:55** göst?rilirdi

## H?ll Yolu

Üç layerli sistem t?tbiq edildi:

### 1. **Database Layer** - UTC olaraq saxlan?r
- Bütün tarixl?r UTC format?nda veril?nl?r bazas?nda saxlan?l?r
- Bu beyn?lxalq standartd?r v? serverin yerind?n as?l? deyil

### 2. **Application Layer** - Azerbaijan vaxt? il? i?l?yir
- SaveChangesAsync zaman? Azerbaijan vaxt? UTC-y? çevrilir
- Veril?nl?r bazas?na UTC olaraq yaz?l?r

### 3. **API Layer** - Response-da Azerbaijan vaxt? qaytar?r
- JSON serialization zaman? UTC ? Azerbaijan (UTC+4) çevrilir
- Frontend-? düzgün vaxt göst?rilir

## Kodun Strukturu

### DateTimeHelper Class

```csharp
public static class DateTimeHelper
{
    // Azerbaijan timezone (UTC+4)
    private static readonly TimeZoneInfo AzerbaijanTimeZone = 
        TimeZoneInfo.CreateCustomTimeZone(
            "Azerbaijan Standard Time",
            TimeSpan.FromHours(4),
            "Azerbaijan Standard Time",
            "Azerbaijan Standard Time"
        );

    // Hal-haz?rk? Azerbaijan vaxt?
    public static DateTime GetAzerbaijanNow()
    {
        return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, AzerbaijanTimeZone);
    }

    // UTC ? Azerbaijan
    public static DateTime ToAzerbaijanTime(DateTime utcDateTime)
    {
        if (utcDateTime.Kind != DateTimeKind.Utc)
        {
            utcDateTime = DateTime.SpecifyKind(utcDateTime, DateTimeKind.Utc);
        }
        return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, AzerbaijanTimeZone);
    }

    // Azerbaijan ? UTC
    public static DateTime ToUtcFromAzerbaijan(DateTime azerbaijanDateTime)
    {
        return TimeZoneInfo.ConvertTimeToUtc(azerbaijanDateTime, AzerbaijanTimeZone);
    }
}
```

### AppDbContext - SaveChangesAsync

```csharp
public new async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
{
    var currentUserId = _claimService?.GetUserId() ?? "System";
    var azerbaijanNow = DateTimeHelper.GetAzerbaijanNow(); // 17:55
    var utcNow = DateTimeHelper.ToUtcFromAzerbaijan(azerbaijanNow); // 13:55 UTC

    foreach (var entry in ChangeTracker.Entries<IAuditedEntity>())
    {
        switch (entry.State)
        {
            case EntityState.Added:
                entry.Entity.CreatedOn = utcNow; // UTC olaraq saxlan?r
                entry.Entity.UpdatedOn = utcNow;
                break;
            case EntityState.Modified:
                entry.Entity.UpdatedOn = utcNow;
                break;
        }
    }

    return await base.SaveChangesAsync(cancellationToken);
}
```

### JSON Converter

```csharp
public class AzerbaijanDateTimeConverter : JsonConverter<DateTime>
{
    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        // UTC-d?n Azerbaijan vaxt?na çevir
        var azerbaijanTime = DateTimeHelper.ToAzerbaijanTime(value);
        writer.WriteStringValue(azerbaijanTime.ToString("yyyy-MM-ddTHH:mm:ss"));
    }
}
```

## Nümun?l?r

### Database-d? Saxlanma

```sql
-- Hal-haz?rk? vaxt: 17:55 (Azerbaijan)
-- Database-d? saxlan?r: 13:55 (UTC)

INSERT INTO Products (Name, CreatedOn, UpdatedOn)
VALUES ('M?hsul', '2024-12-04 13:55:00', '2024-12-04 13:55:00');
```

### API Response

```json
{
  "id": 1,
  "name": "M?hsul",
  "createdOn": "2024-12-04T17:55:00",  // Azerbaijan vaxt? (UTC+4)
  "updatedOn": "2024-12-04T17:55:00"
}
```

### Frontend-d? ?stifad?

```javascript
// API-d?n g?l?n tarix art?q Azerbaijan vaxt?d?r
const product = await fetch('/api/products/1').then(r => r.json());

console.log(product.createdOn); // "2024-12-04T17:55:00"

// JavaScript Date obyektin? çevirm?k
const createdDate = new Date(product.createdOn);
console.log(createdDate.toLocaleString('az-AZ')); // "04.12.2024, 17:55:00"
```

### C# ?stifad?si

```csharp
// 1. Hal-haz?rk? Azerbaijan vaxt?n? almaq
var azerbaijanNow = DateTimeHelper.GetAzerbaijanNow();
Console.WriteLine($"Azerbaijan: {azerbaijanNow}"); // 17:55

// 2. UTC-y? çevirm?k (database üçün)
var utcTime = DateTimeHelper.ToUtcFromAzerbaijan(azerbaijanNow);
Console.WriteLine($"UTC: {utcTime}"); // 13:55

// 3. UTC-d?n Azerbaijan-a çevirm?k (API response üçün)
var backToAzerbaijan = DateTimeHelper.ToAzerbaijanTime(utcTime);
Console.WriteLine($"Back to Azerbaijan: {backToAzerbaijan}"); // 17:55
```

### Service Layer-d? ?stifad?

```csharp
public class OrderService
{
    public async Task<OrderDTO> CreateOrderAsync(CreateOrderDTO dto)
    {
        var azerbaijanNow = DateTimeHelper.GetAzerbaijanNow();
        
        var order = new Order
        {
            OrderNumber = GenerateOrderNumber(),
            // UTC olaraq saxlanacaq (AppDbContext.SaveChangesAsync-da)
            CreatedOn = DateTimeHelper.ToUtcFromAzerbaijan(azerbaijanNow)
        };
        
        await _orderRepository.AddAsync(order);
        return MapToDTO(order);
    }
}
```

## ?sas Xüsusiyy?tl?r

### ? Avantajlar

1. **Database UTC-d?** - Beyn?lxalq standart, server timezone-dan as?l? deyil
2. **API Azerbaijan vaxt?** - ?stifad?çil?r üçün do?ru vaxt
3. **Avtomatik Konvertasiya** - JSON serialization zaman? avtomatik
4. **M?rk?zl??dirilmi?** - DateTimeHelper vasit?sil? ham? eyni metoddan istifad? edir
5. **Type-safe** - Compile-time s?hvl?ri tutur

### ?? N?z?r? Al?nmal?

1. **Database-d? UTC** - Bütün tarixl?r UTC-d? saxlan?r
2. **API Response Azerbaijan** - Frontend-? Azerbaijan vaxt? gönd?rilir
3. **Manual DateTime.Now istifad? etm?yin** - H?mi?? `DateTimeHelper.GetAzerbaijanNow()` istifad? edin
4. **DateTime.UtcNow ?v?zin?** - `DateTimeHelper.GetUtcNow()` v? ya `DateTimeHelper.ToUtcFromAzerbaijan()`

## S?naq Nümun?l?ri

### Scenario 1: M?hsul Yaratmaq

```csharp
// Hal-haz?rda saat 17:55 (Azerbaijan)
var product = new Product
{
    Name = "Test m?hsulu",
    // SaveChangesAsync-da avtomatik UTC-y? çevril?c?k
};

await _context.Products.AddAsync(product);
await _context.SaveChangesAsync();

// Database-d?: CreatedOn = '2024-12-04 13:55:00' (UTC)
// API Response: "createdOn": "2024-12-04T17:55:00" (Azerbaijan)
```

### Scenario 2: Login Zaman?

```csharp
// Login zaman? LastLoginAt yenil?nir
var azerbaijanNow = DateTimeHelper.GetAzerbaijanNow(); // 17:55
user.LastLoginAt = DateTimeHelper.ToUtcFromAzerbaijan(azerbaijanNow); // 13:55 UTC
await _userManager.UpdateAsync(user);

// Database: LastLoginAt = '2024-12-04 13:55:00' (UTC)
// API response zaman? avtomatik 17:55-? çevril?c?k
```

### Scenario 3: Token Expiration

```csharp
var tokenDto = new TokenDTO
{
    AccessToken = accessToken,
    RefreshToken = refreshToken,
    ExpiresAt = DateTimeHelper.GetAzerbaijanNow().AddMinutes(60) // 18:55 Azerbaijan
};

// JSON Response:
// "expiresAt": "2024-12-04T18:55:00"
```

## Frontend Integration

### React Example

```jsx
function ProductCard({ product }) {
  // API-d?n g?l?n tarix art?q Azerbaijan vaxt?d?r
  const createdDate = new Date(product.createdOn);
  
  return (
    <div>
      <h3>{product.name}</h3>
      <p>Yarad?l?b: {createdDate.toLocaleString('az-AZ')}</p>
      {/* Göst?ril?c?k: "04.12.2024, 17:55:00" */}
    </div>
  );
}
```

### JavaScript Date Formatting

```javascript
const formatAzerbaijanDate = (dateString) => {
  const date = new Date(dateString);
  
  return new Intl.DateTimeFormat('az-AZ', {
    year: 'numeric',
    month: '2-digit',
    day: '2-digit',
    hour: '2-digit',
    minute: '2-digit',
    second: '2-digit'
  }).format(date);
};

// Usage
const formattedDate = formatAzerbaijanDate(product.createdOn);
console.log(formattedDate); // "04.12.2024, 17:55:00"
```

### Moment.js Alternative (date-fns)

```javascript
import { format } from 'date-fns';
import { az } from 'date-fns/locale';

const formattedDate = format(
  new Date(product.createdOn),
  'dd MMMM yyyy, HH:mm',
  { locale: az }
);
// "04 Dekabr 2024, 17:55"
```

## Migration Notes

**?h?miyy?tli:** Mövcud data migration t?l?b etmir!

?g?r database-d? art?q UTC tarixl?r varsa:
- ? Heç bir d?yi?iklik laz?m deyil
- ? API avtomatik olaraq UTC+4 ?lav? ed?c?k
- ? Frontend düzgün vaxt? gör?c?k

?g?r database-d? Azerbaijan vaxt? varsa:
```sql
-- Bütün tarixl?ri UTC-y? çevirm?k
UPDATE Products SET CreatedOn = DATE_SUB(CreatedOn, INTERVAL 4 HOUR);
UPDATE Products SET UpdatedOn = DATE_SUB(UpdatedOn, INTERVAL 4 HOUR);
UPDATE Orders SET CreatedOn = DATE_SUB(CreatedOn, INTERVAL 4 HOUR);
-- v? s.
```

## Testing

### Unit Test Example

```csharp
[Fact]
public void DateTimeHelper_Should_Convert_To_Azerbaijan_Time()
{
    // Arrange
    var utcTime = new DateTime(2024, 12, 4, 13, 55, 0, DateTimeKind.Utc);
    
    // Act
    var azerbaijanTime = DateTimeHelper.ToAzerbaijanTime(utcTime);
    
    // Assert
    Assert.Equal(17, azerbaijanTime.Hour);
    Assert.Equal(55, azerbaijanTime.Minute);
}

[Fact]
public void DateTimeHelper_Should_Convert_To_UTC()
{
    // Arrange
    var azerbaijanTime = new DateTime(2024, 12, 4, 17, 55, 0);
    
    // Act
    var utcTime = DateTimeHelper.ToUtcFromAzerbaijan(azerbaijanTime);
    
    // Assert
    Assert.Equal(13, utcTime.Hour);
    Assert.Equal(55, utcTime.Minute);
    Assert.Equal(DateTimeKind.Utc, utcTime.Kind);
}
```

## Troubleshooting

### Problem: API-d? h?l? d? UTC vaxt? göst?rir

**H?ll:**
1. `BusinessDependencyInjection`-da converter ?lav? olunub?
2. Application yenid?n ba?lad?l?b?
3. Browser cache t?mizl?nib?

### Problem: Database-d? yanl?? vaxt saxlan?r

**H?ll:**
1. `AppDbContext.SaveChangesAsync` yenil?nib?
2. `DateTimeHelper.ToUtcFromAzerbaijan()` istifad? olunur?

### Problem: B?zi tarixl?r düzgün, b?zil?ri s?hv

**H?ll:**
1. Bütün DateTime.Now istifad?l?rini tap
2. `DateTimeHelper.GetAzerbaijanNow()` il? ?v?z et
3. Manual UTC t?yin edil?n yerl?ri yoxla

## Best Practices

1. ? **H?mi?? DateTimeHelper istifad? et**
   ```csharp
   // YAX??
   var now = DateTimeHelper.GetAzerbaijanNow();
   
   // PISS
   var now = DateTime.Now;
   ```

2. ? **Database üçün UTC saxla**
   ```csharp
   // YAX??
   entity.CreatedOn = DateTimeHelper.ToUtcFromAzerbaijan(azerbaijanNow);
   
   // P?S
   entity.CreatedOn = azerbaijanNow;
   ```

3. ? **API Response-da avtomatik konvertasiya**
   ```csharp
   // JSON Converter avtomatik i?l?yir
   // ?lav? kod yazma?a ehtiyac yoxdur
   ```

4. ? **Nullable DateTime üçün**
   ```csharp
   DateTime? lastLogin = user.LastLoginAt.HasValue 
       ? DateTimeHelper.ToAzerbaijanTime(user.LastLoginAt.Value)
       : null;
   ```

## Summary

?? **N?tic?:**
- Database: UTC (13:55)
- API Response: Azerbaijan (17:55)
- Frontend: Do?ru vaxt göst?rir

? **T?tbiq olundu:**
- DateTimeHelper class
- AppDbContext konvertasiyas?
- JSON serialization converter
- AuthService yenil?nm?si

?? **S?n?dl?r:**
- Bu fayl: AZERBAIJAN-TIMEZONE-IMPLEMENTATION.md
- Kod: App.Core/Helpers/DateTimeHelper.cs
- Converter: App.Business/Converters/AzerbaijanDateTimeConverter.cs
