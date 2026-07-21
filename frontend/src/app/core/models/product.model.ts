export interface Product {
  id: number;
  name: string;
  description?: string | null;
  price: number;
  stock: number;
  createdAt?: string;
}

// Datos que el formulario envía al crear o editar un producto
// (no incluyen Id ni CreatedAt, que los administra el backend).
export interface ProductInput {
  name: string;
  description?: string | null;
  price: number;
  stock: number;
}
