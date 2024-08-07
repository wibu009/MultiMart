import { memo } from "react";
import "react-multi-carousel/lib/styles.css";
import "./style.scss";
import { Link } from "react-router-dom";
import { ROUTERS } from "utils/router";

const BreadcrumbUS = (props) => {
  return (
    <div className="breadcrumb">
      <div className="breadcrumb__text">
        <h2>BookStack</h2>
        <div className="breadcrumb__option">
          <ul>
            <li className="link">
              <Link to={ROUTERS.USER.HOME}>Trang chủ</Link>
            </li>
            <li>{props.name}</li>
          </ul>
        </div>
      </div>
    </div>
  );
};

export default memo(BreadcrumbUS);
