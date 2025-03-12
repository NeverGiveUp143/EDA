import React from 'react';
import { TextFieldProps } from './propTypes';



const TextField: React.FC<TextFieldProps> = ({ label, value, onChange }) => {
  return (
    <div className="input-container">
      <input
        type="text"
        value={value}
        onChange={(e) => onChange(label, e.target.value)}
        className="text-field"
      />
    </div>
  );
};

export default TextField;
