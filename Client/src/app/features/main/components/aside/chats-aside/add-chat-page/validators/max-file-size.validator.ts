import { AbstractControl, ValidationErrors } from "@angular/forms";

export const maxFileSizeValidator = (control: AbstractControl): ValidationErrors | null => {
    const maxFileSizeInBytes = 5 * 1024 * 1024; // 5MB
    const file: File | null = control.value ?? null;

    if (!file) {
        return null;
    }

    if (file.size > maxFileSizeInBytes) {
        return { maxSize: 'File size must be at most 5MB.' };
    }

    return null;
}