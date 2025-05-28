import type { BaseQueryFn } from '@reduxjs/toolkit/query';
import type { ResultClass } from '../interfaces/ResultClass/ResultClass';
import type { Resources } from '../interfaces/Resources/Resources';
import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import { dynamicBaseQuery } from './dynamicBaseQuery';

const resourcesApi = createApi({
    reducerPath: 'fetchRecources',
    baseQuery: fetchBaseQuery({
        baseUrl: "http://localhost:5025",
    }) as BaseQueryFn,
    endpoints(builder){
      return {
        fetchResources: builder.query<ResultClass<Resources>, void>({
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
