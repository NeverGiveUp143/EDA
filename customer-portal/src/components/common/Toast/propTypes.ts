export interface ToastProps {
    message: string;
    severity: "error" | "info" | "success" | "warning";
    isOpen: boolean;
    onClose: () => void;
}

export enum Severity {
    Error = "error",
    Success = "success",
    Info = "info",
    Warning = "warning"
  }