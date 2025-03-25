
import * as yup from "yup";
import { FieldConfig } from "../components/Form/propTypes";
import { TabBar } from "../components/Tab";
import { ScreenLoaderParams, TabProps } from "../components/Tab/propTypes";
import { Grid } from "../components/Grid";
import { GridProps } from "../components/Grid/propTypes";
import { Form } from "../components/Form";

const Inventory = () => {
      
  const configData: Record<string, FieldConfig> = {
    "Name": { type: "TextField", defaultValue: "" , style : { height: "25px" }},
    "Quantity": { type: "NumberField", defaultValue: 0,  style : { height: "25px" }},
  };
  

  const formValidationSchema = yup.object().shape({
    Name: yup.string()?.trim().required("Name is required"),
    Quantity: yup
      .number()
      .typeError("Quantity must be a number")
      .required("Quantity is required")
      .integer("Quantity must be an integer")
      .min(1, "Must be at least 1")
      .max(999, 'Maximum 3 characters allowed'),
  });
  
  const data : GridProps = {
    url : 'https://localhost:7204/Inventory/GetProducts'
  } 

  const screenLoader = ({ screenId } : ScreenLoaderParams) => {
    switch (screenId) {
      case "products":
        return <Grid {...data} />;
      case "addProducts":
        return <Form  postUrl="https://localhost:7204/Inventory/AddProduct" configData={configData}  formValidationSchema={formValidationSchema}/>;
      default:
        break;
    }
};

  const tabs : TabProps = {
     tabs : [
       {id : 'products', label : 'Products', disabled : false} ,
       {id : 'addProducts', label : 'Add Product', disabled : false} 
      ],
     defaultTab : 'products',
    screenLoader : screenLoader
  }


    return (
      <>      
            <TabBar {...tabs} />
      </>
    );

}


export default Inventory;