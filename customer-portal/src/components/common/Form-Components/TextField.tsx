import React from 'react';
import { TextFieldProps } from './propTypes';
import { Typography } from '@mui/material';



const TextField: React.FC<TextFieldProps> = ({ label, value, register, errors, style}) => {
  return (
    <div className="input-container">
      <input
        type="text"
        value={value}
        {...register(label)}
        style={style ?? {}}
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
