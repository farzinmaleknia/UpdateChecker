import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import type { BaseQueryFn } from '@reduxjs/toolkit/query';

type Resources = { id: string; name: {id: string, name: string} };


const resourcesApi = createApi({
    reducerPath: 'recources',
    baseQuery: fetchBaseQuery({
        baseUrl: 'http://localhost:5025',
    }) as BaseQueryFn,
    endpoints(builder){
      return {
        fetchResources: builder.query<Resources, void>({
            query: () => {
                return {
                    url: '/Resources',
                    method: 'GET',
                };
            }
        })
      }; 
    }
});

export const { useFetchResourcesQuery } = resourcesApi;
export { resourcesApi };
