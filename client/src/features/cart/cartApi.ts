import { apiSlice } from "../../app/api/apiSlice";
import type { Cart } from "../../app/types/cart/Cart";
import { setCart } from "./CartSlice";

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
  async onQueryStarted({}, { dispatch, queryFulfilled }) {
    try {
      const { data } = await queryFulfilled;
      //Update Redux with updated cart from backend
      dispatch(setCart(data));
    } catch (err) {
      console.error("Failed to remove item from cart:", err);
    }
  },
}),

    // Clear cart
    clearCart: builder.mutation<Cart, string>({
      query: (cartId) => ({
        url: `/carts/${cartId}/clear`,
        method: "DELETE",
      }),
       async onQueryStarted({}, { dispatch, queryFulfilled }) {
    try {
      const { data } = await queryFulfilled;
      // Update Redux with updated cart from backend
      dispatch(setCart(data));
    } catch (err) {
      console.error("Failed to remove item from cart:", err);
    }
  },
      //invalidatesTags: (_result, _error, cartId) => [{ type: "Cart", id: cartId }],
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


