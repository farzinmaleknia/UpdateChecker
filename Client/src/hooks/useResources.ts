import { useEffect } from "react";
import { useFetchResourcesQuery } from "../store";
import { setResources } from "../store/slices/resources";
import { useAppDispatch } from '../hooks';


export const useResources =  (): void => {
    const { data , error, isSuccess  } = useFetchResourcesQuery();
    const dispatch = useAppDispatch();

    useEffect(() => {

        if(isSuccess)
        {
            if(data.isSuccess){
                const ResourcesAction = setResources(data.data);
                dispatch(ResourcesAction);


            } else {
                console.error(data.messageKey);
            }

        } else if (error && typeof error === 'object' && 'status' in error) {

            const err = error as any;
            console.error(err?.data?.message || `Request failed with status ${err.status}`);

        } 

    }, [data, error, isSuccess])
}