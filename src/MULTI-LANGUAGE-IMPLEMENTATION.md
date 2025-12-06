# Multi-Language Support Implementation

## Overview
The application now supports **3 languages** for Categories and Products:
- **az** - Azerbaijani (default)
- **en** - English
- **ru** - Russian

## Database Structure

### New Tables

#### CategoryTranslations
| Column | Type | Description |
|--------|------|-------------|
| Id | int | Primary key |
| CategoryId | int | Foreign key to Category |
| LanguageCode | string(5) | "az", "en", or "ru" |
| Name | string(100) | Translated category name |

#### ProductTranslations
| Column | Type | Description |
|--------|------|-------------|
| Id | int | Primary key |
| ProductId | int | Foreign key to Product |
| LanguageCode | string(5) | "az", "en", or "ru" |
| Name | string(200) | Translated product name |
| Description | string(2000) | Translated description |
| Type | string(100) | Translated type |

### Updated Entities

**Category:**
- Name (default Azerbaijani)
- Translations collection (CategoryTranslation)

**Product:**
- Name (default Azerbaijani)
- Type (default Azerbaijani)
- Description (default Azerbaijani)
- Translations collection (ProductTranslation)

## Migration Required

After stopping the application, run:

```bash
cd C:\Users\Emil\source\repos\Bernessa\App\src\App.DAL
dotnet ef migrations add AddMultiLanguageSupport --startup-project ..\App.API\App.API.csproj
dotnet ef database update --startup-project ..\App.API\App.API.csproj
```

## API Changes

### Creating Categories with Translations

**Endpoint:** `POST /api/categories`  
**Content-Type:** `multipart/form-data`

**Form Data:**
```
Name: D?ri bax?m?              (Azerbaijani - required)
NameEn: Skincare              (English - optional)
NameRu: ???? ?? ?????         (Russian - optional)
Slug: skincare
Image: [file]
```

**Response:**
```json
{
  "success": true,
  "data": {
    "id": 1,
    "name": "D?ri bax?m?",
    "slug": "skincare",
    "image": "/images/skincare.jpg",
    "productCount": 0,
    "translations": {
      "az": "D?ri bax?m?",
      "en": "Skincare",
      "ru": "???? ?? ?????"
    }
  }
}
```

### Creating Products with Translations

**Endpoint:** `POST /api/products`  
**Content-Type:** `multipart/form-data`

**Form Data:**
```
Name: N?ml?ndirici krem                    (Azerbaijani - required)
NameEn: Moisturizing Cream                 (English - optional)
NameRu: ??????????? ????                   (Russian - optional)

Type: Krem                                 (Azerbaijani - required)
TypeEn: Cream                              (English - optional)
TypeRu: ????                               (Russian - optional)

Description: Bütün d?ri tipl?r üçün...     (Azerbaijani - required)
DescriptionEn: For all skin types...       (English - optional)
DescriptionRu: ??? ???? ????? ????...      (Russian - optional)

Brand: Brand Name
Price: 45.99
Volume: 50ml
CategoryId: 1
Stock: 100
Images: [file1, file2]
```

**Response:**
```json
{
  "success": true,
  "data": {
    "id": 15,
    "name": "N?ml?ndirici krem",
    "brand": "Brand Name",
    "price": 45.99,
    "volume": "50ml",
    "type": "Krem",
    "description": "Bütün d?ri tipl?ri üçün...",
    "images": ["/images/img1.jpg", "/images/img2.jpg"],
    "categoryId": 1,
    "categoryName": "D?ri bax?m?",
    "stock": 100,
    "isActive": true,
    "isFeatured": false,
    "translations": {
      "az": {
        "languageCode": "az",
        "name": "N?ml?ndirici krem",
        "description": "Bütün d?ri tipl?ri üçün...",
        "type": "Krem"
      },
      "en": {
        "languageCode": "en",
        "name": "Moisturizing Cream",
        "description": "For all skin types...",
        "type": "Cream"
      },
      "ru": {
        "languageCode": "ru",
        "name": "??????????? ????",
        "description": "??? ???? ????? ????...",
        "type": "????"
      }
    }
  }
}
```

## Frontend Integration

### JavaScript Example

