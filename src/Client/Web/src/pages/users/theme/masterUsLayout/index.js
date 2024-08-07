import { memo } from "react";
import FooterUS from "../footer";
import HeaderUS from "../header";

const MasterUsLayout = ({ children, ...props }) => {
  return (
    <div {...props}>
      <HeaderUS />
      {children}
      <FooterUS />
    </div>
  );
};

export default memo(MasterUsLayout);
