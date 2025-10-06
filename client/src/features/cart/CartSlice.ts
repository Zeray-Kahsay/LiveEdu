import { createSlice, type PayloadAction } from "@reduxjs/toolkit";
import type { CartItem } from "../../app/types/cart/CartItem";

interface CartState {
    items: CartItem[]; // i think the cartState should be a type of Cart.ts since cart id needs to be generated
}

const initialState: CartState = {
   items: [],
}

// Save and load from localStorage
const saveCartToStorage = (cart: CartState) => {
    localStorage.setItem("cart", JSON.stringify(cart));
}
const loadCartFromStorage = () : CartState => {
    try {
        const saved = localStorage.getItem("cart");
        return saved ? JSON.parse(saved) : initialState;
    } catch {
        return initialState;
    }
}

const cartSlice = createSlice({
    name: "cart",
    initialState: loadCartFromStorage(),
    reducers: {
       addItemToCart: (state, action: PayloadAction<CartItem>) => {
        const existingItem = state.items.find(item => item.courseId == action.payload.courseId);

        if (!existingItem){
            state.items.push({ ...action.payload, quantity: 1 });
        } else {
            existingItem.quantity += 1;
        }
        saveCartToStorage(state);
       },
       removeItemFromCart: (state, action) => {
        state.items = state.items.filter(
            (item) => item.courseId !== action.payload
        );
        saveCartToStorage(state);
       },
       clearCart: (state) => {
        state.items = [];
        saveCartToStorage(state);
       },
       updateQuantity: (state, action: PayloadAction<{courseId: number; quantity: number}>) => {
        const item = state.items.find(i => i.courseId === action.payload.courseId);
        if (item) item.quantity = Math.max(action.payload.quantity);
        saveCartToStorage(state);
       }
    },
});

export const {addItemToCart, removeItemFromCart, clearCart, updateQuantity} = cartSlice.actions;
export default cartSlice.reducer;