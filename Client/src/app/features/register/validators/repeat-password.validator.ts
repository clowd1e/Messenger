import { AbstractControl, ValidationErrors } from "@angular/forms";
    
export const repeatPasswordValidator = (control: AbstractControl): ValidationErrors | null => {
    const password = control.parent?.get('password')?.value ?? '';
    const repeatPassword = control.value ?? '';
    
    return password === repeatPassword 
        ? null 
        : { repeatPassword: 'Passwords do not match.' };
}