import { memo } from "react";
import { AiOutlineEye, AiOutlineShoppingCart } from "react-icons/ai";
import { Link, generatePath } from "react-router-dom";
import { formatter } from "utils/formater";
import "./style.scss";
import { ROUTERS } from "utils/router";
import { useCart } from '../../pages/users/shoppingCartPage/CartContext';
import { toast } from "react-toastify";

const ProductCard = ({ id, img, name, price, quantity }) => {
  const { addToCart } = useCart();
  if (!id) {
    console.error("ProductCard: Missing 'id' prop");
    return null;
  }
  const handleAddToCart = () => {
    addToCart({
      id,
      img,
      name,
      price,
      quantity
    });
    toast.success('Đã thêm sản phẩm vào giỏ hàng!');
  };
  return (
    <>
      <div className="featured__item pl-pr-10">
        <div
          className="featured__item__pic set-bg"
          style={{
            backgroundImage: `url(${img[0]})`,
          }}
        >
          <ul className="featured__item__pic__hover">
            <li>
              <Link to={generatePath(ROUTERS.USER.PRODUCT_DETAIL, { id })}>
                <AiOutlineEye />
              </Link>
            </li>
            <li onClick={handleAddToCart}>
              <AiOutlineShoppingCart />
            </li>
          </ul>
        </div>
        <div className="featured__item__text">
          <h6>
            <Link to={generatePath(ROUTERS.USER.PRODUCT_DETAIL, { id })}>
              {name}
            </Link>
          </h6>
          <h5>{formatter(price)}</h5>
        </div>
      </div>
    </>
  );
};

export default memo(ProductCard);
