import React, { useState, useEffect } from "react";
import { DataGrid } from "@mui/x-data-grid";
import { Box, Typography } from "@mui/material";
import { GridProps } from "./propTypes";
import useFetch from "../../utils/useFetch";

const Grid = (props: GridProps) => {
    const { url } = props;
    const { data, loading, error } = useFetch<Record<string, any>[]>(url);

    const [columns, setColumns] = useState<any[]>([]);
    const [rows, setRows] = useState<Record<string, unknown>[]>([]);

    useEffect(() => {
        if (data != null && data?.length > 0) {
            const cols = Object.keys(data[0]).map((key) => ({
                field: key,
                headerName: key.toUpperCase(),
                width: 150,
            }));
            setColumns(cols);
            setRows(data);
        }
    }, [data]);

    return (
        <Box sx={{ height: 400, width: "100%" }}>
            {!loading && data === null && error == null && <Typography variant="h6">Connecting...</Typography>}
            {loading && data != null && data.length === 0 && <Typography variant="h6"> No Data Received</Typography>}
            {error && <Typography variant="h6" color="error">Error: {error}</Typography>}
            {!loading && data != null && data?.length > 0 && (
                <DataGrid
                    rows={rows}
                    columns={columns}
                    initialState={{
                        pagination: { paginationModel: { pageSize: 5 } },
                    }}
                    pageSizeOptions={[5]}
                    disableRowSelectionOnClick
                />
            )}
        </Box>
    );
};

export default Grid;
