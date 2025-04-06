
import * as yup from "yup";
import { FieldConfig } from "../components/Form/propTypes";
import { TabBar } from "../components/Tab";
import { ScreenLoaderParams, TabProps } from "../components/Tab/propTypes";
import { Grid } from "../components/Grid";
import { GridProps } from "../components/Grid/propTypes";
import { Form } from "../components/Form";

const Customer = () => {
   
  const configData: Record<string, FieldConfig> = {
    "Name": { type: "TextField", defaultValue: "" , style : { height: "25px" }},
    "Product": { type: "DropDown", defaultValue: "" , url : 'https://localhost:7249/Customer/GetProductDDList', style : { height: "34px" , width: "175px" }},
    "ItemInCart": { type: "NumberField", defaultValue: 0,  style : { height: "25px" }},
    "Email": { type: "TextField", defaultValue: "" , style : { height: "25px" }}
  };
  
  const formValidationSchema = yup.object().shape({
    Name: yup.string()?.trim().required("Name is required"),
    Product: yup.string()?.trim().required("Products is required"),
    ItemInCart: yup
      .number()
      .typeError("Item In Cart must be a number")
      .required("Item In Cart is required")
      .integer("Item In Cart must be an integer")
      .min(1, "Must be at least 1")
      .max(999, 'Maximum 3 characters allowed'),
      Email : yup.string()?.trim().required("Email is required").email("Email is not valid"),
  });
  
  const data : GridProps = {
    url : 'https://localhost:7249/Customer/GetCustomers'
  } 

  const screenLoader = ({ screenId , RedirectToDefaultTab } : ScreenLoaderParams) => {
    switch (screenId) {
      case "customersList":
        return <Grid {...data} />;
      case "customercheckout":
        return <Form  postUrl="https://localhost:7249/Customer/AddCustomer" configData={configData}  formValidationSchema={formValidationSchema} RedirectToDefaultTab={RedirectToDefaultTab}/>;
      default:
        break;
    }
};

  const tabs : TabProps = {
     tabs : [
       {id : 'customersList', label : 'Customers Checkout History', disabled : false} ,
       {id : 'customercheckout', label : 'Customer Checkout', disabled : false} 
      ],
     defaultTab : 'customersList',
    screenLoader : screenLoader
  }


    return (
      <>      
            <TabBar {...tabs} />
      </>
    );

}


export default Customer;