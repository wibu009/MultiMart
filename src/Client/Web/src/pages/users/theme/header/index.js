import { memo, useEffect, useState, useContext } from "react";
import {
  AiOutlineDownCircle,
  AiOutlineFacebook,
  AiOutlineInstagram,
  AiOutlineLinkedin,
  AiOutlineMenu,
  AiOutlinePhone,
  AiOutlineShoppingCart,
  AiOutlineTwitter,
  AiOutlineUpCircle,
} from "react-icons/ai";
import { FaRegBell } from "react-icons/fa";
import { BiMailSend, BiUser } from "react-icons/bi";
import { IoIosLogOut } from "react-icons/io";
import "react-multi-carousel/lib/styles.css";
import { Link, useLocation, useNavigate } from "react-router-dom";
import { formatter } from "utils/formater";
import { ROUTERS } from "utils/router";
import "./style.scss";
import { categories } from "utils/common";
import { useCart } from "pages/users/shoppingCartPage/CartContext";
import { toast, ToastContainer } from "react-toastify";
import { UserContext } from "../../../../context/UserContext";

const HeaderUS = () => {
  const location = useLocation();
  const navigate = useNavigate();
  const { cartItems } = useCart();

  const [isShowHumberger, setIsShowHumberger] = useState(false);
  const [isHome, setIsHome] = useState(location.pathname.length <= 1);
  const [isShowCategories, setIsShowCategories] = useState(isHome);
  const [selectedMenu, setSelectedMenu] = useState(null);
  const [selectedDropdown, setSelectedDropdown] = useState(null);
  const [notifications] = useState(3);

  const { logout, user } = useContext(UserContext);

  console.log('checkkk usserrr>>>>>>>>>>>', user);

  const handleLogout = () => {
    logout();
    navigate("/");
    toast.success('Đăng xuất thành công!')
  }


  useEffect(() => {
    const savedSelectedMenu = localStorage.getItem("selectedMenu");
    const savedSelectedDropdown = localStorage.getItem("selectedDropdown");

    if (savedSelectedMenu) {
      setSelectedMenu(parseInt(savedSelectedMenu, 10));
    }

    if (savedSelectedDropdown) {
      setSelectedDropdown(savedSelectedDropdown);
    }
  }, []);


  useEffect(() => {
    if (selectedMenu !== null) {
      localStorage.setItem("selectedMenu", selectedMenu);
    }

    if (selectedDropdown !== null) {
      localStorage.setItem("selectedDropdown", selectedDropdown);
    }
  }, [selectedMenu, selectedDropdown]);

  const [menus, setMenus] = useState([
    {
      name: "Trang chủ",
      path: ROUTERS.USER.HOME,
    },
    {
      name: "Sản phẩm",
      path: ROUTERS.USER.PRODUCTS,
    },
    {
      name: "Cửa hàng",
      path: "",
      isShoSubMenu: false,
      child: [
        {
          name: "Sách",
          path: "",
        },
        {
          name: "Manga",
          path: "",
        },
        {
          name: "Tiểu thuyết",
          path: "",
        },
      ],
    },
    {
      name: "Tài khoản",
      path: "",
      isShoSubMenu: false,
      child: [
        {
          name: "Thông tin cá nhân",
          path: "",
        },
        {
          name: "Trạng thái đơn hàng",
          path: "/order-status",
        },
      ],
    },
    {
      name: "Thông tin",
      path: "",
    },
  ]);

  useEffect(() => {
    const isHome = location.pathname.length <= 1;
    setIsHome(isHome);
    setIsShowCategories(isHome);
  }, [location]);

  const totalAmount = cartItems.reduce((total, item) => total + item.price * item.quantity, 0);
  const totalQuantity = cartItems.reduce((total, item) => total + item.quantity, 0);

  return (
    <>
      {/* Humberger Begin */}
      <div
        className={`humberger__menu__overlay ${isShowHumberger ? "active" : ""
          }`}
        onClick={() => setIsShowHumberger(false)}
      />
      <div
        className={`humberger__menu__wrapper ${isShowHumberger ? "show__humberger__menu__wrapper" : ""
          }`}
      >
        <div
          className="header__logo"
          onClick={() => navigate(ROUTERS.USER.HOME)}
        >
          <h1>BookStack</h1>
        </div>
        <div className="humberger__menu__cart">
          <ul>
            <li>
              <Link to={ROUTERS.USER.SHOPPING_CART}>
                <AiOutlineShoppingCart /> <span>{totalQuantity}</span>
              </Link>
            </li>
          </ul>
          <div className="header__cart__price">
            Giỏ hàng: <span>{formatter(totalAmount)}</span>
          </div>
        </div>
        <div className="humberger__menu__widget">
          <div className="header__top__right__auth">
            {user && user.auth === true ? <Link to="/logout">
              <BiUser /> Đăng xuất
            </Link> : <Link to="/login">
              <BiUser /> Đăng nhập
            </Link>}
          </div>
        </div>
        <nav className="humberger__menu__nav">
          <ul>
            {menus.map((menu, menuKey) => (
              <li key={menuKey}>
                <Link
                  to={menu.path}
                  onClick={() => {
                    const newMenus = [...menus];
                    newMenus[menuKey].isShoSubMenu =
                      !newMenus[menuKey].isShoSubMenu;
                    setMenus(newMenus);
                  }}
                >
                  {menu.name}{" "}
                  {menu.child &&
                    (menu.isShoSubMenu ? (
                      <AiOutlineDownCircle />
                    ) : (
                      <AiOutlineUpCircle />
                    ))}
                </Link>
                {menu.child && (
                  <ul
                    className={`header__menu__dropdown ${menu.isShoSubMenu ? "show__submenu" : ""
                      }`}
                  >
                    {menu.child.map((childItem, childKey) => (
                      <li key={`${menuKey}-${childKey}`}>
                        <Link to={childItem.path}>{childItem.name}</Link>
                      </li>
                    ))}
                  </ul>
                )}
              </li>
            ))}
          </ul>
        </nav>
        <div className="header__top__right__social">
          <Link to="">
            <AiOutlineFacebook />
          </Link>
          <Link to="">
            <AiOutlineInstagram />
          </Link>
          <Link to="">
            <AiOutlineLinkedin />
          </Link>
          <Link to="">
            <AiOutlineTwitter />
          </Link>
        </div>
        <div className="humberger__menu__contact">
          <ul>
            <li>
              <i className="fa fa-envelope"></i> bookstack@gmail.com
            </li>
            <li>Miễn phí ship đơn từ {formatter(200000)}</li>
          </ul>
        </div>
      </div>
      {/* Humberger End */}

      <header className="header">
        <ToastContainer
          position="top-right"
          autoClose={5000}
          hideProgressBar={false}
          newestOnTop={false}
          closeOnClick
          rtl={false}
          pauseOnFocusLoss
          draggable
          pauseOnHover
          theme="light"
        />
        {/* Humberger Begin */}
        <div className="header__top">
          <div className="container">
            <div className="row">
              <div className="col-lg-6">
                <div className="header__top__left">
                  <ul>
                    <li>
                      <BiMailSend /> bookstack@gmail.com
                    </li>
                    <li>Miễn phí ship đơn từ {formatter(200000)}</li>
                  </ul>
                </div>
              </div>
              <div className="col-lg-6">
                <div className="header__top__right">
                  <div className="header__top__right__social">
                    <ul>
                      {user && user.email && <span style={{ marginRight: 20, marginTop: -5 }}>Xin chào: {user.email}</span>}
                      <li>
                        <Link to="">
                          <AiOutlineFacebook />
                        </Link>
                      </li>
                      <li>
                        <Link to="">
                          <AiOutlineInstagram />
                        </Link>
                      </li>
                      <li>
                        <Link to="">
                          <AiOutlineLinkedin />
                        </Link>
                      </li>
                      <li>
                        <Link to="">
                          <AiOutlineTwitter />
                        </Link>
                      </li>
                      <li>
                        {user && user.auth === true ?
                          <Link className="logout-link" onClick={() => handleLogout()}>
                            <IoIosLogOut />
                            <span className="logout-text">Đăng xuất</span>
                          </Link> :
                          <Link to="/login">
                            <BiUser /> Đăng nhập
                          </Link>}
                      </li>
                    </ul>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
        {/* Humberger End */}
        {/* Header Begin */}
        <div className="container">
          <div className="row">
            <div className="col-lg-3">
              <div
                className="header__logo"
                onClick={() => navigate(ROUTERS.USER.HOME)}
              >
                <h1>BookStack</h1>
              </div>
            </div>
            <div className="col-lg-6">
              <nav className="header__menu">
                <ul>
                  {menus.map((menu, menuKey) => (
                    <li key={menuKey} className={`${menuKey === 0 ? "active" : ""} ${selectedMenu === menuKey ? "selected" : ""}`}>
                      <Link
                        to={menu.path}
                        onClick={(e) => {
                          const newMenus = [...menus];
                          newMenus[menuKey].isShoSubMenu =
                            !newMenus[menuKey].isShoSubMenu;
                          setMenus(newMenus);
                          setSelectedMenu(menuKey);
                          setSelectedDropdown(null);
                        }}
                      >
                        {menu.name}
                      </Link>
                      {menu.child && (
                        <ul className="header__menu__dropdown">
                          {menu.child.map((childItem, childKey) => (
                            <li key={`${menuKey}-${childKey}`} className={selectedDropdown === `${menuKey}-${childKey}` ? "selected-dropdown" : ""}>
                              <Link to={childItem.path}
                                onClick={(e) => {
                                  setSelectedDropdown(`${menuKey}-${childKey}`);
                                }}>{childItem.name}</Link>
                            </li>
                          ))}
                        </ul>
                      )}
                    </li>
                  ))}
                </ul>
              </nav>
            </div>
            <div className="col-lg-3">
              <div className="header__cart">
                <div className="header__cart__price">
                  <span>{formatter(totalAmount)}</span>
                </div>
                <ul>
                  <li>
                    <Link to={ROUTERS.USER.SHOPPING_CART}>
                      <AiOutlineShoppingCart /> <span>{totalQuantity}</span>
                    </Link>
                  </li>
                  <li>
                    <Link >
                      <FaRegBell /> <span>{notifications}</span>
                    </Link>
                  </li>
                </ul>
              </div>
              <div className="humberger__open">
                <AiOutlineMenu onClick={() => setIsShowHumberger(true)} />
              </div>
            </div>
          </div>
        </div>
        {/* Header End */}
      </header>
      {/* Hero Begin */}
      <div className="container">
        <div className="row">
          <div className="col-lg-3 hero__categories_container">
            <div className="hero__categories">
              <div
                className="hero__categories__all"
                onClick={() => setIsShowCategories(!isShowCategories)}
              >
                <AiOutlineMenu />
                <span>Danh sách sản phẩm</span>
              </div>
              <ul className={isShowCategories ? "" : "hidden"}>
                <li>
                  {categories.map((name, key) => (
                    <Link to={ROUTERS.USER.PRODUCTS} key={key}>
                      {name}
                    </Link>
                  ))}
                </li>
              </ul>
            </div>
          </div>
          <div className="col-lg-9 hero__search__container">
            <div className="hero__search">
              <div className="hero__search__form">
                <form>
                  <div className="hero__search__form__group">
                    <input type="text" placeholder="Bạn đang tìm gì?" />
                    <button type="submit" className="site-btn">
                      Tìm kiếm
                    </button>
                  </div>
                </form>
              </div>
              <div className="hero__search__phone">
                <div className="hero__search__phone__icon">
                  <AiOutlinePhone />
                </div>
                <div className="hero__search__phone__text">
                  <p>0398204444</p>
                  <span>Hỗ trợ 24/7</span>
                </div>
              </div>
            </div>
            {isHome && (
              <div className="hero__item">
                <div className="hero__text">
                  <span>Siêu khuyến mãi</span>
                  <h2>
                    Lên tới <br />
                    100%
                  </h2>
                  <p>Miễn phí giao hàng tận nơi.</p>
                  <Link to="" className="primary-btn">
                    Mua ngay
                  </Link>
                </div>
              </div>
            )}
          </div>
        </div>
      </div>
      {/* Hero End */}
    </>
  );
};

export default memo(HeaderUS);
