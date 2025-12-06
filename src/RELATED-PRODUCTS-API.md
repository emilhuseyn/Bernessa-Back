# Related Products API

## Overview
API endpoint to get related products from the same category as a given product. Perfect for "You may also like" or "Similar products" sections.

## Endpoint

**GET** `/api/products/{id}/related`

**Parameters:**
- `id` (path, required) - The product ID to find related products for
- `limit` (query, optional) - Maximum number of related products to return (default: 8)

**Authorization:** Not required (public endpoint)

## Response

Returns products from the same category, excluding the current product, with the following prioritization:
1. **Featured products** first
2. **In-stock products** prioritized
3. **Newest products** (by creation date)

## Example Requests

### Get Related Products (Default Limit)
```http
GET /api/products/15/related
```

### Get More Related Products
```http
GET /api/products/15/related?limit=12
```

### Get Fewer Related Products
```http
GET /api/products/15/related?limit=4
```

## Response Format

### Success Response (200 OK)
```json
{
  "success": true,
  "data": [
    {
      "id": 23,
      "name": "Hydrating Face Cream",
      "brand": "Brand Name",
      "price": 45.99,
      "originalPrice": 59.99,
      "volume": "50ml",
      "type": "Cream",
      "description": "Deep hydration for all skin types",
      "images": [
        "/images/abc123_cream1.jpg",
        "/images/def456_cream2.jpg"
      ],
      "categoryId": 2,
      "categoryName": "Skincare",
      "stock": 25,
      "isActive": true,
      "isFeatured": true,
      "createdOn": "2024-11-15T10:30:00Z",
      "updatedOn": "2024-12-01T14:20:00Z"
    },
    {
      "id": 45,
      "name": "Night Repair Serum",
      "brand": "Brand Name",
      "price": 89.99,
      "originalPrice": null,
      "volume": "30ml",
      "type": "Serum",
      "description": "Overnight skin repair and rejuvenation",
      "images": [
        "/images/ghi789_serum.jpg"
      ],
      "categoryId": 2,
      "categoryName": "Skincare",
      "stock": 12,
      "isActive": true,
      "isFeatured": false,
      "createdOn": "2024-11-20T08:15:00Z",
      "updatedOn": "2024-11-28T16:45:00Z"
    }
  ]
}
```

### Error Response (400 Bad Request)
```json
{
  "success": false,
  "message": "M?hsul tap?lmad?"
}
```

## Features

? **Smart Prioritization**
- Featured products appear first
- In-stock items prioritized over out-of-stock
- Newest products shown when other criteria are equal

? **Flexible Limit**
- Control how many related products to show
- Default is 8 products (good for most UI layouts)
- Can be adjusted per request

? **Same Category Only**
- Only returns products from the exact same category
- Ensures relevance to the original product

? **Excludes Current Product**
- Never returns the product itself in the related list
- Avoids redundancy

? **Active Products Only**
- Only returns active, non-deleted products
- Ensures customers only see available items

## Usage Examples

### JavaScript/Fetch
```javascript
const getRelatedProducts = async (productId, limit = 8) => {
  const response = await fetch(
    `http://localhost:5000/api/products/${productId}/related?limit=${limit}`
  );
  
  const result = await response.json();
  
  if (result.success) {
    return result.data;
  } else {
    console.error('Error:', result.message);
    return [];
  }
};

// Usage
const relatedProducts = await getRelatedProducts(15, 4);
console.log('Related Products:', relatedProducts);
```

### Axios
```javascript
import axios from 'axios';

const productService = {
  async getRelatedProducts(productId, limit = 8) {
    try {
      const response = await axios.get(
        `/api/products/${productId}/related`,
        { params: { limit } }
      );
      
      return response.data.data;
    } catch (error) {
      console.error('Error fetching related products:', error);
      return [];
    }
  }
};

// Usage
const related = await productService.getRelatedProducts(15, 6);
```

### React Component
```jsx
import { useState, useEffect } from 'react';

function RelatedProducts({ productId }) {
  const [relatedProducts, setRelatedProducts] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchRelatedProducts = async () => {
      setLoading(true);
      
      try {
        const response = await fetch(
          `/api/products/${productId}/related?limit=8`
        );
        const result = await response.json();
        
        if (result.success) {
          setRelatedProducts(result.data);
        }
      } catch (error) {
        console.error('Error:', error);
      } finally {
        setLoading(false);
      }
    };

    if (productId) {
      fetchRelatedProducts();
    }
  }, [productId]);

  if (loading) {
    return <div>Yükl?nir...</div>;
  }

  if (relatedProducts.length === 0) {
    return null;
  }

  return (
    <div className="related-products">
      <h2>Ox?ar M?hsullar</h2>
      <div className="product-grid">
        {relatedProducts.map(product => (
          <div key={product.id} className="product-card">
            <img src={product.images[0]} alt={product.name} />
            <h3>{product.name}</h3>
            <p className="brand">{product.brand}</p>
            <div className="price">
              {product.originalPrice && (
                <span className="original-price">
                  {product.originalPrice} AZN
                </span>
              )}
              <span className="current-price">{product.price} AZN</span>
            </div>
            {product.isFeatured && <span className="badge">Featured</span>}
            {product.stock === 0 && <span className="out-of-stock">Stokda yoxdur</span>}
          </div>
        ))}
      </div>
    </div>
  );
}

