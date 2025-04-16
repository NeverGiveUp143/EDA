import React from 'react';
import { CalendarProps } from './propTypes';
import { Typography } from '@mui/material';



const Calendar: React.FC<CalendarProps> = ({ label, value, register, errors, style }) => {
  return (
    <div className="calendar-container">
      <input
        type="date"
        value={value}
        {...register(label)}
        style={style ?? {}}
        className="calendar"
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

export default Calendar;
