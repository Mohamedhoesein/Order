type RequiredPart<Property extends string[], T = string | boolean> = {[key in Property[number]]: T;};
type OptionalPart<Property extends string[], T = string | boolean> = {[key in Property[number]]?: T;};

export interface FormState<Property extends string[]> {
    formData: RequiredPart<Property, string>;
    formErrors: RequiredPart<Property, string>;
    formHasError: RequiredPart<Property, boolean>;
    disabled: boolean;
};

export interface InitializeArguments<Property extends string[]> {
    keys: Readonly<Property>,
    values?: OptionalPart<Property, string>,
    errors: RequiredPart<Property, string>,
    disabled?: boolean,
    otherValues: any
}

export const initializeForm = <Property extends string[], State extends FormState<Property>>(
    {
        keys,
        values = {},
        errors,
        disabled = false,
        otherValues
    }: InitializeArguments<Property>
): State => {
    let state: State = {
        formData: {} as RequiredPart<Property>,
        formErrors: errors,
        formHasError: {} as RequiredPart<Property>,
        disabled: disabled,
        ...otherValues
    } as State;
    for (const key of keys) {
        const usableRequiredKey = key as keyof RequiredPart<Property>;
        const usableOptionalKey = key as keyof OptionalPart<Property>;
        if (values[usableOptionalKey]) {
            state.formData[usableRequiredKey] = values[usableOptionalKey] as string;
        }
        else {
            state.formData[usableRequiredKey] = "";
        }
        state.formHasError[usableRequiredKey] = false;
    }
    return state;
};

export type onInputChange<Property extends string, State extends FormState<[Property]>> = (state: State) => void;
export type onFormSubmit<State extends FormState<[]>> = (state: State) => void;

export interface InputProps<Property extends string, State extends FormState<[Property]>> {
    name: Property;
    onChange: onInputChange<Property, State>;
    partialWidth?: boolean;
    pattern?: RegExp;
    required?: boolean;
    state: State;
    type: React.HTMLInputTypeAttribute;
    additionalCondition?: (text: string) => boolean;
    disabled?: boolean;
};

export interface SubmitProps<State extends FormState<[]>> {
    onSubmit: onFormSubmit<State>;
    partialWidth?: boolean;
    state: State;
    text: string;
}