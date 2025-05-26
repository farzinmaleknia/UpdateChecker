import { configureStore } from "@reduxjs/toolkit";
import { setupListeners } from '@reduxjs/toolkit/query';
import resourcesReducer from "./slices/resources";
import { setResources } from "./slices/resources";
import { resourcesApi } from "./apis/resourcesApi";

export const store = configureStore({
    reducer: {
        [resourcesApi.reducerPath]: resourcesApi.reducer,
        resources: resourcesReducer,
    },
    middleware: (getDefaultMitddleware) => {
        return getDefaultMitddleware()
            .concat(resourcesApi.middleware);
    }
});

setupListeners(store.dispatch);

export {useFetchResourcesQuery} from './apis/resourcesApi';
export { setResources } from "./slices/resources"; 

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
