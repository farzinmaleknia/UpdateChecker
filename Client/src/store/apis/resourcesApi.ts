import type { BaseQueryFn, FetchArgs, FetchBaseQueryError  } from '@reduxjs/toolkit/query';
import type { ResultClass } from '../interfaces/ResultClass/ResultClass';
import type { Resources } from '../interfaces/Resources/Resources';
import { createApi } from '@reduxjs/toolkit/query/react';
import { dynamicBaseQuery } from './dynamicBaseQuery';

const resourcesApi = createApi({
    reducerPath: 'fetchRecources',
    baseQuery: dynamicBaseQuery as BaseQueryFn<string | FetchArgs, unknown, FetchBaseQueryError>,
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
