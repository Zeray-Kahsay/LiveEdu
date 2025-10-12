 export interface OrderDto {
    orderId: number;
    userId: number | null;
    total: number;
    createdAt: string; // ISO date string
    isPaid: boolean;
    items: OrderItemDto[];
}


interface OrderItemDto {
    courseId: number;
    courseTitle: string;
    price: number;
    quantity: number;

}