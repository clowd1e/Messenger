import { Error } from "./error";

export type HttpError = {
    errors: Error;
    status: number;
    title: string;
    type: string;
}