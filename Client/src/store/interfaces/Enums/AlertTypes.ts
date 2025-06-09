export const AlertTypes = {
  Success: "Success",
  Info: "Info",
  Warning: "Warning",
  Error: "Error",
} as const;

export type AlertTypes = keyof typeof AlertTypes;
