import type { CartItem } from "./CartItem";

export interface Cart {
  id: string;
  cartId: string;
  items: CartItem[];
  total: number;
  userId?: number;
  paymentIntentId?: string;
  clientSecret?: string;
  
}