```javascript
// Get product with all translations
const getProduct = async (productId) => {
  const response = await fetch(`/api/products/${productId}`);
  const result = await response.json();
  
  if (result.success) {
    const product = result.data;
    
    // Get current language from localStorage or user preference
    const currentLang = localStorage.getItem('language') || 'az';
    
    // Use translated data
    const translation = product.translations[currentLang];
    
    return {
      id: product.id,
      name: translation.name,
      description: translation.description,
      type: translation.type,
      brand: product.brand,
      price: product.price,
      images: product.images
    };
  }
};
```

### React Component Example

```jsx
import { useState, useEffect } from 'react';

function ProductCard({ productId }) {
  const [product, setProduct] = useState(null);
  const [language, setLanguage] = useState('az');

  useEffect(() => {
    fetch(`/api/products/${productId}`)
      .then(res => res.json())
      .then(result => {
        if (result.success) {
          setProduct(result.data);
        }
      });
  }, [productId]);

  if (!product) return <div>Loading...</div>;

  const translation = product.translations[language];

  return (
    <div className="product-card">
      {/* Language Switcher */}
      <div className="language-switcher">
        <button onClick={() => setLanguage('az')}>AZ</button>
        <button onClick={() => setLanguage('en')}>EN</button>
        <button onClick={() => setLanguage('ru')}>RU</button>
      </div>

      {/* Product Info */}
      <img src={product.images[0]} alt={translation.name} />
      <h3>{translation.name}</h3>
      <p className="type">{translation.type}</p>
      <p className="description">{translation.description}</p>
      <p className="brand">{product.brand}</p>
      <p className="price">{product.price} AZN</p>
    </div>
  );
}
```

### Vue.js Example

```vue
<template>
  <div class="product-card">
    <!-- Language Switcher -->
    <div class="language-switcher">
      <button @click="currentLang = 'az'" :class="{ active: currentLang === 'az' }">AZ</button>
      <button @click="currentLang = 'en'" :class="{ active: currentLang === 'en' }">EN</button>
      <button @click="currentLang = 'ru'" :class="{ active: currentLang === 'ru' }">RU</button>
    </div>

    <!-- Product Info -->
    <img :src="product.images[0]" :alt="currentTranslation.name" />
    <h3>{{ currentTranslation.name }}</h3>
    <p class="type">{{ currentTranslation.type }}</p>
    <p class="description">{{ currentTranslation.description }}</p>
    <p class="brand">{{ product.brand }}</p>
    <p class="price">{{ product.price }} AZN</p>
  </div>
</template>

<script>
export default {
  props: {
    productId: Number
  },
  data() {
    return {
      product: null,
      currentLang: 'az'
    };
  },
  computed: {
    currentTranslation() {
      if (!this.product) return {};
      return this.product.translations[this.currentLang];
    }
  },
  async mounted() {
    const response = await fetch(`/api/products/${this.productId}`);
    const result = await response.json();
    
    if (result.success) {
      this.product = result.data;
    }
  }
};
</script>
```

## Creating Multi-Language Products (Frontend)

```javascript
const createProduct = async (formData, translations) => {
  const form = new FormData();
  
  // Basic fields (Azerbaijani)
  form.append('Name', formData.nameAz);
  form.append('Type', formData.typeAz);
  form.append('Description', formData.descriptionAz);
  
  // English translations (optional)
  if (translations.en) {
    form.append('NameEn', translations.en.name);
    form.append('TypeEn', translations.en.type);
    form.append('DescriptionEn', translations.en.description);
  }
  
  // Russian translations (optional)
  if (translations.ru) {
    form.append('NameRu', translations.ru.name);
    form.append('TypeRu', translations.ru.type);
    form.append('DescriptionRu', translations.ru.description);
  }
  
  // Other fields
  form.append('Brand', formData.brand);
  form.append('Price', formData.price);
  form.append('Volume', formData.volume);
  form.append('CategoryId', formData.categoryId);
  form.append('Stock', formData.stock);
  
  // Images
  formData.images.forEach(image => {
    form.append('Images', image);
  });
  
  const response = await fetch('/api/products', {
    method: 'POST',
    headers: {
      'Authorization': `Bearer ${token}`
    },
    body: form
  });
  
  return await response.json();
};

// Usage
const result = await createProduct(
  {
    nameAz: 'N?ml?ndirici krem',
    typeAz: 'Krem',
    descriptionAz: 'Bütün d?ri tipl?ri üçün',
    brand: 'Brand Name',
    price: 45.99,
    volume: '50ml',
    categoryId: 1,
    stock: 100,
    images: [file1, file2]
  },
  {
    en: {
      name: 'Moisturizing Cream',
      type: 'Cream',
      description: 'For all skin types'
    },
    ru: {
      name: '??????????? ????',
      type: '????',
      description: '??? ???? ????? ????'
    }
  }
);
```

