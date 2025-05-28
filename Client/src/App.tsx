import { useAppDispatch, useAppSelector } from './hooks';
import { useEffect } from "react";
import { setResources } from "./store";
import { useResources } from "./hooks/useResources";
import { localResources } from "./data/Defaults.json";
import Main from "./pages/Main";

function App() {
  const isResourcesLoaded = useResources();
  const dispatch = useAppDispatch();
  const resources  = useAppSelector((state) => state);

  useEffect(() => {
    const ResourcesAction = setResources(localResources);
    dispatch(ResourcesAction);
  }, []);
  
  return (

    <div className="">
      {isResourcesLoaded? <Main/>: null}
    </div>
  );
}

export default App;
