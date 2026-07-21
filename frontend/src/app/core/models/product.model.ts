export interface Product {
  id: number;
  name: string;
  description?: string | null;
  price: number;
  stock: number;
  createdAt?: string;
}

export interface ProductInput {
  name: string;
  description?: string | null;
  price: number;
  stock: number;
}
