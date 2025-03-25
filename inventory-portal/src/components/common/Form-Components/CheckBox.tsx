import React from 'react';
import { CheckBoxProps } from './propTypes';
import { Typography } from '@mui/material';



const CheckBox: React.FC<CheckBoxProps> = ({ label, checked, register, errors, style }) => {
  return (
    <div className="checkbox-container">
      <input
        type="checkbox"
        checked={checked}
        {...register(label)}
        style={style ?? {}}
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
