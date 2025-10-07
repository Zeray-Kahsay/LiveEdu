import { createSlice, type PayloadAction } from "@reduxjs/toolkit";
import type { CartItem } from "../../app/types/cart/CartItem";
import type { Cart } from "../../app/types/cart/Cart";


const initialState: Cart = {
    id: crypto.randomUUID(),
    items: [],
    total: 0,
}

// TODO: move these logics into a dedicated file

// Save and load from localStorage
const saveCartToStorage = (cart: Cart) => {
    localStorage.setItem("cart", JSON.stringify(cart));
}
const loadCartFromStorage = () : Cart => {
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