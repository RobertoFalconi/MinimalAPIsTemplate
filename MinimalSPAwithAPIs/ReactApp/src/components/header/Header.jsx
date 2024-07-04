
import { EnvironmentOutlined, QuestionCircleOutlined } from '@ant-design/icons';
import { Menu } from 'antd';
import React, { useState } from 'react';

const items = [
  {
    label: "A",
    key: 'A',
  },
  {
    label: "B",
    key: 'B',
  },
  {
    label: "C",
    key: 'C',
  },
  {
    label: "D",
    key: 'D',
  }
];

function Header() {
  const [current, setCurrent] = useState('mail');

  const onClick = (e) => {
    console.log('click ', e);
    setCurrent(e.key);
  };

  return (
    <>
      <Menu theme="dark" onClick={onClick} selectedKeys={[current]} mode="horizontal" items={items} />
    </>
  );
};

export default Header;