import React from "react";
import ReactDOM from "react-dom/client";
import { BrowserRouter } from "react-router-dom";
import RouterCustom from "./pages/router";
import "./style/style.scss";
import { UserProvider } from "context/UserContext";

const root = ReactDOM.createRoot(document.getElementById("root"));

root.render(
  <React.StrictMode>
    <UserProvider>
    <BrowserRouter basename="/FE-RadzenBook-Store">
      <RouterCustom />
    </BrowserRouter>
    </UserProvider>
  </React.StrictMode>
);
