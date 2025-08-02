import { createFormControlConfiguration, FormControlConfiguration } from "../../shared/models/configurations/forms/form-control-configuration";

export const loginFormConfiguration: Record<string, FormControlConfiguration> = {
    email: createFormControlConfiguration({
        required: 'Email is required',
        minlength: 'Email must be at least 3 characters long',
        email: 'Email must be a valid email address',
        maxlength: 'Email must be at most 50 characters long'
    }),
    password: createFormControlConfiguration({
        required: 'Password is required',
        minlength: 'Password must be at least 8 characters long',
        maxlength: 'Password must be at most 30 characters long'
    })
};