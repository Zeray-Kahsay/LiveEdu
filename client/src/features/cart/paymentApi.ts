import { apiSlice } from "../../app/api/apiSlice";

export const paymentApi = apiSlice.injectEndpoints({
  endpoints: (builder) => ({
    createPaymentIntent: builder.mutation({
      query: (cart) => ({
        url: "/payment/create-intent",
        method: "POST",
        body: cart,
      }),
    }),
  }),
});

export const { useCreatePaymentIntentMutation } = paymentApi;
