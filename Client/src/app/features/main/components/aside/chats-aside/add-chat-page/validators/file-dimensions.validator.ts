import { AbstractControl, ValidationErrors } from "@angular/forms";

export const fileDimensionsValidator = (control: AbstractControl): Promise<ValidationErrors | null> => {
  const file = control.value as File | null;
  if (!file) {
    return Promise.resolve(null);
  }

  return new Promise<ValidationErrors | null>((resolve) => {
    const img = new Image();
    img.src = URL.createObjectURL(file);
    img.onload = async () => {
      if (img.width !== img.height) {
        resolve({ dimensions: 'Image dimensions must be 1x1 ratio' });
      } else {
        resolve(null);
      }
    };
    img.onerror = () => resolve({ invalidImage: true });
  });
};