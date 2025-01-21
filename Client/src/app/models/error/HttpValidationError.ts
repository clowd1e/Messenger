import { Error } from "./Error";

export type HttpValidationError = {
    errors: Error[];
    status: number;
    title: string;
    type: string;
}