import React from 'react';

import './App.css';
import { Grid, TabBar } from './components';
import { ScreenLoaderParams, TabProps } from './components/Tab/propTypes';
import { GridProps } from './components/Grid/propTypes';
import { FieldConfig } from './components/Form/propTypes';
import Form from './components/Form/Form';

function App() {
  const configData: Record<string, FieldConfig> = {
    "First Name": { type: "TextField", defaultValue: "" },
    "Middle Name": { type: "TextField", defaultValue: "" },
    "Last Name": { type: "TextField", defaultValue: "" },
    "Display Name": { type: "TextField", defaultValue: "" },
    "Date OF Birth": { type: "Calendar", defaultValue: "" },
    "Physically Handicapped": { type: "CheckBox", defaultValue: false }
  };

  const data : GridProps = {
    url : 'https://localhost:7249/Customer/GetCustomers'
  } 

  const screenLoader = ({ screenId } : ScreenLoaderParams) => {
    switch (screenId) {
      case "customers":
        return <Grid {...data} />;
      case "checkout":
        return <Form configData={configData} />;
      default:
        break;
    }
};

  const tabs : TabProps = {
     tabs : [
       {id : 'customers', label : 'Customers Checkout History', disabled : false} ,
       {id : 'checkout', label : 'Customer Checkout', disabled : false} 
      ],
     defaultTab : 'customers',
    screenLoader : screenLoader
  }

  return (
    <div className="App">
      <TabBar {...tabs} />
    </div>
  );
}

export default App;
