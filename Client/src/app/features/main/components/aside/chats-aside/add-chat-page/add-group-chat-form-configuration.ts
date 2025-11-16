import { createFormControlConfiguration, FormControlConfiguration } from "../../../../../../shared/models/configurations/forms/form-control-configuration";

export const addGroupChatFormConfiguration: Record<string, FormControlConfiguration> = {
    groupName: createFormControlConfiguration({
        required: 'Group name is required',
        minlength: 'Group name must be at least 3 characters long',
        maxlength: 'Group name must be at most 50 characters long'
    }),
    groupDescription: createFormControlConfiguration({
        minlength: 'Description must be at least 1 characters long',
        maxlength: 'Description must be at most 200 characters long'
    }),
    groupIcon: createFormControlConfiguration({
        maxSize: 'Icon file size must be at most 5MB',
        dimensions: 'Image dimensions must be 1x1 ratio'
    })
};