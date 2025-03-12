import { useEffect, useState } from "react";
import useFetch from "../../utils/useFetch";
import { FormProps } from "./propTypes";
import { Box, Button, Grid2, Typography } from "@mui/material";
import { Calendar, CheckBox, DropDown, TextField } from '../common';
import { ComponentProps } from './propTypes';
import FormWrapper from "./FormWrapper";

const RenderFormFields = ({ type, field, value, url, onChange }: ComponentProps)  => {
  switch (type) {
    case "TextField":
      return <TextField label={field} value={value as string} onChange={onChange} />;
    case "DropDown":
      return <DropDown label={field} value={value as string} url = {url || ''} onChange={onChange} />;
    case "Calendar":
      return <Calendar label={field} value={value as string} onChange={onChange} />;
    case "CheckBox":
      return <CheckBox label={field} checked={value as boolean} onChange={onChange} />;
    default:
      return null;
  }

};


const Form = ({ url, configData }: FormProps) => {
  const [formData, setFormData] = useState<Record<string, any>>({});
  const { data, loading, error } = useFetch<Record<string, any>[]>(url?.trim() || "");

  useEffect(() => {
    const initialFormData: Record<string, any> = {};
    
    Object.keys(configData).forEach((field) => {
      initialFormData[field] = configData[field]?.defaultValue || "";
    });
  
    setFormData(() => ({
      ...initialFormData, 
      ...data, 
    }));
  }, [data, configData]);


  const handleChange = (field: string, value: any) => {
    setFormData((prev) => ({
      ...prev,
      [field]: value,
    }));
  };

  if (!loading && !data && url?.trim()) {
    return <Typography variant="h6">Connecting...</Typography>;
  }

  if (loading && (!data || data.length === 0) && url?.trim()) {
    return <Typography variant="h6">No Data Received</Typography>;
  }

  if (error) {
    return (
      <Typography variant="h6" color="error">
        Error: {error}
      </Typography>
    );
  }

  return (
    <Box sx={{ width: "100%" , marginTop : '15px', padding : '15px'}}>
      <form>
        <FormWrapper>
          {Object.keys(configData).map((field) => {
            const { type = "",defaultValue = "" , url = "" } = configData[field] || {};
            const value = formData[field] || defaultValue;
            return (
              <Grid2 container key={field} alignItems="center" spacing={2}>
                <Grid2
                  size={6}
                  style={{ textAlign: "right", paddingRight: "10px" }}
                >
                  <Typography
                    variant="body1"
                    style={{ textAlign: "left", width: "100%" }}
                  >
                    {field}
                  </Typography>
                </Grid2>
                <Grid2
                  size={6}
                  style={{ display: "flex", alignItems: "center" }}
                >
                  {RenderFormFields({
                    type,
                    field,
                    value,
                    onChange: handleChange,
                    url,
                  })}
                </Grid2>
              </Grid2>
            );
          })}
          <br />
          <Button
            type="submit"
            variant="contained"
            color="primary"
            style={{ marginTop: "10px" }}
            onClick={(e: React.FormEvent) => {
              e.preventDefault();
              console.log(formData);
            }}
          >
            Submit
          </Button>
        </FormWrapper>
      </form>
    </Box>
  );
};

export default Form;
