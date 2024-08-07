import { memo } from "react";
import PropTypes from 'prop-types';
import "./style.scss";

const Quantity = ({ quantity, onQuantityChange, hasAddToCart = true, onAddToCart }) => {
  const handleQuantityChange = (newQuantity) => {
    if (newQuantity < 1) return; 
    onQuantityChange(newQuantity);
  };

  return (
    <div className="quantity-container">
      <div className="quantity">
        <span className="qtybtn" onClick={() => handleQuantityChange(quantity - 1)}>-</span>
        <input 
          type="number" 
          value={quantity} 
          onChange={(e) => handleQuantityChange(Number(e.target.value))} 
          min="1"
        />
        <span className="qtybtn" onClick={() => handleQuantityChange(quantity + 1)}>+</span>
      </div>
      {hasAddToCart && (
        <button type="submit" className="button-submit" onClick={onAddToCart}>
          Thêm giỏ hàng
        </button>
      )}
    </div>
  );
};

Quantity.propTypes = {
  quantity: PropTypes.number.isRequired,
  onQuantityChange: PropTypes.func.isRequired,
  onAddToCart: PropTypes.func,
  hasAddToCart: PropTypes.bool,
};

export default memo(Quantity);
