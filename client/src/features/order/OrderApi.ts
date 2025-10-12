import { apiSlice } from "../../app/api/apiSlice";
import type { CreateOrderDto } from "../../app/types/cart/CreateOrder";
import type { OrderDto } from "../../app/types/cart/Order";

export const orderApi = apiSlice.injectEndpoints({
  endpoints: (builder) => ({
    createOrder: builder.mutation<OrderDto, CreateOrderDto>({
      query: (data) => ({
        url: "/orders",
        method: "POST",
        body: data,
      }),
      invalidatesTags: ["Cart", "Orders"],
    }),
    getUserOrders: builder.query<OrderDto[], number>({
      query: (userId) => `/orders/user/${userId}`,
      providesTags: ["Orders"],
    }),
  }),
  overrideExisting: false,
});

export const { useCreateOrderMutation, useGetUserOrdersQuery } = orderApi;
