import { useEffect, type JSX} from "react";
import { useUpdateLoginMutation } from "../store";
import type { LoginForUpdateDTO } from "../store/interfaces/Update/LoginForUpdateDTO";
import type { VerificationForUpdateDTO } from "../store/interfaces/Update/VerificationForUpdateDTO";
import UpdateLogin from "../components/UpdateLogin";
import UpdateVerification from "../components/UpdateVerification";
import { UpdateSteps } from "../store/interfaces/Enums/UpdateSteps";
import { Alert } from "../components/Alert";
import { useAppSelector } from "../hooks";

/////////////////////////////////////////////////////////////////////////////
// Work from here : adding useUpdateVerificationMutation
///////////////////////////////////////////////////

const Update = () => {
  const [updateLogin, results] = useUpdateLoginMutation();
    const resources = useAppSelector((state) => state.resources);

  const onLoginFormSubmited = (auth : LoginForUpdateDTO) => {
    updateLogin(auth);
  }

  useEffect(() => {
    console.log(results.data?.messageKey);
  }, [results])

  const renderContent = (): JSX.Element => {
    switch (results.data?.data.updateStep) {
      case UpdateSteps.WaitingForVerificationCode:
        return(<div>VerficationCode</div>);
      default:
        return(<UpdateLogin onHandleSubmit={onLoginFormSubmited} />);
    }
  }

  return (
    <div>
      {results.data?.messageKey.length != undefined && results.data?.messageKey.length > 0 
        && <Alert>{resources.Messages[results.data?.messageKey[0]] || results.data?.messageKey[0]}</Alert> }
      {renderContent()}
    </div>
  );
};

export default Update;
