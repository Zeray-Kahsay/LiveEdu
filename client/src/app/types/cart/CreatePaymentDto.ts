import type { PaymentItemDto } from "./PaymentItemDto";

export interface CreatePaymentDto {
    userId?: number;
    items: PaymentItemDto[];
    currency: string;
    
}