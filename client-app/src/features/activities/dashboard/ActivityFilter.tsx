import React from "react";
import Calendar from "react-calendar";
import { Header, Icon, Menu, Segment } from "semantic-ui-react";

const ActivityFilter = () => {
  return (
    <>
      <Menu vertical fluid>
        <Menu.Item>
          <Header>
            <Icon color="teal" name="filter" />
            <Header.Content>Filter</Header.Content>
          </Header>
        </Menu.Item>
        <Menu.Item>All Activities</Menu.Item>
        <Menu.Item>I'm Going</Menu.Item>
        <Menu.Item>I'm Hosting</Menu.Item>
      </Menu>
      <Segment>
        <Calendar />
      </Segment>
    </>
  );
};

export default ActivityFilter;
