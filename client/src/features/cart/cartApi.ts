import { apiSlice } from "../../app/api/apiSlice";
import type { Cart } from "../../app/types/cart/Cart";


export const cartApi = apiSlice.injectEndpoints({
  endpoints: (builder) => ({
    getCart: builder.query<Cart, string>({
      query: (cartId) => `/cart/${cartId}`,
    }),

    addItemToCart: builder.mutation<Cart, { cartId: string; courseId: number }>({
      query: ({ cartId, courseId }) => ({
        url: `/cart/${cartId}/add/${courseId}`,
        method: "POST",
      }),
    }),

    removeItemFromCart: builder.mutation<Cart, { cartId: string; courseId: number }>({
      query: ({ cartId, courseId }) => ({
        url: `/cart/${cartId}/remove/${courseId}`,
        method: "DELETE",
      }),
    }),

    clearCart: builder.mutation<void, string>({
      query: (cartId) => ({
        url: `/cart/${cartId}/clear`,
        method: "DELETE",
      }),
    }),
  }),
});

export const {
  useGetCartQuery,
  useAddItemToCartMutation,
  useRemoveItemFromCartMutation,
  useClearCartMutation,
} = cartApi;
