import { useEffect, useState, type JSX} from "react";
import { useUpdateLoginMutation, useUpdateVerificationMutation } from "../store";
import type { LoginForUpdateDTO } from "../store/interfaces/Update/LoginForUpdateDTO";
import type { VerificationForUpdateDTO } from "../store/interfaces/Update/VerificationForUpdateDTO";
import UpdateLogin from "../components/UpdateLogin";
import UpdateVerification from "../components/UpdateVerification";
import { UpdateSteps } from "../store/interfaces/Enums/UpdateSteps";
import { Alert } from "../components/Alert";
import { useAppSelector } from "../hooks";


const Update = () => {
  const [updateLogin, loginResults] = useUpdateLoginMutation();
  const [updateVerification, verificationResults] = useUpdateVerificationMutation();
  const [activeResultKey, setActiveResultKey] = useState<"login" | "verification" | null>(null);
  const resources = useAppSelector((state) => state.resources);

  const onLoginFormSubmitted = (auth : LoginForUpdateDTO) => {
    setActiveResultKey("login");
    updateLogin(auth);
  }

  
  const onVerificationSubmitted = (code: VerificationForUpdateDTO) => {
    setActiveResultKey("verification");
    updateVerification(code);
  };

  const activeResult = activeResultKey === "login" ? loginResults : activeResultKey === "verification" ? verificationResults : null;

  // useEffect(() => {
  //   console.log(results.data?.messageKey);
  // }, [results])

  const renderContent = (): JSX.Element => {
    switch (activeResult?.data?.data.updateStep) {
      case UpdateSteps.WaitingForVerificationCode:
        return(<UpdateVerification onHandleSubmit={onVerificationSubmitted} sessionId={activeResult.data?.data.sessionId} />);
      default:
        return(<UpdateLogin onHandleSubmit={onLoginFormSubmitted} />);
    }
  }

  return (
    <div>
      {activeResult?.data?.messageKey.length != undefined && activeResult?.data?.messageKey.length > 0 
        && <Alert>{resources.Messages[activeResult?.data?.messageKey[0]] || activeResult?.data?.messageKey[0]}</Alert> }
      {renderContent()}
    </div>
  );
};

export default Update;
