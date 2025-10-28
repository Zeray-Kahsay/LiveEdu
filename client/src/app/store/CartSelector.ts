import { createSelector } from "@reduxjs/toolkit";
import type { RootState } from "./store";

export const selectCart = (state: RootState) => state.cart.cart;

export const selectCartTotal = createSelector(
[selectCart],
    cart => cart?.items?.reduce((sum: number, curr: any) => sum +curr.price * curr.quantity, 0) ?? 0
);