import { Fragment } from "react/jsx-runtime";


interface InputProps {
  name: string;
  id: string;
  value: string;
  onChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
  placeholder?: string;
  type?: string;
}

export const TextInput = ({
  name,
  id,
  value,
  onChange,
  placeholder,
  type,
}: InputProps) => {
  return (
    <Fragment>
      <input
        name={name}
        id={id}
        type={type}
        className=" block border border-gray-300 rounded-md shadow-sm focus:border-blue-500 focus:outline-none m-2 px-4 py-2"
        value={value}
        onChange={(e) => onChange(e)}
        placeholder={placeholder}
      />
    </Fragment>
  );
};
