import { createSlice } from "@reduxjs/toolkit";
import type { PayloadAction } from "@reduxjs/toolkit" 
import type { Resources } from "../interfaces/Resources/Resources";

const initialState : Resources = {};

export const resourcesSlice = createSlice({
    name: "resources",
    initialState,
    reducers: {
        setResources: (state, action: PayloadAction<Resources>) =>{
            return {...state , ...action.payload};
        }
    }
})

export const { setResources } = resourcesSlice.actions;
export default resourcesSlice.reducer;