import { apiSlice } from "../../app/api/apiSlice";
import type { CreatePaymentDto } from "../../app/types/cart/CreatePaymentDto";
import type { PaymentIntentResponseDto } from "../../app/types/cart/PaymentIntentResponseDto";

export const paymentApi = apiSlice.injectEndpoints({
  endpoints: (builder) => ({
    createPaymentIntent: builder.mutation<PaymentIntentResponseDto, CreatePaymentDto>({
      query: (data) => ({
        url: "/payment/create-payment-intent",
        method: "POST",
        body: data,
      }),
    }),
  }),
});

export const { useCreatePaymentIntentMutation } = paymentApi;
