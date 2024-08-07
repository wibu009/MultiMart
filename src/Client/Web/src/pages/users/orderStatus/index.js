// src/OrderStatus.js
import React, { memo, useState } from 'react';
import "./style.scss";

const OrderStatus = () => {
    const [activeTab, setActiveTab] = useState('choxuly');

    const orders = [
        {
            id: 1,
            image: 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRRpIB2GjEDm7Jx6673kUWjFocuTa9H4t08HA&s',
            name: 'Sachs sachs va sachs',
            quantity: 1,
            price: 8000000,
            status: 'Đã thanh toán',
            statusTab: 'choxuly',
        },
        {
            id: 2,
            image: 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQH3MEvncGVrRr47pb3FwEwBfcxoQdIWNUnEg&s',
            name: 'Truyen',
            quantity: 1,
            price: 28000000,
            status: 'Cho xu ly',
            statusTab: 'danggiao',
        },
        {
            id: 3,
            image: 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQH3MEvncGVrRr47pb3FwEwBfcxoQdIWNUnEg&s',
            name: 'Chu thuat hoi chien',
            quantity: 10,
            price: 99999,
            status: 'Cho xu ly',
            statusTab: 'dagiao',
        },
    ];

    const handleTabClick = (e, tab) => {
        e.preventDefault();
        setActiveTab(tab);
      };

    return (
        <div className="container">
            <div className="tabs">
                <button  type="submit" className={`tab ${activeTab === 'choxuly' ? 'active' : ''}`} onClick={(e) => handleTabClick(e, 'choxuly')}>Chờ xử lý</button>
                <button  type="submit" className={`tab ${activeTab === 'danggiao' ? 'active' : ''}`} onClick={(e) => handleTabClick(e, 'danggiao')}>Đang giao</button>
                <button  type="submit" className={`tab ${activeTab === 'dagiao' ? 'active' : ''}`} onClick={(e) => handleTabClick(e, 'dagiao')}>Đã giao</button>
            </div>
            <div className="order-list">
                {orders
                    .filter(order => order.statusTab === activeTab)
                    .map(order => (
                        <div className="order-item" key={order.id}>
                            <img className="order-image" src={order.image} alt={order.name} />
                            <div className="order-info">
                                <div>{order.name}</div>
                                <div>x{order.quantity}</div>
                                <div className="order-price">{order.price.toLocaleString()}đ</div>
                                <div>{order.status}</div>
                                <button type="button">Hủy đơn hàng</button>
                            </div>
                        </div>
                    ))}
            </div>
        </div>
    );
};

export default memo(OrderStatus);
