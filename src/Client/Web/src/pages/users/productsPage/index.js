import { memo, useState, useEffect } from "react";
import BreadcrumbUS from "pages/users/theme/breadcrumb";
import "./style.scss";
import { Link } from "react-router-dom";
import { categories } from "utils/common";
import { ROUTERS } from "utils/router";
import { ProductCard } from "component";
import { allProducts } from "utils/allProducts";
import ReactPaginate from "react-paginate";

const ProductsPage = () => {

  const [products, setProducts] = useState([]);
  const [currentPage, setCurrentPage] = useState(0);
  const [pageCount, setPageCount] = useState(0);
  const productsPerPage = 12;

  // useEffect(() => {
  //   const fetchProducts = async () => {
  //     try {
  //       const response = await fetch(`/api/users?page=${currentPage + 1}&limit=${productsPerPage}`);
  //       const data = await response.json();
  //       setProducts(data.products);
  //       setPageCount(Math.ceil(data.total / productsPerPage));
  //     } catch (error) {
  //       console.error("Failed to fetch products", error);
  //     }
  //   };
  //   fetchProducts();
  // }, [currentPage]);

  const handlePageClick = ({ selected }) => {
    setCurrentPage(selected);
  };

  const sorts = [
    "Giá thấp đến cao",
    "Giá cao đến thấp",
    "Mới đến cũ",
    "Cũ đến mới",
    "Bán chạy nhất",
    "Đang giảm giá",
  ];


  return (
    <>
      <BreadcrumbUS name={"Sản phẩm"} />
      <div className="container">
        <div className="row">
          <div className="col-lg-3 col-md-12 col-sm-12 col-xs-12">
            <div className="sidebar">
              <div className="sidebar__item">
                <h2>Tìm kiếm</h2>
                <input type="text" />
              </div>
              <div className="sidebar__item">
                <h2>Mức giá</h2>
                <div className="price-range-wrap">
                  <div>
                    <p>Từ:</p>
                    <input min={0} type="number" />
                  </div>
                  <div>
                    <p>Đến:</p>
                    <input min={0} type="number" />
                  </div>
                </div>
              </div>
              <div className="sidebar__item">
                <h2>Sắp xếp</h2>
                <div className="tags">
                  {sorts.map((item, key) => (
                    <div
                      className={`tag ${key === 0 ? "active" : ""}`}
                      key={key}
                    >
                      {item}
                    </div>
                  ))}
                </div>
              </div>
              <div className="sidebar__item">
                <h2>Thể loại khác</h2>
                <ul>
                  {categories.map((name, key) => (
                    <li key={key}>
                      <Link to={ROUTERS.USER.PRODUCTS} key={key}>
                        {name}
                      </Link>
                    </li>
                  ))}
                </ul>
              </div>
            </div>
          </div>
          <div className="col-lg-9 col-md-12 col-sm-12 col-xs-12">
            <div className="row pl-10">
              {allProducts.map((item, key) => (
                <div className="col-lg-4 col-md-4 col-sm-6 col-xs-12" key={key}>
                  <ProductCard
                    id={item.id}
                    img={item.img}
                    name={item.name}
                    price={item.price}
                  />
                </div>
              ))}
            </div>
            <ReactPaginate
              previousLabel={"Trang trước"}
              nextLabel={"Trang sau"}
              breakLabel={"..."}
              breakClassName={"break-me"}
              pageCount={pageCount}
              marginPagesDisplayed={2}
              pageRangeDisplayed={5}
              onPageChange={handlePageClick}
              containerClassName={"pagination"}
              subContainerClassName={"pages pagination"}
              activeClassName={"active"}
            />
          </div>
        </div>
      </div>
    </>
  );
};

export default memo(ProductsPage);
