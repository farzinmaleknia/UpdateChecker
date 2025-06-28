import type { VerificationForUpdateDTO } from "../store/interfaces/Update/VerificationForUpdateDTO";
import { useState } from "react";
import { useAppSelector } from "../hooks";
import { TextInput } from "./TextInput";
import { Button } from "./Button";

interface UpdateVerificationProps {
  onHandleSubmit : (model: VerificationForUpdateDTO) => void,
}

const UpdateVerification = ({onHandleSubmit}: UpdateVerificationProps ) => {
  const [VerificationModel, setVerificationModel] = useState<VerificationForUpdateDTO>({
    VerificationCode: "",
  });
  const resources = useAppSelector((state) => state.resources);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setVerificationModel((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    onHandleSubmit(VerificationModel);
  };

  return (
    <div>
      <form onSubmit={handleSubmit} className="w-120 flex flex-col">
        <div className="flex flex-col self-center">
          <label className="font-bold mx-2" htmlFor="username">
            {resources?.Titles.Username} :
          </label>
          <TextInput
            name="verificationCode"
            id="verificationCode"
            value={VerificationModel.VerificationCode}
            onChange={handleChange}
          ></TextInput>
        </div>
        <div className="flex justify-end">
          <Button type="submit">{resources?.Titles.Submit}</Button>
        </div>
      </form>
    </div>
  );
};

export default UpdateVerification;
