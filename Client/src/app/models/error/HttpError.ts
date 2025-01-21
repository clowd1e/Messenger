import { Error } from "./Error";

export type HttpError = {
    errors: Error;
    status: number;
    title: string;
    type: string;
}