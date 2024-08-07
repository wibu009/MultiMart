import BreadcrumbUS from "pages/users/theme/breadcrumb";
import { memo } from "react";
import "./style.scss";
import { Quantity } from "component";

import { AiOutlineClose } from "react-icons/ai";
import { formatter } from "utils/formater";
import { useNavigate } from "react-router-dom";
import { ROUTERS } from "utils/router";
import { useCart } from "./CartContext";

const ShoppingCartPage = () => {
  const navigate = useNavigate();
  const { cartItems, removeFromCart ,updateCartItemQuantity  } = useCart();

  // const updateCartItemQuantity = (itemId, newQuantity) => {
  //   console.log("Updating item", itemId, "quantity to", newQuantity);
  // };

  // const totalAmount = cartItems.reduce((total, item) => total + item.price * item.quantity, 0);
  // const totalQuantity = cartItems.reduce((total, item) => total + item.quantity, 0);

  const totalAmount = cartItems.reduce((total, item) => {
    if (!isNaN(item.price) && !isNaN(item.quantity)) {
      return total + item.price * item.quantity;
    }
    return total;
  }, 0);

  const totalQuantity = cartItems.reduce((total, item) => {
    if (!isNaN(item.quantity)) {
      return total + item.quantity;
    }
    return total;
  }, 0);

  return (
    <>
      <BreadcrumbUS name={"Giỏ hàng"} />
      <div className="container">
        <div className="table__cart">
          <table>
            <thead>
              <tr>
                <th>Tên</th>
                <th>Giá</th>
                <th>Số lượng</th>
                <th>Thành tiền</th>
                <th />
              </tr>
            </thead>
            <tbody>
              {cartItems.map((item, key) => (
                <tr key={key}>
                  <td className="shoping__cart__item">
                    <img src={item.img[0]} alt="product-pic" />
                    <h4>{`${item.name}`}</h4>
                  </td>
                  <td>{!isNaN(item.price) ? formatter(item.price) : "Invalid Price"}</td>
                  <td>
                    <Quantity
                      quantity={item.quantity}
                      onQuantityChange={(newQuantity) => updateCartItemQuantity(item.id, newQuantity)}
                      hasAddToCart={false}
                    />
                  </td>
                  <td>{!isNaN(item.price) && !isNaN(item.quantity) ? formatter(item.price * item.quantity) : "Invalid Amount"}</td>
                  <td className="icon_close" onClick={() => removeFromCart(item.id)}>
                    <AiOutlineClose />
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
        <div className="row">
          <div className="col-lg-6 col-md-12 col-sm-12 col-xs-12">
            <div className="shopping__continue">
              <h3>Mã giảm giá:</h3>
              <div className="shopping__discount">
                <input placeholder="Nhập mã giảm giá" />
                <button type="button" className="button-submit">
                  Áp dụng
                </button>
              </div>
            </div>
          </div>
          <div className="col-lg-6 col-md-12 col-sm-12 col-xs-12">
            <div className="shopping__checkout">
              <h2>Tổng đơn:</h2>
              <ul>
                <li>
                  Giá: <span>{formatter(totalAmount)}</span>
                </li>
                <li>
                  Số lượng: <span>{totalQuantity}</span>
                </li>
                <li>
                  Thành tiền: <span>{formatter(totalAmount)}</span>
                </li>
              </ul>
              <button
                type="button"
                className="button-submit"
                onClick={() => navigate(ROUTERS.USER.CHECKOUT)}
              >
                Thanh toán
              </button>
            </div>
          </div>
        </div>
      </div>
    </>
  );
};

export default memo(ShoppingCartPage);
