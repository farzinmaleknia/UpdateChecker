import { configureStore } from "@reduxjs/toolkit";
import { setupListeners } from '@reduxjs/toolkit/query';
import { resourcesApi } from "./apis/resourcesApi";

export const store = configureStore({
    reducer: {
        [resourcesApi.reducerPath]: resourcesApi.reducer,
    },
    middleware: (getDefaultMitddleware) => {
        return getDefaultMitddleware()
            .concat(resourcesApi.middleware);
    }
});

setupListeners(store.dispatch);

export {useFetchResourcesQuery} from './apis/resourcesApi';
