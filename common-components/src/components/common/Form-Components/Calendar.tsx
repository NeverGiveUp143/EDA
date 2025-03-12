import React from 'react';
import { CalendarProps } from './propTypes';



const Calendar: React.FC<CalendarProps> = ({ label, value, onChange }) => {
  return (
    <div className="calendar-container">
      <input
        type="date"
        value={value}
        onChange={(e) => onChange(label, e.target.value)}
        className="calendar"
      />
    </div>
  );
};

export default Calendar;
