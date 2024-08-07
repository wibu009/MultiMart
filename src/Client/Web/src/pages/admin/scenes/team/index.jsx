import { Box, IconButton, Typography, useTheme, Tooltip } from "@mui/material";
import { useState, useEffect } from "react";
import { DataGrid } from "@mui/x-data-grid";
import { token } from "../theme";
import { mockDataTeam } from "../../data/mockData";
import AdminPanelSettingsOutlinedIcon from "@mui/icons-material/AdminPanelSettingsOutlined";
import LockOpenOutlinedIcon from "@mui/icons-material/LockOpenOutlined";
import SecurityOutlinedIcon from "@mui/icons-material/SecurityOutlined";
import Header from "../../components/Header";
import { memo } from "react";
import FormDialog from "pages/admin/components/FormDialog";
import AddIcon from "@mui/icons-material/Add";
import EditIcon from '@mui/icons-material/Edit';
import RemoveIcon from '@mui/icons-material/Remove';
import { AllUser } from "services/AllServices";

const Team = () => {
  const theme = useTheme();
  const colors = token(theme.palette.mode);
  const [open, setOpen] = useState(false);
  const [selectedRow, setSelectedRow] = useState(null);


  const [listUser, setListUser] = useState([]);

  useEffect(() => {
    getAllUser();
  }, [])

  const getAllUser = async () => {
    let response = await AllUser();
    console.log('resss', response);
    if (response && response.data) {
      setListUser(response.data)
    }
  }

  console.log('list>>>>>>', listUser);

  const columns = [
    { field: "id", headerName: "ID" },
    {
      field: "first_name",
      headerName: "First Name",
      flex: 1,
      cellClassName: "name-column--cell",
    },
    {
      field: "email",
      headerName: "Email",
      flex: 1,
    },
    {
      field: "accessLevel",
      headerName: "Access Level",
      flex: 1,
      renderCell: ({ row: { access } }) => {
        return (
          <Box
            width="60%"
            m="0 auto"
            p="5px"
            display="flex"
            justifyContent="center"
            backgroundColor={
              access === "admin"
                ? colors.greenAccent[600]
                : access === "manager"
                  ? colors.greenAccent[700]
                  : colors.greenAccent[700]
            }
            borderRadius="4px"
          >
            {access === "admin" && <AdminPanelSettingsOutlinedIcon />}
            {access === "manager" && <SecurityOutlinedIcon />}
            {access === "user" && <LockOpenOutlinedIcon />}
            <Typography color={colors.grey[100]} sx={{ ml: "5px" }}>
              {access}
            </Typography>
          </Box>
        );
      },
    },
  ];

  const handleSelectionModelChange = (selectionModel) => {
    const selectedID = selectionModel[0]; // Assuming single selection
    const selectedRowData = mockDataTeam.find((row) => row.id === selectedID);
    setSelectedRow(selectedRowData);
  };

  const handleEditClick = () => {
    if (selectedRow) {
      console.log(selectedRow);
      setOpen(true);
    } else {
      alert("Please select a row to edit");
    }
  };

  return (
    <Box m="20px">
      <Box
        display="flex"
        flexDirection="row"
        justifyContent="space-between"
        alignItems="center"
        mb={2}
      >
        <Header title="Người dùng" subtitle="Managing the Team Members" />
        <Box>
          <Tooltip title="Thêm">
            <IconButton color="primary" onClick={() => setOpen(true)}>
              <AddIcon />
            </IconButton>
          </Tooltip>
          <Tooltip title="Sửa">
            <IconButton color="primary" onClick={handleEditClick}>
              <EditIcon />
            </IconButton>
          </Tooltip>
          <Tooltip title="Xóa">
            <IconButton color="primary" onClick={() => setOpen(true)}>
              <RemoveIcon />
            </IconButton>
          </Tooltip>
        </Box>
      </Box>

      <Box
        m="40px 0 0 0"
        height="75vh"
        sx={{
          "& .MuiDataGrid-root": {
            border: "none",
          },
          "& .MuiDataGrid-cell": {
            borderBottom: "none",
          },
          "& .name-column--cell": {
            color: colors.greenAccent[300],
          },
          "& .MuiDataGrid-columnHeaders": {
            backgroundColor: colors.blueAccent[700],
            borderBottom: "none",
          },
          "& .MuiDataGrid-virtualScroller": {
            backgroundColor: colors.primary[400],
          },
          "& .MuiDataGrid-footerContainer": {
            borderTop: "none",
            backgroundColor: colors.blueAccent[700],
          },
          "& .MuiCheckbox-root": {
            color: `${colors.greenAccent[200]} !important`,
          },
        }}
      >
        <DataGrid checkboxSelection rows={listUser} columns={columns} onRowSelectionModelChange={handleSelectionModelChange} />
      </Box>
      <FormDialog open={open} onClose={() => setOpen(false)} initialValues={selectedRow} />

    </Box>
  );
};

export default memo(Team);
