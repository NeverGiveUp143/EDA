import { ReactElement } from "react";
import * as yup from "yup";

export interface TabProps {
    tabs: TabInterface[];
    defaultTab: string;
    screenLoader: (params: ScreenLoaderParams) => ReactElement | undefined;
}

export interface ScreenLoaderParams {
    screenId: string;
}

export interface TabInterface {
    id: string;
    label: string;
    disabled?: boolean;
}

export interface GridProps {
    url : string
}

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
