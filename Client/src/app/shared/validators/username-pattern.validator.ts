import { AbstractControl, ValidationErrors } from "@angular/forms";

export const usernamePatternValidator = (control: AbstractControl): ValidationErrors | null => {
    const username: string = control.value ?? '';

    if (/[^a-zA-Z0-9]+/.test(username)) {
        return { pattern: 'Username can only contain letters and digits.' };
    }

    return null;
}