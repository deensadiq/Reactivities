import React from "react";
import { Menu, Button, Container } from "semantic-ui-react";

interface IProps {
  openCreateForm: () => void;
}

const NavBar: React.FC<IProps> = ({openCreateForm}) => {
  return (
    <Menu fixed="top" inverted>
      <Container>
        <Menu.Item>
          <img src="/assets/logo.png" alt="logo" style={{marginRight: '10px'}} />
          Reactivities
        </Menu.Item>
        <Menu.Item name="Activties" />
        <Menu.Item>
          <Button onClick={() => openCreateForm()} positive content="Create Activity" />
        </Menu.Item>
      </Container>
    </Menu>
  );
}

export default NavBar;
