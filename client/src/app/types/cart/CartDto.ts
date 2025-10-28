import type { CartItem } from "./CartItem";

export interface CartDto {
    id: number;
    cartId: string;
    userId?: number;
    items: CartItem[];
    paymentIntentId?: string;
    clientSecret?: string;
}