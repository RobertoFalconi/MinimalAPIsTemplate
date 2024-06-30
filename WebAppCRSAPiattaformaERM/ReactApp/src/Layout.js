import React from "react";
import { Container } from "reactstrap";
import Footer from "./components/footer/Footer.jsx";
import Header from "./components/header/Header.jsx";

export function Layout(props) {
  const displayName = Layout.name;

    return (
      <div>
        {/* <NavMenu /> importare header sirio*/}
        <Header />
        <Container fluid tag="main">{props.children}</Container>
        <Footer />
      </div>
    );
  }

