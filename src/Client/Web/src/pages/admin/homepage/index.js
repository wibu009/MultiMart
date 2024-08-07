import { useState } from "react";
// import { Routes, Route } from "react-router-dom";
import SideBar from "../scenes/global/SideBar";
import Topbar from "../scenes/global/Topbar";
import { ColorModeContext, useMode } from "../scenes/theme";
import { CssBaseline, ThemeProvider } from "@mui/material";
// import Dashboard from "../scenes/dashboard";
// import Team from "./scenes/team";
// import Invoices from "./scenes/invoices";
// import Contacts from "./scenes/contacts";
// import Bar from "./scenes/bar";
// import Form from "./scenes/form";
// import Line from "./scenes/line";
// import Pie from "./scenes/pie";
// import FAQ from "./scenes/faq";
// import Geography from "./scenes/geography";
// import Calendar from "./scenes/calendar/calendar";

import { memo } from "react";

const HomePageAdmin = ({ screen, ...props }) => {
  const [theme, colorMode] = useMode();
  const [isSidebar, setIsSidebar] = useState(true);
  return (
    <div {...props}>
      <ColorModeContext.Provider value={colorMode}>
        <ThemeProvider theme={theme}>
          <CssBaseline />
          <div
            style={{
              display: "flex",
              position: "relative",
            }}
          >
            <SideBar isSidebar={isSidebar} />
            <main
              style={{
                height: "100%",
                width: "100%",
                fontFamily: "Be Vietnam Pro",
              }}
            >
              <Topbar setIsSidebar={setIsSidebar} />
              {screen}

              {/* <Route path="/team" element={<Team />} />
              <Route path="/contacts" element={<Contacts />} />
              <Route path="/invoices" element={<Invoices />} />
              <Route path="/form" element={<Form />} />
              <Route path="/bar" element={<Bar />} />
              <Route path="/pie" element={<Pie />} />
              <Route path="/line" element={<Line />} />
              <Route path="/faq" element={<FAQ />} />
              <Route path="/calendar" element={<Calendar />} />
              <Route path="/geography" element={<Geography />} /> */}
            </main>
          </div>
        </ThemeProvider>
      </ColorModeContext.Provider>
    </div>
  );
};

export default memo(HomePageAdmin);
