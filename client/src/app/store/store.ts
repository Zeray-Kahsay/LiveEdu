import { configureStore } from "@reduxjs/toolkit";
import { useDispatch, useSelector, type TypedUseSelectorHook } from "react-redux";
import uiReducer from "../layout/uiSlice";
import authReducer from "../../features/auth/authSlice";
import cartReducer from "../../features/cart/CartSlice";
import { apiSlice } from "../api/apiSlice";


export const store = configureStore({
    reducer: {
        ui: uiReducer,
        auth: authReducer,
        cart: cartReducer,
        [apiSlice.reducerPath]: apiSlice.reducer,
    },
   middleware: (getDefaultMiddleware) =>
    getDefaultMiddleware().concat(apiSlice.middleware),
    
})

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;

export const useAppDispatch = () => useDispatch<AppDispatch>();
export const useAppSelector: TypedUseSelectorHook<RootState> = useSelector;