export default RelatedProducts;
```

### Vue.js Component
```vue
<template>
  <div v-if="relatedProducts.length > 0" class="related-products">
    <h2>Ox?ar M?hsullar</h2>
    <div class="product-grid">
      <div 
        v-for="product in relatedProducts" 
        :key="product.id" 
        class="product-card"
      >
        <img :src="product.images[0]" :alt="product.name" />
        <h3>{{ product.name }}</h3>
        <p class="brand">{{ product.brand }}</p>
        <div class="price">
          <span v-if="product.originalPrice" class="original-price">
            {{ product.originalPrice }} AZN
          </span>
          <span class="current-price">{{ product.price }} AZN</span>
        </div>
        <span v-if="product.isFeatured" class="badge">Featured</span>
        <span v-if="product.stock === 0" class="out-of-stock">
          Stokda yoxdur
        </span>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  props: {
    productId: {
      type: Number,
      required: true
    },
    limit: {
      type: Number,
      default: 8
    }
  },
  data() {
    return {
      relatedProducts: []
    };
  },
  async mounted() {
    await this.fetchRelatedProducts();
  },
  methods: {
    async fetchRelatedProducts() {
      try {
        const response = await fetch(
          `/api/products/${this.productId}/related?limit=${this.limit}`
        );
        const result = await response.json();
        
        if (result.success) {
          this.relatedProducts = result.data;
        }
      } catch (error) {
        console.error('Error:', error);
      }
    }
  },
  watch: {
    productId() {
      this.fetchRelatedProducts();
    }
  }
};
</script>
```

## Use Cases

### 1. Product Detail Page
Show related products at the bottom of a product detail page:
```javascript
// When viewing product ID 15
GET /api/products/15/related?limit=8
```

### 2. Shopping Cart Recommendations
Suggest similar items when a product is added to cart:
```javascript
const showCartRecommendations = async (addedProductId) => {
  const related = await getRelatedProducts(addedProductId, 4);
  // Show in a modal or sidebar
};
```

### 3. "Complete the Look" Section
Show complementary products from the same category:
```javascript
// For a skincare product
GET /api/products/23/related?limit=6
```

### 4. Mobile App Scrolling
Load fewer items for mobile view:
```javascript
const isMobile = window.innerWidth < 768;
const limit = isMobile ? 4 : 8;
GET `/api/products/${productId}/related?limit=${limit}`
```

## Response Characteristics

| Aspect | Value |
|--------|-------|
| Default Limit | 8 products |
| Maximum Limit | No hard limit (controlled by query param) |
| Sorting Priority | 1. Featured, 2. Stock, 3. Newest |
| Excludes | Current product, inactive products, deleted products |
| Category Match | Exact category ID match only |
| Includes | Product images, category name, all product details |

## Performance Notes

- ? Efficient database query with filtering
- ? Single database call per request
- ? Results are already sorted and limited
- ? No additional processing needed on frontend
- ?? Consider caching for frequently accessed products

## Integration with Product Detail Page

Complete example showing how to use this with a product detail page:

```javascript
const ProductDetailPage = async (productId) => {
  // Fetch the main product
  const productResponse = await fetch(`/api/products/${productId}`);
  const productResult = await productResponse.json();
  const product = productResult.data;

  // Fetch related products
  const relatedResponse = await fetch(
    `/api/products/${productId}/related?limit=8`
  );
  const relatedResult = await relatedResponse.json();
  const relatedProducts = relatedResult.data;

  return {
    product,
    relatedProducts,
    category: product.categoryName
  };
};
```

## Error Handling

The endpoint may return errors in these cases:

1. **Product Not Found (400)**
   - When the provided product ID doesn't exist
   - Message: "M?hsul tap?lmad?"

2. **No Related Products**
   - Returns empty array in data
   - Success is still true
   - Frontend should handle empty state

```javascript
const related = await getRelatedProducts(productId);

if (related.length === 0) {
  // Show message like "No similar products available"
  console.log('Bu kateqoriyada ba?qa m?hsul yoxdur');
}
```

## SEO Benefits

Using this endpoint can improve SEO by:
- ? Increasing page views (users browse similar products)
- ? Reducing bounce rate (users stay longer)
- ? Improving user engagement
- ? Creating internal linking structure
- ? Better category organization

## Testing

Test the endpoint with these scenarios:

```bash
# Test with existing product
curl http://localhost:5000/api/products/1/related

# Test with custom limit
curl http://localhost:5000/api/products/1/related?limit=4

# Test with non-existent product
curl http://localhost:5000/api/products/99999/related

# Test with product that has no related items
# (product is the only one in its category)
curl http://localhost:5000/api/products/100/related
```

## Best Practices

1. **Limit Selection**
   - Use 4-6 for mobile devices
   - Use 8-12 for desktop
   - Adjust based on your layout

2. **Loading States**
   - Show skeleton loaders while fetching
   - Handle empty states gracefully

3. **Error Handling**
   - Don't break the page if related products fail to load
   - Show alternatives or hide section if empty

4. **Performance**
   - Load related products after main product
   - Consider lazy loading if below the fold

5. **User Experience**
   - Show clear "You may also like" heading
   - Display product images prominently
   - Include quick add-to-cart buttons
