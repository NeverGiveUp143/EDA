import React from 'react';
import { CheckBoxProps } from './propTypes';
import { Typography } from '@mui/material';



const CheckBox: React.FC<CheckBoxProps> = ({ label, checked, register, errors }) => {
  return (
    <div className="checkbox-container">
      <input
        type="checkbox"
        checked={checked}
        {...register(label)}
        style={{ height: "25px" }}
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

export default CheckBox;
