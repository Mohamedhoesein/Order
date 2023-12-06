import { FormControl, ValidationErrors, ValidatorFn, Validators } from "@angular/forms";
import { ErrorStateMatcher } from "@angular/material/core";

export type InputTypes = "button" | "checkbox" | "color" | "date" | "datetime-local" | "email" | "file" | "hidden" | "image" | "month" | "number" | "password" | "radio" | "range" | "reset" | "search" | "submit" | "tel" | "text" | "time" | "url" | "week";

export type ErrorMessage = {
  id: number;
  key: string;
  message?: string;
  serverError: boolean;
}

export type Control = {
  name: string;
  label: string;
  type: InputTypes;
  control: FormControl;
  errorStateMatcher: ErrorStateMatcher;
  blur: (control: Control) => Control;
  placeholder: string;
  errors: ErrorMessage[];
}

export type ControlInfo = {
  id?: string,
  name: string,
  hasServerError?: boolean,
  placeHolder: string
}

export type Validator = {
  key: string,
  message: string,
  validator: ValidatorFn
}

export type ExtendedControlInfo = ControlInfo & {
  type: InputTypes,
  validators: Validator[]
}

export abstract class BaseFormComponent {
  public constructor(protected _matcher: ErrorStateMatcher) { }

  public static getTextFormControl(): FormControl<string>{
    return new FormControl<string>(
      '',
      {
        nonNullable: true,
        validators: [Validators.required]
      }
    );
  }
  public static getEmailFormControl(): FormControl<string>{
    return new FormControl<string>(
      '',
      {
        nonNullable: true,
        validators: [Validators.required, Validators.email]
      }
    );
  }
  public static getPasswordFormControl(): FormControl<string>{
    return new FormControl<string>(
      '',
      {
        nonNullable: true,
        validators: [Validators.required, Validators.minLength(10), Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W\D)[a-zA-Z\d\W\D]{10,}$/)]
      }
    );
  }

  public createTextControl({ id, name, hasServerError, placeHolder }: ControlInfo): Control {
    const processedName: string = this.processName(name);
    hasServerError = hasServerError ?? true;
    return {
      name: id ?? name,
      label: processedName,
      type: 'text',
      control: BaseFormComponent.getTextFormControl(),
      errorStateMatcher: this._matcher,
      blur: this.removeServerError.bind(this),
      placeholder: placeHolder,
      errors: this.generateTextErrorMessages(processedName, hasServerError)
    };
  }

  public createEmailControl({ id, name, hasServerError, placeHolder }: ControlInfo): Control {
    const processedName: string = this.processName(name);
    hasServerError = hasServerError ?? true;
    return {
      name: id ?? name,
      label: processedName,
      type: 'email',
      control: BaseFormComponent.getEmailFormControl(),
      errorStateMatcher: this._matcher,
      blur: this.removeServerError.bind(this),
      placeholder: placeHolder,
      errors: this.generateEmailErrorMessages(processedName, hasServerError)
    };
  }

  public createPasswordControl({ id, name, hasServerError, placeHolder }: ControlInfo): Control {
    const processedName: string = this.processName(name);
    hasServerError = hasServerError ?? true;
    return {
      name: id ?? name,
      label: processedName,
      type: 'password',
      control: BaseFormComponent.getPasswordFormControl(),
      errorStateMatcher: this._matcher,
      blur: this.removeServerError.bind(this),
      placeholder: placeHolder,
      errors: this.generatePasswordErrorMessages(processedName, hasServerError)
    };
  }

  public createExtendedRequiredControl({ id, name, hasServerError, placeHolder, validators, type }: ExtendedControlInfo): Control {
    const processedName: string = this.processName(name);
    hasServerError = hasServerError ?? true;
    const control: FormControl<string> = this.createExtendedFormControl(BaseFormComponent.getTextFormControl(), validators.map(validator => validator.validator));
    let errors: ErrorMessage[] = this.generateTextErrorMessages(processedName, hasServerError);
    validators.forEach(validator => {
      errors.push({
        id: errors.length,
        key: validator.key,
        message: validator.message,
        serverError: false
      });
    });
    return {
      name: id ?? name,
      label: processedName,
      type: type,
      control: control,
      errorStateMatcher: this._matcher,
      blur: this.removeServerError.bind(this),
      placeholder: placeHolder,
      errors: errors
    }
  }

  public removeError(error: string, formControl: Control): Control {
    let errors: ValidationErrors | null = formControl.control.errors;
    if (errors === null)
      return formControl;
    delete errors[error];
    formControl.control.setErrors(errors);
    formControl.control.markAsTouched();
    formControl.control.markAsDirty();
    return formControl;
  }

  public removeServerError(formControl: Control): Control {
    return this.removeError('serverError', formControl);
  }

  public setError(formControl: Control, error: string[]): Control {
    if (error.length > 0) {
      formControl.control.setErrors({
        ...formControl.control.errors,
        serverError: error[0]
      });
    }
    return formControl;
  }

  private generateTextErrorMessages(name: string, hasServerError: boolean): ErrorMessage[] {
    let errors: ErrorMessage[] = [];

    if (hasServerError) {
      errors.push(this.serverError());
      errors.push(this.requiredError(1, name));
    }
    else {
      errors.push(this.requiredError(0, name));
    }
    return errors;
  }

  private createExtendedFormControl(formControl: FormControl, validators: ValidatorFn[]): FormControl {
    formControl.addValidators(validators);
    return formControl;
  }

  private generateEmailErrorMessages(name: string, hasServerError: boolean): ErrorMessage[] {
    let errors: ErrorMessage[] = [];

    let baseId: number = 0;

    if (hasServerError) {
      errors.push(this.serverError());
      baseId = 1;
    }
  
    errors.push(this.requiredError(baseId++, name));
    errors.push(this.emailError(baseId));
    return errors;
  }

  private generatePasswordErrorMessages(name: string, hasServerError: boolean): ErrorMessage[] {
    let errors: ErrorMessage[] = [];
    
    let baseId: number = 0;

    if (hasServerError) {
      errors.push(this.serverError());
      baseId = 1;
    }

    errors.push(this.requiredError(baseId++, name));
    errors.push(this.minLengthError(baseId++, name, 10));
    errors.push(this.passwordPatternError(baseId++));
    return errors;
  }

  private requiredError(id: number, name: string): ErrorMessage {
    return {
      id: id,
      key: 'required',
      message: `${name} is required.`,
      serverError: false
    }
  }

  private emailError(id: number): ErrorMessage {
    return {
      id: id,
      key: 'email',
      message: 'Please enter a valid email address.',
      serverError: false
    }
  }

  private minLengthError(id: number, name: string, length: number): ErrorMessage {
    return {
      id: id,
      key: 'minlength',
      message: `${name} must be at least ${length} characters long.`,
      serverError: false
    }
  }

  private passwordPatternError(id: number): ErrorMessage {
    return {
      id: id,
      key: 'pattern',
      message: `Password must have a lower and upper case letter, digit, and special character.`,
      serverError: false
    }
  }

  private serverError(): ErrorMessage {
    return {
      id: 0,
      key: 'serverError',
      serverError: true
    }
  }

  private processName(name: string): string {
    return this.split(this.toUpperCase(name));
  }

  private toUpperCase(name: string): string {
    return `${name[0].toUpperCase()}${name.substring(1)}`;
  }

  private split(key: string): string {
    return key.split(/(?=[A-Z])/).join(' ');
  }
}
