type AdditionalCondition<Property extends string[], State extends FormState<Property>> = (text: string, state: State) => boolean;
type RequiredPartCondition<Property extends string[], State extends FormState<Property>, Additional extends AdditionalCondition<Property, State>> = {[key in Property[number]]: Additional;};
type OptionalPartCondition<Property extends string[], State extends FormState<Property>, Additional extends AdditionalCondition<Property, State>> = {[key in Property[number]]?: Additional;};
type RequiredPart<Property extends string[], T = string | boolean | RegExp> = {[key in Property[number]]: T;};
type OptionalPart<Property extends string[], T = string | boolean | RegExp> = {[key in Property[number]]?: T;};
export type StringRequired<Property extends string[]> = RequiredPart<Property, string>;
export type BoolRequired<Property extends string[]> = RequiredPart<Property, boolean>;
export type RegexRequired<Property extends string[]> = RequiredPart<Property, RegExp>;
export type AdditionalRequired<Property extends string[], State extends FormState<Property>> = RequiredPart<Property, AdditionalCondition<Property, State>>;
type StringOptional<Property extends string[]> = OptionalPart<Property, string>;
type RegexOptional<Property extends string[]> = OptionalPart<Property, RegExp>;
type AdditionalOptional<Property extends string[], State extends FormState<Property>> = OptionalPart<Property, AdditionalCondition<Property, State>>;

export interface FormState<Property extends string[]> {
    formData: RequiredPart<Property, string>;
    formErrors: RequiredPart<Property, string>;
    formHasError: RequiredPart<Property, boolean>;
    formAdditionalCondition: RequiredPartCondition<Property, FormState<Property>, AdditionalCondition<Property, FormState<Property>>>;
    formRequired: RequiredPart<Property, boolean>;
    formPattern: RequiredPart<Property, RegExp>;
    formDirty: RequiredPart<Property, boolean>;
    disabled: boolean;
    submitId: string;
}

export interface InitializeArguments<Property extends string[], State extends FormState<Property>> {
    keys: Readonly<Property>;
    values?: OptionalPart<Property, string>;
    errors: RequiredPart<Property, string>;
    additionalCondition?: OptionalPartCondition<Property, State, AdditionalCondition<Property, State>>;
    required?: Readonly<Property>;
    pattern?: OptionalPart<Property, RegExp>;
    disabled?: boolean;
    otherValues?: object;
    submitId: string;
}

const checkKey = <All extends string[], Property extends All[number], State extends FormState<All>>(
    key: Property,
    state: State
): boolean => {
    const usableRequiredKeyBool = key as keyof BoolRequired<All>;
    const usableRequiredKeyRegex = key as keyof RegexRequired<All>;
    const usableRequiredKeyAdditional = key as keyof AdditionalRequired<All, State>;
    const usableRequiredKeyString = key as keyof StringRequired<All>;
    const isRequired = state.formRequired[usableRequiredKeyBool];
    const pattern = state.formPattern[usableRequiredKeyRegex];
    const additionalCondition = state.formAdditionalCondition[usableRequiredKeyAdditional];
    const data = state.formData[usableRequiredKeyString];

    return (isRequired && !data) ||
        !data.match(pattern) ||
        !additionalCondition(data, state);
}

export const hasError = <Property extends string[], State extends FormState<Property>>(state: State): boolean => {
    return Object.values(state.formHasError).some(error => error);
}

export const setAllDirty = <Property extends string[], State extends FormState<Property>>(state: State): State => {
    for (const key in Object.keys(state)) {
        const usableKey = key as keyof BoolRequired<Property>;
        state.formDirty[usableKey] = true;
    }
    return state;
}

export const validate = <All extends string[], ToTest extends All[number], State extends FormState<All>>(
    keys: ToTest,
    state: State,
    change: (state: State) => void
): void => {
    const newState = state;
    const usableRequiredKey = keys as keyof BoolRequired<[ToTest]>;
    newState.formHasError[usableRequiredKey] = checkKey(usableRequiredKey, state);
    change({...newState});
}

export const submitValidate = <Property extends string[], State extends FormState<Property>>(
    keys: Readonly<Property>,
    state: State,
    changeState: (state: State) => void
): void => {
    const newState = state;

    for (const key in keys) {
        const usableRequiredKey = key as keyof BoolRequired<Property>;
        newState.formHasError[usableRequiredKey] = checkKey(keys[key], state);
    }

    if (!hasError(state))
        newState.disabled = true;
    changeState({
        ...newState,
        formHasError: {
            ...newState.formHasError
        }
    });
}

