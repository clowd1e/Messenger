import { createFormControlConfiguration, FormControlConfiguration } from "../../shared/models/configurations/forms/form-control-configuration";

export const forgotPasswordFormConfiguration: Record<string, FormControlConfiguration> = {
    email: createFormControlConfiguration({
        required: 'Email is required',
        minlength: 'Email must be at least 3 characters long',
        email: 'Email must be a valid email address',
        maxlength: 'Email must be at most 50 characters long'
    })
};