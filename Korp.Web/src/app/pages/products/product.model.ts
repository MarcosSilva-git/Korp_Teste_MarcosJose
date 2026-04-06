export type Product = {
    id: number,
    name: string,
    stock: number,
    reserved: number,
    available: number,
    createdAt: Date
}

export type CreateOrUpdateProduct = Pick<Product, 'id' | 'name' | 'stock'>;