export const initializeForm = <Property extends string[], State extends FormState<Property>>(
    {
        keys,
        values = {},
        errors,
        additionalCondition = {},
        required,
        pattern = {},
        disabled = false,
        otherValues = {},
        submitId
    }: InitializeArguments<Property, State>
): State => {
    const state: State = {
        formData: {} as StringRequired<Property>,
        formErrors: errors,
        formHasError: {} as BoolRequired<Property>,
        formAdditionalCondition: {} as AdditionalRequired<Property, State>,
        formRequired: {} as BoolRequired<Property>,
        formPattern: {} as RegexRequired<Property>,
        formDirty: {} as BoolRequired<Property>,
        disabled: disabled,
        submitId: submitId,
        ...otherValues
    } as State;
    for (const key of keys) {
        const usableRequiredKeyBool = key as keyof BoolRequired<Property>;
        const usableRequiredKeyRegex = key as keyof RegexRequired<Property>;
        const usableRequiredKeyAdditional = key as keyof AdditionalRequired<Property, State>;
        const usableRequiredKeyString = key as keyof StringRequired<Property>;
        const usableOptionalKeyRegex = key as keyof RegexOptional<Property>;
        const usableOptionalKeyAdditional = key as keyof AdditionalOptional<Property, State>;
        const usableOptionalKeyString = key as keyof StringOptional<Property>;

        if (values[usableOptionalKeyString]) {
            state.formData[usableOptionalKeyString] = values[usableOptionalKeyString] as string;
        }
        else {
            state.formData[usableOptionalKeyString] = "";
        }
        if (additionalCondition[usableOptionalKeyAdditional]) {
            state.formAdditionalCondition[usableRequiredKeyAdditional] =
                additionalCondition[usableOptionalKeyAdditional] as <Property extends string[], State extends FormState<Property>>(text: string, state: State) => boolean;
        }
        else {
            state.formAdditionalCondition[usableRequiredKeyAdditional] = () => true;
        }
        state.formRequired[usableRequiredKeyBool] = required != undefined && usableRequiredKeyBool in required;
        if (pattern[usableRequiredKeyRegex]) {
            state.formPattern[usableRequiredKeyRegex] = pattern[usableOptionalKeyRegex] as RegExp;
        }
        else {
            state.formPattern[usableRequiredKeyRegex] = /.*/;
        }
        state.formDirty[usableRequiredKeyBool] = false;
        state.formHasError[usableRequiredKeyString] = checkKey(key, state);
    }
    return state;
};

export type onInputChange<Property extends string[], State extends FormState<Property>> = (state: State) => void;
export type onFormSubmit = () => void;

export interface InputProps<All extends string[], Property extends All[number], FullState extends FormState<All>> {
    name: Property;
    onChange: onInputChange<All, FullState>;
    partialWidth?: boolean;
    state: FullState;
    type: React.HTMLInputTypeAttribute;
    disabled?: boolean;
}

export interface SubmitProps<State extends FormState<[]>> {
    onSubmit: onFormSubmit;
    partialWidth?: boolean;
    state: State;
    text: string;
}

export const update = <AllProperties extends string[], FullState extends FormState<AllProperties>, Property extends string[], PartialState extends FormState<Property>>
    (fullState: FullState, setState: (state: FullState) => void): onInputChange<Property, PartialState> => {
        return (partial: PartialState): void => {
            const newState: FullState = {
                ...fullState,
                ...partial,
                formData: {
                    ...fullState.formData,
                    ...partial.formData
                },
                formHasError: {
                    ...fullState.formHasError,
                    ...partial.formHasError
                },
                formDirty: {
                    ...fullState.formDirty,
                    ...partial.formDirty
                },
                formErrors: {
                    ...fullState.formErrors,
                    ...partial.formErrors
                },
                formPattern: {
                    ...fullState.formPattern,
                    ...partial.formPattern
                },
                formRequired: {
                    ...fullState.formRequired,
                    ...partial.formRequired
                },
                formAdditionalCondition: {
                    ...fullState.formAdditionalCondition,
                    ...partial.formAdditionalCondition
                }
            }
            setState(newState);
        }
    };