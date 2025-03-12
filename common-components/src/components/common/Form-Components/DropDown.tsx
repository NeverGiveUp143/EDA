import React, { useEffect, useState } from 'react';
import useFetch from '../../../utils/useFetch';
import { Typography } from '@mui/material';
import { DropDownProps } from './propTypes';



const DropDown: React.FC<DropDownProps> = ({ label, value, url, onChange }) => {
  const [options , setOptions] = useState<string[]>([]);
  const {data , loading , error} = useFetch<string[]>(url);

  useEffect(() => {
    var fetchOptions = async () => {
       if (data !== null && data !== undefined) {
          setOptions(data);
        }
    }
    fetchOptions();
  })

  return (
    <div className="dropdown-container">
      {!loading && data == null && (
        <Typography variant="h6">Connecting...</Typography>
      )}
      {loading && data != null && data?.length === 0 && (
        <Typography variant="h6"> No Data Received</Typography>
      )}
      {error && (
        <Typography variant="h6" color="error">
          Error: {error}
        </Typography>
      )}
      {!loading && data != null && data.length > 0 && (
        <select
          value={value}
          onChange={(e) => onChange(label, e.target.value)}
          className="dropdown"
        >
          {options.map((option, index) => (
            <option key={index} value={option}>
              {option}
            </option>
          ))}
        </select>
      )}
    </div>
  );
};

export default DropDown;
