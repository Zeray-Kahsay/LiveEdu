import { createSlice, type PayloadAction } from "@reduxjs/toolkit";
import type { Cart } from "../../app/types/cart/Cart";
import type { CartItem } from "../../app/types/cart/CartItem";

// ✅ Load from localStorage
const storedCart = localStorage.getItem("cart");
const initialState: {
   cart: Cart | null
   } = { cart: storedCart ? JSON.parse(storedCart) : null,};

const cartSlice = createSlice({
  name: "cart",
  initialState,
  reducers: {
    // ✅ Replace entire cart (e.g., after addItem or fetch)
    setCart: (state, action: PayloadAction<Cart>) => {
      const cart  = action.payload;
      const total = cart.items.reduce((sum, curr) => sum + curr.price * curr.quantity, 0);
      state.cart = {...cart, total};
      localStorage.setItem("cart", JSON.stringify({...cart, total}));
      
    },

    // ✅ Clear cart (e.g., after successful checkout)
    clearCart: (state) => {
      state.cart = null;
      localStorage.removeItem("cart");
    },

    // ✅ Optional: remove one item (client-side only)
    removeItem: (state, action: PayloadAction<number>) => {
      if (!state.cart) return;
      state.cart.items = state.cart.items.filter(
        (item: CartItem) => item.courseId !== action.payload
      );
      state.cart.total = state.cart.items.reduce(
        (sum, item) => sum + item.price * item.quantity,
        0
      );
      localStorage.setItem("cart", JSON.stringify(state.cart));
    },
  },
});

export const { setCart, clearCart, removeItem } = cartSlice.actions;
export default cartSlice.reducer;


// import { createSlice, type PayloadAction } from "@reduxjs/toolkit";
// import type { CartItem } from "../../app/types/cart/CartItem";
// import type { Cart } from "../../app/types/cart/Cart";

// const initialState: Cart = {
//   id: crypto.randomUUID(),
//   items: [],
//   total: 0,
// };

// // LocalStorage helpers
// const saveCartToStorage = (cart: Cart) => {
//   localStorage.setItem("cart", JSON.stringify(cart));
// };

// const loadCartFromStorage = (): Cart => {
//   try {
//     const saved = localStorage.getItem("cart");
//     return saved ? JSON.parse(saved) : initialState;
//   } catch {
//     return initialState;
//   }
// };

// const cartSlice = createSlice({
//   name: "cart",
//   initialState: loadCartFromStorage(),
//   reducers: {
//     addItemToCart: (state, action: PayloadAction<CartItem>) => {
//       const existingItem = state.items.find((i) => i.courseId === action.payload.courseId);
//       if (existingItem) {
//         existingItem.quantity += 1;
//       } else {
//         state.items.push({ ...action.payload, quantity: 1 });
//       }
//       state.total = state.items.reduce((sum, i) => sum + i.price * i.quantity, 0);
//       saveCartToStorage(state);
//     },

//     removeItemFromCart: (state, action: PayloadAction<number>) => {
//       state.items = state.items.filter((i) => i.courseId !== action.payload);
//       state.total = state.items.reduce((sum, i) => sum + i.price * i.quantity, 0);
//       saveCartToStorage(state);
//     },

//     clearCart: (state) => {
//       state.items = [];
//       state.total = 0;
//       state.paymentIntentId = undefined;
//       state.clientSecret = undefined;
//       saveCartToStorage(state);
//     },

//     updateQuantity: (state, action: PayloadAction<{ courseId: number; quantity: number }>) => {
//       const item = state.items.find((i) => i.courseId === action.payload.courseId);
//       if (item) item.quantity = Math.max(action.payload.quantity, 1);
//       state.total = state.items.reduce((sum, i) => sum + i.price * i.quantity, 0);
//       saveCartToStorage(state);
//     },

//     //Save Payment Intent info after Stripe call
//     setPaymentIntent: (
//       state,
//       action: PayloadAction<{ paymentIntentId: string; clientSecret: string }>
//     ) => {
//       state.paymentIntentId = action.payload.paymentIntentId;
//       state.clientSecret = action.payload.clientSecret;
//       saveCartToStorage(state);
//     },

//     // Replace entire cart (used after backend sync)
//     setCart: (state, action: PayloadAction<Cart>) => {
//       return { ...action.payload };
//     },
//   },
// });

// export const {
//   addItemToCart,
//   removeItemFromCart,
//   clearCart,
//   updateQuantity,
//   setPaymentIntent,
//   setCart,
// } = cartSlice.actions;

// export default cartSlice.reducer;
