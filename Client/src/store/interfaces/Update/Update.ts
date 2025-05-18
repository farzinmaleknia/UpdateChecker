import type { UpdateSteps } from "../Enums/UpdateSteps";

export interface Update {
    updateStep : UpdateSteps;
    sessionId : string;
}