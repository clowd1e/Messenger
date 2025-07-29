import { createFormControlConfiguration, FormControlConfiguration } from "../../shared/models/configurations/forms/form-control-configuration";

export const registerFormConfiguration: Record<string, FormControlConfiguration> = {
    username: createFormControlConfiguration({
        required: 'Username is required',
        minlength: 'Username must be at least 3 characters long',
        maxlength: 'Username must be at most 30 characters long',
        pattern: 'Username can only contain letters and digits'
    }),
    name: createFormControlConfiguration({
        required: 'Name is required',
        minlength: 'Name must be at least 1 character long',
        maxlength: 'Name must be at most 30 characters long'
    }),
    email: createFormControlConfiguration({
        required: 'Email is required',
        minlength: 'Email must be at least 3 characters long',
        email: 'Email must be a valid email address',
        maxlength: 'Email must be at most 50 characters long'
    }),
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