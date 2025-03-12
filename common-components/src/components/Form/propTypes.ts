
export interface FieldConfig  {
    type: string;
    defaultValue: string | boolean;
    url?: string;
  };

export interface FormProps {
    url?: string;
    configData : Record<string, FieldConfig>;
  }

export interface ComponentProps {
    type: string;
    field: string;
    value: any;
    url?: string;
    onChange: (field: string, value: any) => void
  }