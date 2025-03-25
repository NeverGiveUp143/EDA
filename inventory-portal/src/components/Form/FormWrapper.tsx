import { Box } from "@mui/material";

const FormWrapper = ({ children }: { children: React.ReactNode }) => {
  return (
    <Box
      sx={{
        display: "flex",
        flexDirection: "column",
        gap: 3, 
        width: "400px",
        margin: "auto",
        padding: 2,
        border: "1px solid #ccc",
        borderRadius: "8px",
        backgroundColor: "#f9f9f9",
      }}
    >
      {children}
    </Box>
  );
};

export default FormWrapper;
