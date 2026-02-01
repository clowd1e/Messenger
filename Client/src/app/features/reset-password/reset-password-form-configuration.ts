import { createFormControlConfiguration, FormControlConfiguration } from "../../shared/models/configurations/forms/form-control-configuration";

export const resetPasswordFormConfiguration: Record<string, FormControlConfiguration> = {
    password: createFormControlConfiguration({
        required: 'Password is required',
        minlength: 'Password must be at least 8 characters long',
        maxlength: 'Password must be at most 30 characters long',
        uppercasePattern: 'Password must contain at least one uppercase letter',
        lowercasePattern: 'Password must contain at least one lowercase letter',
        digitPattern: 'Password must contain at least one digit',
        specialCharacterPattern: 'Password must contain at least one special character'
    }),
    repeatPassword: createFormControlConfiguration({
        required: 'Repeat Password is required',
        repeatPassword: 'Passwords don\'t match.'
    })
};