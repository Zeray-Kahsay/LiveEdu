
    export interface CreateOrderDto {
        userId: number | undefined;
        items: CartItemDto[];
        currency: string;
    }

    export interface CartItemDto {
        courseId: number;
        quantity: number;
        title: string;
        price: number;
        subject?: string;
        gradeLevel?: string;
        teacherName?: string;
        description?: string;
    }

 