import React from 'react';

import './App.css';
import { Grid, TabBar } from './components';
import { ScreenLoaderParams, TabProps } from './components/Tab/propTypes';
import { GridProps } from './components/Grid/propTypes';

function App() {

  const data : GridProps = {
    url : 'https://localhost:7249/Customer/GetCustomers'
  } 

  const screenLoader = ({ screenId } : ScreenLoaderParams) => {
    switch (screenId) {
      case "customers":
        return <Grid {...data} />;
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
