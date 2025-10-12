import type { PaymentItemDto } from "./PaymentItemDto";

export interface CreatePaymentDto {
    userId?: number;
    orderId?: number;   
    items: PaymentItemDto[];
    currency: string;
    
}