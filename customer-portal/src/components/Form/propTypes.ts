import * as yup from "yup";

export interface FieldConfig  {
    type: string;
    defaultValue: any;
    style?: any;
    url?: string;
  };

export interface FormProps {
    postUrl: string;
    configData : Record<string, FieldConfig>;
    formValidationSchema :  yup.InferType<any>;
  }

export interface ComponentProps {
    type: string;
    field: string;
    style?: any;
    watch: any;
    url?: string;
    register : any,
    errors : any
  }