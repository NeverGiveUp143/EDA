export interface CalendarProps {
  label: string;
  value: string;
  onChange: (field: string, value: any) => void;
}

export interface CheckBoxProps {
  label: string;
  checked: boolean;
  onChange: (field: string, value: any) => void;
}

export interface DropDownProps {
    label: string;
    value: string;
    url: string;
    onChange: (field: string, value: any) => void;
  }

export interface TextFieldProps {
  label: string;
  value: string;
  onChange: (field: string, value: any) => void;
}