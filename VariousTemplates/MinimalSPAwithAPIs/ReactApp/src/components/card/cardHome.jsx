import { Card, Col, Row } from "antd";
import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import myLogo from '../../Assets/img/logo-my-mobile.svg';

function CardHome() {

  const [cardList, setCardList] = useState([]);

  useEffect(() => {
    setCardList([
      {
            title: <>lorem ipsum</>,
            description: "lorem ipsum",
        imgUrl: myLogo,
        link: "/definizioneTempistiche"
      },
      {
          title: <>lorem ipsum</>,
          description: "lorem ipsum",
        imgUrl: myLogo,
        link: "/mappaturaProcessi"
      }
    ])
  }, []);

  const cardRender = cardList.map((item) => {
    return (<>
      <Col className="gutter-row" span={6}>
        <Link to={item.link}>
          <Card
            hoverable
            className="mt-5"
            cover={<img alt="example" src={item.imgUrl} />}
            style={{
              width: 325,
            }}

          >
            <Card.Meta title={item.title} description={item.description} />
          </Card>
        </Link>
      </Col>
    </>)
  })

  return (<>
    <div className="contenutoInEvidenza">
      <Card
        hoverable
        className="mt-5 cardPrincipale"
        cover={<img className="imgPrincipale" alt="example" src={myLogo} />}
      >
        <Card.Meta title={<h4>MinimalSPAwithAPI</h4>} description={<>
          <div>
            <h6>Lorem ipsum</h6>
            <span>
              Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nulla ut ultrices orci, ultrices convallis ipsum. Cras rutrum orci a urna<br />
              ullamcorper, non vulputate libero sagittis. Nunc sed libero sit amet ante efficitur ultricies. Proin neque nunc, egestas eu arcu venenatis,<br />
              finibus suscipit libero. Vestibulum leo purus, ullamcorper ut porta eu, vulputate ut nunc. In tempus erat non nunc aliquam sollicitudin.<br />
              Fusce facilisis egestas eros in vehicula. Vivamus euismod, felis non tempor tempor, libero est luctus quam, vel semper justo sapien sed lorem.<br />
            </span>
          </div>
        </>} />
      </Card>
    </div>
    <Row gutter={{ xs: 8, sm: 16, md: 24 }}>
      {cardRender}
    </Row>
  </>)

}

export default CardHome;
