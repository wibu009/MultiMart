import feaImg2 from "assets/users/images/featured/jjk-vol11.jpg";
import feaImg3 from "assets/users/images/featured/kny.jpg";
import feaImg4 from "assets/users/images/featured/hero-academy.jpg";
import feaImg11 from "assets/users/images/featured/fire_force.jpg";
import feaImg12 from "assets/users/images/featured/doraemon.jpg";

import React, { memo} from "react";
import Carousel from "react-multi-carousel";
import { Tab, TabList, TabPanel, Tabs } from "react-tabs";
import "./style.scss";
import { featProducts } from "utils/common";
import { ProductCard } from "component";
// import { getAllProducts, getUsers, getProductById } from "api/api";


// if (loading) {
//   return <p>Loading...</p>;
// }

// if (error) {
//   return <p>Error: {error.message}</p>;
// }

const HomePage = () => {

  // const [products, setProducts] = useState([]);
  // const [featuredProduct, setFeaturedProduct] = useState(null);
  // const [users, setUsers] = useState([]);
  // const [loading, setLoading] = useState(true);
  // const [error, setError] = useState(null);
  // console.log('product', products)

  // useEffect(() => {
  //   const fetchData = async () => {
  //     try {
  //       const productsData = await getAllProducts();
  //       const usersData = await getUsers();
  //       // const featuredProductData = await getProductById(1);
  //       setProducts(productsData);
  //       // setFeaturedProduct(featuredProductData);
  //       setUsers(usersData);
  //     } catch (error) {
  //       setError(error);
  //     } finally {
  //       setLoading(false);
  //     }
  //   };

  //   fetchData();
  // }, []);

  // if (loading) {
  //   return <p>Loading...</p>;
  // }

  // if (error) {
  //   return <p>Error: {error.message}</p>;
  // }


  const responsive = {
    superLargeDesktop: {
      breakpoint: { max: 4000, min: 3000 },
      items: 5,
    },
    desktop: {
      breakpoint: { max: 3000, min: 1024 },
      items: 4,
    },
    tablet: {
      breakpoint: { max: 1024, min: 464 },
      items: 2,
    },
    mobile: {
      breakpoint: { max: 464, min: 0 },
      items: 1,
    },
  };

  const renderFeaturedProducts = (data) => {
    const tabList = [];
    const tabPanels = [];

    Object.keys(data).forEach((key, index) => {
      tabList.push(<Tab key={index}>{data[key].title}</Tab>);

      const tabPanel = [];
      data[key].products.forEach((item, index) => {
        tabPanel.push(
          <div className="col-lg-3 col-md-4 col-sm-6 col-xs-12" key={index}>
            <ProductCard id={item.id} img={item.img} name={item.name} price={item.price} />
          </div>
        );
      });

      tabPanels.push(tabPanel);
    });

    return (
      <Tabs defaultChecked={1}>
        <TabList>{tabList}</TabList>
        {tabPanels.map((item, index) => (
          <TabPanel key={index}>
            <div className="row">{item}</div>
          </TabPanel>
        ))}
      </Tabs>
    );
  };

  const sliderItems = [
    {
      bgImage: feaImg2,
      name: "Jujutsu Kaisen",
    },
    {
      bgImage: feaImg3,
      name: "Kimetsu no Yaiba",
    },
    {
      bgImage: feaImg12,
      name: "Doraemon",
    },
    {
      bgImage: feaImg11,
      name: "Fire Force",
    },
    {
      bgImage: feaImg4,
      name: "My Hero Academia",
    },
  ];

  return (
    <>
      {/* Categories Begin */}
      <div className="container container__categories__slider">
        <Carousel
          responsive={responsive}
          className="categories__slider"
        // autoPlay
        >
          {sliderItems.map((item, key) => (
            <div
              key={key}
              className="categories__slider__item"
              style={{ backgroundImage: `url(${item.bgImage})` }}
            >
              <p>{item.name}</p>
            </div>
          ))}
        </Carousel>
      </div>
      {/* Categories  End */}
      {/* Featured Begin */}
      <div className="container">
        <div className="featured">
          <div className="section-title">
            <h2>Sản phẩm nổi bật</h2>
          </div>
          {renderFeaturedProducts(featProducts)}
        </div>
      </div>
      {/* Featured End */}

      {/* Banner Begin */}
      {/* <div className="container">
        {users.map(item => 
        <h2 key={item.id}>
         {item.password}
        </h2>
        )
        }
      </div> */}
      {/* Banner End */}
    </>
  );
};

export default memo(HomePage);
