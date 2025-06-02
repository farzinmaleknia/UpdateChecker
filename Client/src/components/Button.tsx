import { Fragment } from "react/jsx-runtime"


interface ButtonProps {
  children: React.ReactNode;
  color?: string;
  type?: React.ButtonHTMLAttributes<HTMLButtonElement>["type"];
}

export const Button = ({ children, color, type = "button" }: ButtonProps) => {
  const colorClassMap: Record<string, string> = {
    blue: "bg-blue-300 border-blue-300",
    red: "bg-red-300 border-red-300",
    green: "bg-green-300 border-green-300",
    // add more if needed
  };

  const colorClass = color
    ? colorClassMap[color] ?? colorClassMap["blue"]
    : colorClassMap["blue"];

  return (
    <Fragment>
      <button
        type={type}
        className={`min-w-25 bg-blue-300 p-2 rounded-md border ${colorClass} text-white shadow-md`}
      >
        {children}
      </button>
    </Fragment>
  );
};