## React Form Example

```jsx
function MultiLanguageProductForm() {
  const [activeTab, setActiveTab] = useState('az');
  const [formData, setFormData] = useState({
    az: { name: '', type: '', description: '' },
    en: { name: '', type: '', description: '' },
    ru: { name: '', type: '', description: '' },
    brand: '',
    price: '',
    volume: '',
    categoryId: '',
    stock: '',
    images: []
  });

  const handleSubmit = async (e) => {
    e.preventDefault();
    
    const form = new FormData();
    
    // Azerbaijani (required)
    form.append('Name', formData.az.name);
    form.append('Type', formData.az.type);
    form.append('Description', formData.az.description);
    
    // English (optional)
    if (formData.en.name) {
      form.append('NameEn', formData.en.name);
      form.append('TypeEn', formData.en.type);
      form.append('DescriptionEn', formData.en.description);
    }
    
    // Russian (optional)
    if (formData.ru.name) {
      form.append('NameRu', formData.ru.name);
      form.append('TypeRu', formData.ru.type);
      form.append('DescriptionRu', formData.ru.description);
    }
    
    // Other fields
    form.append('Brand', formData.brand);
    form.append('Price', formData.price);
    form.append('Volume', formData.volume);
    form.append('CategoryId', formData.categoryId);
    form.append('Stock', formData.stock);
    
    // Images
    formData.images.forEach(img => form.append('Images', img));
    
    const response = await fetch('/api/products', {
      method: 'POST',
      headers: { 'Authorization': `Bearer ${token}` },
      body: form
    });
    
    const result = await response.json();
    console.log(result);
  };

  return (
    <form onSubmit={handleSubmit}>
      {/* Language Tabs */}
      <div className="language-tabs">
        <button type="button" onClick={() => setActiveTab('az')} 
                className={activeTab === 'az' ? 'active' : ''}>
          Azerbaijani *
        </button>
        <button type="button" onClick={() => setActiveTab('en')} 
                className={activeTab === 'en' ? 'active' : ''}>
          English
        </button>
        <button type="button" onClick={() => setActiveTab('ru')} 
                className={activeTab === 'ru' ? 'active' : ''}>
          Russian
        </button>
      </div>

      {/* Translation Fields */}
      <div className="tab-content">
        {activeTab === 'az' && (
          <div>
            <input
              type="text"
              placeholder="Ad (Az?rbaycan dilind?) *"
              value={formData.az.name}
              onChange={e => setFormData({
                ...formData,
                az: { ...formData.az, name: e.target.value }
              })}
              required
            />
            <input
              type="text"
              placeholder="Növ *"
              value={formData.az.type}
              onChange={e => setFormData({
                ...formData,
                az: { ...formData.az, type: e.target.value }
              })}
              required
            />
            <textarea
              placeholder="T?svir *"
              value={formData.az.description}
              onChange={e => setFormData({
                ...formData,
                az: { ...formData.az, description: e.target.value }
              })}
              required
            />
          </div>
        )}
        
        {activeTab === 'en' && (
          <div>
            <input
              type="text"
              placeholder="Name (English)"
              value={formData.en.name}
              onChange={e => setFormData({
                ...formData,
                en: { ...formData.en, name: e.target.value }
              })}
            />
            <input
              type="text"
              placeholder="Type"
              value={formData.en.type}
              onChange={e => setFormData({
                ...formData,
                en: { ...formData.en, type: e.target.value }
              })}
            />
            <textarea
              placeholder="Description"
              value={formData.en.description}
              onChange={e => setFormData({
                ...formData,
                en: { ...formData.en, description: e.target.value }
              })}
            />
          </div>
        )}
        
        {activeTab === 'ru' && (
          <div>
            <input
              type="text"
              placeholder="???????? (???????)"
              value={formData.ru.name}
              onChange={e => setFormData({
                ...formData,
                ru: { ...formData.ru, name: e.target.value }
              })}
            />
            <input
              type="text"
              placeholder="???"
              value={formData.ru.type}
              onChange={e => setFormData({
                ...formData,
                ru: { ...formData.ru, type: e.target.value }
              })}
            />
            <textarea
              placeholder="????????"
              value={formData.ru.description}
              onChange={e => setFormData({
                ...formData,
                ru: { ...formData.ru, description: e.target.value }
              })}
            />
          </div>
        )}
      </div>

      {/* Common Fields */}
      <input type="text" placeholder="Brand" value={formData.brand}
             onChange={e => setFormData({ ...formData, brand: e.target.value })} />
      <input type="number" placeholder="Price" value={formData.price}
             onChange={e => setFormData({ ...formData, price: e.target.value })} />
      
      {/* ... other fields ... */}
      
      <button type="submit">Create Product</button>
    </form>
  );
}
```

