import { apiSlice } from "../../app/api/apiSlice";
import type { Cart } from "../../app/types/cart/Cart";

interface PaymentIntentResponse {
  clientSecret: string;
  publishableKey: string;
}

export const cartApi = apiSlice.injectEndpoints({
  endpoints: (builder) => ({
    // Get an existing cart by ID
    getCart: builder.query<Cart, string>({
      query: (cartId) => `/carts/${cartId}`,
      providesTags: (_result, _error, cartId) => [{ type: "Cart", id: cartId }],
    }),

    // Add item to cart
    addItemToCart: builder.mutation<Cart, { courseId: number, cartId?: string }>({
      query: ({ courseId, cartId }) => ({
        url: `/carts/add/${courseId}${cartId ? `?cartId=${cartId}` : ''}`,
        method: "POST",
      }),
      invalidatesTags: (_result, _error, { cartId }) => [{ type: "Cart", id: cartId }],
    }),

    //Remove item from cart
    removeItemFromCart: builder.mutation<Cart, { cartId: string; courseId: number }>({
      query: ({ cartId, courseId }) => ({
        url: `/carts/${cartId}/remove/${courseId}`,
        method: "DELETE",
      }),
      invalidatesTags: (_, _error, { cartId }) => [{ type: "Cart", id: cartId }],
    }),

    // Clear cart
    clearCart: builder.mutation<void, string>({
      query: (cartId) => ({
        url: `/carts/${cartId}/clear`,
        method: "DELETE",
      }),
      invalidatesTags: (_result, _error, cartId) => [{ type: "Cart", id: cartId }],
    }),

    // Create or update payment intent (Stripe)
    createPaymentIntent: builder.mutation<PaymentIntentResponse, string>({
      query: (cartId) => ({
        url: `/payments/create-payment-intent`,
        method: "POST",
        body: { cartId },
      }),
      invalidatesTags: (_result, _error, cartId) => [{ type: "Cart", id: cartId }],
    }),
  }),
});

export const {
  useGetCartQuery,
  useAddItemToCartMutation,
  useRemoveItemFromCartMutation,
  useClearCartMutation,
  useCreatePaymentIntentMutation,
} = cartApi;


