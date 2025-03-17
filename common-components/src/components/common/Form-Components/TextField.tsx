import React from 'react';
import { TextFieldProps } from './propTypes';
import { Typography } from '@mui/material';



const TextField: React.FC<TextFieldProps> = ({ label, value, register, errors}) => {
  return (
    <div className="input-container">
      <input
        type="text"
        value={value}
        {...register(label)}
        style={{ height: "25px" }}
        className="text-field"
      />
      {errors[label] && (
        <Typography
          color="error"
          style={{
            position: "absolute",
            fontSize: "small",
          }}
        >
          {errors[label].message}
        </Typography>
      )}
    </div>
  );
};

export default TextField;
