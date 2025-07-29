import { Error } from "./error";

export type HttpValidationError = {
    errors: Error[];
    status: number;
    title: string;
    type: string;
}