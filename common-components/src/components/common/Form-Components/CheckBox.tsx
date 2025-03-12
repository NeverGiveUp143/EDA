import React from 'react';
import { CheckBoxProps } from './propTypes';



const CheckBox: React.FC<CheckBoxProps> = ({ label, checked, onChange }) => {
  return (
    <div className="checkbox-container">
      <input
        type="checkbox"
        checked={checked}
        onChange={(e) => onChange(label, e.target.checked)}
      />
    </div>
  );
};

export default CheckBox;
