export const UpdateSteps = {
  WaitingForCredentials: "WaitingForCredentials",
  WaitingForVerificationCode: "WaitingForVerificationCode",
  Completed: "Completed",
  Error: "Error",
} as const;

export type UpdateSteps = keyof typeof UpdateSteps;
