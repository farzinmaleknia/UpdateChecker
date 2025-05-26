export interface ResultClass<T> {
    messageKey : string; 
    isSuccess : boolean; 
    data : T; 
    statusCode : number; 
}