## Best Practices

### 1. Default Language
- Always provide Azerbaijani content (required)
- English and Russian are optional
- Frontend should fallback to Azerbaijani if translation missing

### 2. Language Detection
```javascript
// Get user's preferred language
const getUserLanguage = () => {
  // 1. Check localStorage
  const saved = localStorage.getItem('language');
  if (saved && ['az', 'en', 'ru'].includes(saved)) {
    return saved;
  }
  
  // 2. Check browser language
  const browserLang = navigator.language.split('-')[0];
  if (['az', 'en', 'ru'].includes(browserLang)) {
    return browserLang;
  }
  
  // 3. Default to Azerbaijani
  return 'az';
};
```

### 3. Translation Helper
```javascript
const t = (item, field, lang = 'az') => {
  if (!item || !item.translations) return '';
  
  // Try to get translation for current language
  const translation = item.translations[lang];
  if (translation && translation[field]) {
    return translation[field];
  }
  
  // Fallback to Azerbaijani
  const azTranslation = item.translations['az'];
  if (azTranslation && azTranslation[field]) {
    return azTranslation[field];
  }
  
  // Fallback to direct field
  return item[field] || '';
};

// Usage
const productName = t(product, 'name', currentLang);
const productDescription = t(product, 'description', currentLang);
```

### 4. SEO Considerations
```html
<!-- Set language attribute on HTML tag -->
<html lang="az">

<!-- Add alternate language links -->
<link rel="alternate" hreflang="az" href="https://example.com/az/products/1" />
<link rel="alternate" hreflang="en" href="https://example.com/en/products/1" />
<link rel="alternate" hreflang="ru" href="https://example.com/ru/products/1" />

<!-- Use translated content in meta tags -->
<meta name="description" content="Product description in current language" />
<meta property="og:title" content="Product name in current language" />
```

## Features

? **3 Languages Supported** - Azerbaijani, English, Russian
? **Optional Translations** - Only Azerbaijani is required
? **Cascading Fallback** - Falls back to Azerbaijani if translation missing
? **Full CRUD Support** - Create, Read, Update, Delete with translations
? **Automatic Inclusion** - Translations loaded with products/categories
? **Flexible API** - Accept any combination of translations

## Testing

```bash
# Create category with all translations
curl -X POST http://localhost:5000/api/categories \
  -H "Authorization: Bearer TOKEN" \
  -F "Name=D?ri bax?m?" \
  -F "NameEn=Skincare" \
  -F "NameRu=???? ?? ?????" \
  -F "Slug=skincare" \
  -F "Image=@image.jpg"

# Create product with partial translations
curl -X POST http://localhost:5000/api/products \
  -H "Authorization: Bearer TOKEN" \
  -F "Name=Krem" \
  -F "NameEn=Cream" \
  -F "Brand=Brand" \
  -F "Price=45.99" \
  -F "Volume=50ml" \
  -F "Type=Krem" \
  -F "Description=T?svir" \
  -F "CategoryId=1" \
  -F "Stock=100"
```

## Notes

- Translations are stored in separate tables for better normalization
- Each language code is unique per product/category
- Deleting a product/category cascades to delete translations
- Update operations merge translations (add new, update existing, remove empty)
- All existing endpoints now return translations automatically
