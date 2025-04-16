import Snackbar from '@mui/material/Snackbar';
import Alert from '@mui/material/Alert';
import { ToastProps } from './propTypes';

const Toast = (props : ToastProps) => {
  const {message , severity , isOpen , onClose} = props;
  
  return (
    <div>
      <Snackbar open={isOpen} autoHideDuration={3000} onClose={onClose} anchorOrigin={{ vertical: 'top', horizontal: 'right' }}>
        <Alert         
          onClose={onClose}
          severity= {severity}
          variant="filled"
          sx={{ width: '100%' }}
        >   
          {message}
        </Alert>
      </Snackbar>
    </div>
  );
}

export default Toast;