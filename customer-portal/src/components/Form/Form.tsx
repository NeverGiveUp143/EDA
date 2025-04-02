import { useEffect, useMemo, useState } from "react";
import { Box, Button, SnackbarCloseReason, Typography } from "@mui/material";
import { useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import Calendar from "../common/Form-Components/Calendar";
import TextField from "../common/Form-Components/TextField";
import DropDown from "../common/Form-Components/DropDown";
import NumberField from "../common/Form-Components/NumberField";
import CheckBox from "../common/Form-Components/CheckBox";
import FormWrapper from "./FormWrapper";
import { FormProps, ComponentProps } from "./propTypes";
import PostApi from "../../utils/ApiHandler";
import {Toast} from "../common/Toast";
import { Severity, ToastProps } from "../common/Toast/propTypes";
import { FORMERRORMESSAGE, FORMSUCCESSMESSAGE } from "../../utils/Constants";

const RenderFormFields = ({ type, field, watch, url, register, errors , style }: ComponentProps) => {
  const value = watch(field);

  switch (type) {
    case "TextField":
      return <TextField register={register} label={field} value={value} errors={errors} style = {style}/>;
    case "DropDown":
      return <DropDown register={register} label={field} value={value} url={url ?? ""} errors={errors} style = {style}/>;
    case "Calendar":
      return <Calendar register={register} label={field} value={value} errors={errors} style = {style}/>;
    case "CheckBox":
      return <CheckBox register={register} label={field} checked={!!value} errors={errors} style = {style}/>;
    case "NumberField":
      return <NumberField register={register} label={field} value={value} errors={errors} style = {style}/>;
    default:
      return null;
  }
};

const Form = ({ postUrl, configData,formValidationSchema,RedirectToDefaultTab }: FormProps) => {
  const formData = useMemo(() => {
    return Object.fromEntries(
      Object.keys(configData).map((field) => [field, configData[field]?.defaultValue ?? ""])
    );
  }, [configData]);

  const [isOpen, setIsOpen] = useState(false);
  const [toastProps , setToastProps] = useState<ToastProps>();
  
  const {
    register,
    handleSubmit,
    setValue,
    watch,
    formState: { errors, isDirty, isValid },
  } = useForm({
    resolver: yupResolver(formValidationSchema),
    mode: "onChange",
    defaultValues: formData,
  });

  useEffect(() => {
    Object.keys(formData).forEach((field) => setValue(field, formData[field]));
  }, [formData, setValue]);


  const handleToastClose = (
    _event?: React.SyntheticEvent | Event,
    reason?: SnackbarCloseReason,
  ) => {
    if (reason === 'clickaway') {
      return;
    }

    setIsOpen(false);
  };

  const onSubmit = async (formValues: any) => {
    const result = await PostApi(postUrl, formValues);

  if (result.success) {
    setToastProps({ message: FORMSUCCESSMESSAGE, severity: Severity.Success , isOpen: true , onClose : handleToastClose});
    setIsOpen(true);
    setTimeout(() => {
      RedirectToDefaultTab();
    },2000)  
  } else {
    setToastProps({ message: result?.error ?? FORMERRORMESSAGE , severity: Severity.Error , isOpen: false , onClose : handleToastClose});
    setIsOpen(true);
  }
  };

  if (!Object.keys(formData).length) return <Typography variant="h6">No Data Received</Typography>;

  return (
    <>
    <Box sx={{ width: "100%", mt: 2, p: 2 }}>
      <form onSubmit={handleSubmit(onSubmit)}>
        <FormWrapper>
          {Object.keys(configData).map((field) => (
            <div key={field} style={{ display: "flex", alignItems: "center", marginBottom: "15px" }}>
              <Typography variant="body1" sx={{ width: "30%", fontSize: "1.2rem",textAlign: "left"}}>
                {field}
              </Typography>
              <RenderFormFields type={configData[field]?.type} field={field} url={configData[field]?.url} register={register} watch={watch} errors={errors} style={configData[field]?.style}/>
            </div>
          ))}
          <Button type="submit" variant="contained" color="primary" sx={{ mt: 2 }} disabled={!isDirty || !isValid}>
            Save
          </Button>
        </FormWrapper>
      </form>
    </Box>
      {isOpen && toastProps && toastProps.message && <Toast {...toastProps} />}
    </>
  );
};

export default Form;
