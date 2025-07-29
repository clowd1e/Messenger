import { signal, WritableSignal } from "@angular/core";
import { ValidationErrors } from "@angular/forms";

export type FormControlConfiguration = {
    controlInvalid: WritableSignal<boolean>;
    controlTouchedOrDirty: WritableSignal<boolean>;
    controlErrors: WritableSignal<ValidationErrors | null>;
    errorMessagesConfig: WritableSignal<Record<string, string>>;
}

export function createFormControlConfiguration(
    errorMessages: Record<string, string>
): FormControlConfiguration {
  return {
    controlInvalid: signal(false),
    controlTouchedOrDirty: signal(false),
    controlErrors: signal<ValidationErrors | null>(null),
    errorMessagesConfig: signal(errorMessages)
  };
}