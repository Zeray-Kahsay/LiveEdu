import { createSlice } from "@reduxjs/toolkit";

interface uiState{
    isLoading: boolean;
}

const initialState : uiState  = {
    isLoading: false
}

export const uiSlice = createSlice({
    name: 'ui',
    initialState,       
    reducers: {
        startLoading : (state) => {
            state.isLoading = true;
        },
        stopLoading : (state) => {
            state.isLoading = false;
        }
    }
})

export const { startLoading, stopLoading } = uiSlice.actions;
export default uiSlice.reducer;

