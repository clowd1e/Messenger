import { AbstractControl, ValidationErrors } from "@angular/forms";

export const passwordPatternValidator = (control: AbstractControl): ValidationErrors | null => {
    const password: string = control.value ?? '';

    if (!/[A-Z]/.test(password)) {
        return { uppercasePattern: 'Password must contain at least one uppercase letter.' };
    }

    if (!/[a-z]/.test(password)) {
        return { lowercasePattern: 'Password must contain at least one lowercase letter.' };
    }

    if (!/[0-9]/.test(password)) {
        return { digitPattern: 'Password must contain at least one digit.' };
    }

    if (!/[^a-zA-Z0-9]/.test(password)) {
        return { specialCharacterPattern: 'Password must contain at least one special character.' };
    }

    return null;
}