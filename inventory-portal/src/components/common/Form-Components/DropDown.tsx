import React, { useEffect, useState } from 'react';
import useFetch from '../../../utils/useFetch';
import { Typography } from '@mui/material';
import { DropDownProps } from './propTypes';



const DropDown: React.FC<DropDownProps> = ({ label, value, url, register, errors , style }) => { 
  const {data , loading , error} = useFetch<Record<string,any>[]>(url);
  const [options , setOptions] = useState<Record<string,any>[]>([]);

  useEffect(() => {
    var fetchOptions = async () => {
       if (data !== null && data !== undefined) {
          setOptions(data);
        }
    }
    fetchOptions();
  },[data])

  return (
    <div className="dropdown-container">
      {!loading && data == null && error === null && (
        <Typography variant="h6">Connecting...</Typography>
      )}
      {loading && data !== null && data?.length === 0 && (
        <Typography variant="h6"> No Data Received</Typography>
      )}
      {error && (
        <Typography variant="h6" color="error">
          Error: {error}
        </Typography>
      )}
      {!loading && data !== null && data.length > 0 && (
        <select
          value={value}
          {...register(label)}
          className="dropdown"
          style={style ?? {}}
        >
          <option value="" >
            Select {label}
          </option>
          {options.map((option, index) => (
            <option key={index} value={option.value}>
              {option.label}
            </option>
          ))}
        </select>
      )}
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

export default DropDown;
