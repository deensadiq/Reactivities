import { observer } from "mobx-react-lite";
import React, { useContext } from "react";
import Calendar from "react-calendar";
import { Header, Menu } from "semantic-ui-react";
import { RootStoreContext } from "../../../app/stores/rootStore";

const ActivityFilter = () => {
  const rootStore = useContext(RootStoreContext);
  const {
    activityStore: { setPredicate, predicate },
  } = rootStore;
  return (
    <>
      <Menu vertical size="large" style={{ width: "100%", marginTop: "50px" }}>
        <Header icon="filter" attached color="teal" content="Filters" />
        <Menu.Item
          active={predicate.has("all")}
          onClick={() => setPredicate("all", "true")}
          content="All Activities"
        />
        <Menu.Item
          active={predicate.has("isGoing")}
          onClick={() => setPredicate("isGoing", "true")}
          content="I'm Going"
        />
        <Menu.Item
          active={predicate.has("isHost")}
          onClick={() => setPredicate("isHost", "true")}
          content="I'm Hosting"
        />
      </Menu>
      <Header />
      <Calendar
        value={predicate.get("startDate") || new Date()}
        onChange={(date: Date) => setPredicate("startDate", date)}
      />
    </>
  );
};

export default observer(ActivityFilter);
