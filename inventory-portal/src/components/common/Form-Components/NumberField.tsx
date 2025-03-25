import React from 'react';
import { NumberFieldProps } from './propTypes';
import { Typography } from '@mui/material';



const NumberField: React.FC<NumberFieldProps> = ({ label, value, register, errors,style}) => {
  return (
    <div className="input-container">
      <input
        type="number"
        value={value}
        {...register(label)}
        style={style ?? {}}
        className="text-field"
        onPaste={(e) => {
            e.preventDefault();
          }}
        onKeyDown={(e) => {
            if (/[+\-.]/.test(e.key) || e.key === "e" || e.key === "E") {
              e.preventDefault(); 
            }
          }}
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

export default NumberField;
