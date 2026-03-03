import { Menu } from "antd";
import React, { useState } from "react";

function Footer() {
  const [current, setCurrent] = useState("mail");

  return (
    <Menu className="footerBG" selectedKeys={[current]} theme="dark" mode="horizontal" />
  );
}

export default Footer;
