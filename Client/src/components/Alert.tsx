import { Fragment } from "react/jsx-runtime"
import { AlertTypes } from "../store/interfaces/Enums/AlertTypes";
import { useAppSelector } from "../hooks";


interface ButtonProps {
  children: React.ReactNode;
  type?: AlertTypes;
}

export const Alert = ({ children, type = AlertTypes.Error}: ButtonProps) => {
  const resources = useAppSelector((state) => state.resources);

  let title = resources?.Titles.Error;  
  let bg800 = `bg-red-800`;
  let bg500 = `bg-red-500`;
  let text100 = `text-red-100`;

  switch (type) {
    case AlertTypes.Success:
      bg800 = `bg-green-800`;
      bg500 = `bg-green-500`;
      text100 = `text-green-100`;
      title = resources?.Titles.Success;
      break;
    case AlertTypes.Info:
      bg800 = `bg-indigo-800`;
      bg500 = `bg-indigo-500`;
      text100 = `text-indigo-100`;
      title = resources?.Titles.Info;
      break;
    case AlertTypes.Warning:
      bg800 = `bg-orange-800`;
      bg500 = `bg-orange-500`;
      text100 = `text-orange-100`;
      title = resources?.Titles.Warning;
      break;
  }




  return (
    <Fragment>
      <div className="text-center py-4 lg:px-4">
        <div className={`p-2 ${bg800} items-center ${text100} leading-none lg:rounded-full flex lg:inline-flex`} role="alert">
          <span className={`flex rounded-full ${bg500} uppercase px-2 py-1 text-xs font-bold mr-3`}>{title}</span>
          <span className="font-semibold mr-2 text-left flex-auto">{children}</span>
          {/* <svg className="fill-current opacity-75 h-4 w-4" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20"><path d="M12.95 10.707l.707-.707L8 4.343 6.586 5.757 10.828 10l-4.242 4.243L8 15.657l4.95-4.95z"/></svg> */}
        </div>
      </div>
    </Fragment>
  );
};