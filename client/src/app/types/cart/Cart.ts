import type { CartItem } from "./CartItem";

export interface Cart {
  id: string;
  items: CartItem[];
  total: number;
  
}