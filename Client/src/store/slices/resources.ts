import { createSlice } from "@reduxjs/toolkit";
import type { PayloadAction } from "@reduxjs/toolkit" 
import type { Resources } from "../interfaces/Resources/Resources";

interface ResourcesState {
    resources : Resources | null;
}

const initialState : ResourcesState = {
    resources: null,
};

export const resourcesSlice = createSlice({
    name: "resources",
    initialState,
    reducers: {
        setResources: (state, action: PayloadAction<Resources>) =>{
            state.resources = action.payload;
        }
    }
})

export const { setResources } = resourcesSlice.actions;
export default resourcesSlice.reducer;